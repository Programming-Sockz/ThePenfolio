using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThePenfolio.Server.Models;
using ThePenfolio.Server.Services;
using ThePenfolio.Shared.DTOs;
using ThePenfolio.Shared.Enums;
namespace ThePenfolio.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BookListController : ControllerBase
    {
        private readonly ThePenfolioDbContext _context;
        private readonly IMapper _mapper;

        public BookListController(ThePenfolioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        [HttpPost("addBook")]
        public async Task<IActionResult> AddBookToBookList(BookToBookListDTO bookToBookListDTO)
        {
            var bookList = await _context.BookList.FirstOrDefaultAsync(x=>x.ListType == bookToBookListDTO.ListType && x.CreatedById == bookToBookListDTO.UserId);

            if (bookList != null)
            {
                if (_context.BookBookList.Any(x => x.BookId == bookToBookListDTO.BookId && x.BookListId == bookList.Id))
                {
                    return Ok("Book already exists in the list");
                }
                
                _context.BookBookList.Add(new BookBookList
                {
                    BookId = bookToBookListDTO.BookId,
                    BookListId = bookList.Id
                });
                
                await _context.SaveChangesAsync();
                
                return Ok("Book added to the list");
            }
            
            var newBookList = new BookList
            {
                CreatedById = bookToBookListDTO.UserId,
                ListType = bookToBookListDTO.ListType,
                CreatedOn = DateTime.Now
            };
            
            _context.BookList.Add(newBookList);
            
            await _context.SaveChangesAsync();
            
            _context.BookBookList.Add(new BookBookList
            {
                BookId = bookToBookListDTO.BookId,
                BookListId = newBookList.Id
            });
            
            await _context.SaveChangesAsync();
            
            return Ok("Book added to the list");
        }
        
        [HttpDelete("removeBook")]
        public async Task<IActionResult> RemoveBookFromBookList(BookToBookListDTO removeBookFromBookListDTO)
        {
            var bookList = await _context.BookList.FirstOrDefaultAsync(x=>x.ListType == removeBookFromBookListDTO.ListType && x.CreatedById == removeBookFromBookListDTO.UserId);

            if (bookList == null)
            {
                return Ok("Book list not found");
            }

            var bookBookList = await _context.BookBookList.FirstOrDefaultAsync(x => x.BookId == removeBookFromBookListDTO.BookId && x.BookListId == bookList.Id);

            if (bookBookList == null)
            {
                return Ok("Book not found in the list");
            }

            _context.BookBookList.Remove(bookBookList);
            
            await _context.SaveChangesAsync();
            
            return Ok("Book removed from the list");
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<BookListDTO>> GetByUserId(Guid id, [FromQuery] ListTypes listType)
        {
            var bookList = await _context.BookList.FirstOrDefaultAsync(x=>x.CreatedById == id && x.ListType == listType);

            if (bookList == null)
            {
                return Ok(null);
            }

            var bookBookList = await _context.BookBookList.Where(x=>x.BookListId == bookList.Id).ToListAsync();

            BookListDTO bookListDTO = new()
            {
                Id = bookList.Id,
                CreatedById = bookList.CreatedById,
                CreatedOn = bookList.CreatedOn,
                ListType = bookList.ListType,
                Books = new List<BookDTO>()
            };
            
            foreach(var book in bookBookList)
            {
                bookListDTO.Books.Add(_mapper.Map<BookDTO>(await _context.Books
                    .Include(x => x.Author)
                    .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                    .Include(tg=>tg.BookTags)
                    .ThenInclude(t=>t.Tag)
                    .FirstOrDefaultAsync(x=>x.Id == book.BookId)));
            }
            
            return Ok(bookListDTO);
        }
    }
}

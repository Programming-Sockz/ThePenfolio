using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThePenfolio.Server.Models;
using ThePenfolio.Server.Services;
using ThePenfolio.Shared.DTOs;
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
        
        [HttpPost]
        public async Task<IActionResult> CreateBookList(BookListDTO bookListDTO)
        {
            var bookList = _mapper.Map<BookList>(bookListDTO);
            _context.BookList.Add(bookList);

            await _context.SaveChangesAsync();

            if (bookListDTO.Books.Any())
            {
                foreach (var book in bookListDTO.Books)
                {
                    _context.BookBookList.Add(new()
                    {
                        BookListId = bookList.Id,
                        BookId = book.Id
                    });
                }
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetBookLists()
        {
            var bookLists = await _context.BookList.ToListAsync();
            return Ok(_mapper.Map<List<BookListDTO>>(bookLists));
        }
    }
}

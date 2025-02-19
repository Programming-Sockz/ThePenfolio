using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThePenfolio.Server.Models;
using ThePenfolio.Server.Services;
using ThePenfolio.Shared.DTOs;
namespace ThePenfolio.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ThePenfolioDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(ThePenfolioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<BookDTO>> Get()
        {
            var books = await _context.Books.ToListAsync();

            var booksDTO = _mapper.Map<List<BookDTO>>(books);

            return booksDTO;
        }

        [HttpGet("name/{bookTitle}")]
        public async Task<ActionResult<List<BookDTO>>> GetByName(string bookTitle)
        {
            var books = await _context.Books.Where(x => x.Title.Contains(bookTitle))
                .Include(x => x.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Include(tg=>tg.BookTags)
                .ThenInclude(t=>t.Tag)
                .ToListAsync();
            
            if (books == null)
            {
                return new List<BookDTO>();
            }

            return Ok(books.Adapt<List<BookDTO>>());
        }

        [HttpGet("id/{bookId}")]
        public async Task<ActionResult<BookDTO>> GetById(Guid bookId)
        {
            var book = await _context.Books
                .Include(x => x.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Include(tg=>tg.BookTags)
                .ThenInclude(t=>t.Tag)
                .Include(b=>b.Chapters)
                .FirstOrDefaultAsync(x=>x.Id == bookId);

            if (book == null)
            {
                return NotFound();
            }
            
            foreach(var chapter in book.Chapters)
            {
                chapter.Content = "";
                chapter.AuthorNoteBottom = "";
                chapter.AuthorNoteTop = "";
            }

            return Ok(_mapper.Map<BookDTO>(book));
        }

        [HttpGet("reduced/id/{bookId}")]
        public async Task<ActionResult<BookDTO>> GetReducedById(Guid bookId)
        {
            var book = await _context.Books
                .Include(x => x.Author)
                .FirstOrDefaultAsync(x=>x.Id == bookId);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookDTO>(book));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BookDTO bookDTO)
        {
            if (bookDTO == null)
            {
                return BadRequest();
            }

            var book = _mapper.Map<Book>(bookDTO);
            
            _context.Books.Add(book);

            await _context.SaveChangesAsync();
            
            return Ok(book.Adapt<BookDTO>());
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> Put(Guid bookId, [FromBody] BookDTO bookDTO)
        {
            var dbBook = await _context.Books.FindAsync(bookId);

            if(dbBook == null)
            {
                return NotFound();
            }

            _mapper.From(bookDTO).AdaptTo(dbBook);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("author/{id}")]
        public async Task<ActionResult<BookDTO>> GetByAuthor(Guid id)
        {
            var book = await _context.Books
                .Where(x=>x.AuthorId == id)
                .Include(x => x.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Include(tg => tg.BookTags)
                .ThenInclude(t => t.Tag)
                .ToListAsync();

            if(book == null)
            {
                return NotFound();
            }

            return Ok(book.Adapt<List<BookDTO>>());
        }

        [HttpPost("search")]
        public async Task<List<BookDTO>> BookSearch([FromBody] BookSearchDTO search) 
        {
            var books = _context.Books
                .Include(x => x.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Include(tg=>tg.BookTags)
                .ThenInclude(t=>t.Tag)
                .AsQueryable();
            
            
            if (!string.IsNullOrEmpty(search.Title))
            {
                books = books.Where(x => x.Title.ToLower().Contains(search.Title.ToLower()));
            }
            
            if (search.AuthorId != Guid.Empty)
            {
                books = books.Where(x => x.AuthorId == search.AuthorId);
            }

            foreach (var includedGenre in search.IncludeGenres)
            {
                books = books.Where(x => x.BookGenres.Any(bg => bg.GenreId == includedGenre));
            }

            foreach (var excludedGenre in search.ExcludeGenres)
            {
                books = books.Where(x => !x.BookGenres.Any(bg => bg.GenreId == excludedGenre));
            }
            
            foreach(var includedTag in search.IncludeTags)
            {
                books = books.Where(x => x.BookTags.Any(bg => bg.TagId == includedTag));
            }
            
            foreach(var excludedTag in search.ExcludeTags)
            {
                books = books.Where(x => !x.BookTags.Any(bg => bg.TagId == excludedTag));
            }
            
            var searchResult = await books.ToListAsync();

            
            if (searchResult == null)
            {
                return new List<BookDTO>();
            }
            
            return searchResult.Adapt<List<BookDTO>>();
        }
    }
}

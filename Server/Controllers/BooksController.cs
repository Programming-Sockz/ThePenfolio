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
    public class BooksController(ThePenfolioDbContext context, IMapper mapper) : ControllerBase
    {

        [HttpGet]
        public async Task<List<BookDTO>> Get()
        {
            var books = await context.Books.ToListAsync();

            var booksDTO = mapper.Map<List<BookDTO>>(books);

            return booksDTO;
        }

        [HttpGet("name/{bookTitle}")]
        public async Task<ActionResult<List<BookDTO>>> GetByName(string bookTitle)
        {
            var books = await context.Books.Where(x => x.Title.Contains(bookTitle))
                .Include(x => x.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Include(tg=>tg.BookTags)
                .ThenInclude(t=>t.Tag)
                .Include(b=>b.Chapters)
                .ToListAsync();
            
            if (books == null)
            {
                return new List<BookDTO>();
            }
            
            books = books.Select(book => 
            {
                book.Chapters = book.Chapters
                    .Where(x => x.ReleasedOn != null && x.ReleasedOn < DateTime.Now)
                    .Select(x => new Chapter
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Content = "",
                        BookId = x.BookId,
                        AuthorNoteBottom = "",
                        AuthorNoteTop = "",
                        ReleasedOn = x.ReleasedOn,
                        CreatedOn = x.CreatedOn
                    }).ToList();
                return book;
            }).ToList();

            return Ok(books.Adapt<List<BookDTO>>());
        }

        [HttpGet("id/{bookId}")]
        public async Task<ActionResult<BookDTO>> GetById(Guid bookId)
        {
            var book = await context.Books
                .Include(x => x.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Include(tg=>tg.BookTags)
                .ThenInclude(t=>t.Tag)
                .Include(b=>b.Chapters)
                .ThenInclude(c=>c.ChapterUserLikes)
                .FirstOrDefaultAsync(x=>x.Id == bookId);

            if (book == null)
            {
                return NotFound();
            }
            
            book.Chapters = book.Chapters
                .Where(x => x.ReleasedOn != null && x.ReleasedOn < DateTime.Now)
                .Select(x => new Chapter
                {
                    Id = x.Id,
                    Name = x.Name,
                    Content = "",
                    BookId = x.BookId,
                    AuthorNoteBottom = "",
                    AuthorNoteTop = "",
                    ReleasedOn = x.ReleasedOn,
                    CreatedOn = x.CreatedOn,
                    ChapterUserLikes = x.ChapterUserLikes
                }).ToList();

            return Ok(book.Adapt<BookDTO>());
        }

        [HttpGet("reduced/id/{bookId}")]
        public async Task<ActionResult<BookDTO>> GetReducedById(Guid bookId)
        {
            var book = await context.Books
                .Include(x => x.Author)
                .FirstOrDefaultAsync(x=>x.Id == bookId);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<BookDTO>(book));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BookDTO bookDTO)
        {
            if (bookDTO == null)
            {
                return BadRequest();
            }

            var book = mapper.Map<Book>(bookDTO);
            
            context.Books.Add(book);

            await context.SaveChangesAsync();

            foreach (var genre in bookDTO.Genres)
            {
                context.BookGenre.Add(new()
                {
                    BookId = book.Id,
                    GenreId = genre.Id
                });
            }

            foreach (var genre in bookDTO.Tags)
            {
                context.BookTags.Add(new()
                {
                    BookId = book.Id,
                    TagId = genre.Id
                });
            }
            
            await context.SaveChangesAsync();
            
            return Ok();
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> Put(Guid bookId, [FromBody] BookDTO bookDTO)
        {
            var dbBook = await context.Books.FindAsync(bookId);

            if(dbBook == null)
            {
                return NotFound();
            }

            bookDTO.Author = null;
            
            bookDTO.Chapters = null;
            
            mapper.From(bookDTO).AdaptTo(dbBook);

            await context.SaveChangesAsync();
            
            context.BookGenre.RemoveRange(context.BookGenre.Where(x => x.BookId == dbBook.Id));
            
            foreach (var genre in bookDTO.Genres)
            {
                context.BookGenre.Add(new()
                {
                    BookId = dbBook.Id,
                    GenreId = genre.Id
                });
            }
            
            context.BookTags.RemoveRange(context.BookTags.Where(x => x.BookId == dbBook.Id));

            foreach (var genre in bookDTO.Tags)
            {
                context.BookTags.Add(new()
                {
                    BookId = dbBook.Id,
                    TagId = genre.Id
                });
            }
            
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("author/{id}")]
        public async Task<ActionResult<List<BookDTO>>> GetByAuthor(Guid id, [FromQuery] bool isAuthorView = false)
        {
            var books = await context.Books
                .Where(x=>x.AuthorId == id)
                .Include(x => x.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Include(tg => tg.BookTags)
                .ThenInclude(t => t.Tag)
                .Include(b=>b.Chapters)
                .ThenInclude(c=>c.ChapterUserLikes)
                .ToListAsync();

            if(books == null)
            {
                return NotFound();
            }

            if(isAuthorView)
            {
                books = books.Select(book =>
                {
                    book.Chapters = book.Chapters
                        .Select(x => new Chapter
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Content = "",
                            BookId = x.BookId,
                            AuthorNoteBottom = "",
                            AuthorNoteTop = "",
                            ReleasedOn = x.ReleasedOn,
                            CreatedOn = x.CreatedOn,
                            LastEditedOn = x.LastEditedOn,
                            ChapterUserLikes = x.ChapterUserLikes
                        }).ToList();
                    return book;
                }).ToList();
            }
            else
            {
                books = books.Select(book =>
                {
                    book.Chapters = book.Chapters
                        .Where(x => x.ReleasedOn != null && x.ReleasedOn < DateTime.Now)
                        .Select(x => new Chapter
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Content = "",
                            BookId = x.BookId,
                            AuthorNoteBottom = "",
                            AuthorNoteTop = "",
                            ReleasedOn = x.ReleasedOn,
                            CreatedOn = x.CreatedOn,
                            LastEditedOn = x.LastEditedOn,
                            ChapterUserLikes = x.ChapterUserLikes
                        }).ToList();
                    return book;
                }).ToList();
            }
            
            return Ok(books.Adapt<List<BookDTO>>());
        }

        [HttpPost("search")]
        public async Task<List<BookDTO>> BookSearch([FromBody] BookSearchDTO search) 
        {
            var books = context.Books
                .Include(x => x.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Include(tg=>tg.BookTags)
                .ThenInclude(t=>t.Tag)
                .Include(b=>b.Chapters)
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
            
            searchResult = searchResult.Select(book => 
            {
                book.Chapters = book.Chapters
                    .Where(x => x.ReleasedOn != null && x.ReleasedOn < DateTime.Now)
                    .Select(x => new Chapter
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Content = "",
                        BookId = x.BookId,
                        AuthorNoteBottom = "",
                        AuthorNoteTop = "",
                        ReleasedOn = x.ReleasedOn,
                        CreatedOn = x.CreatedOn
                    }).ToList();
                return book;
            }).ToList();
            
            return searchResult.Adapt<List<BookDTO>>();
        }
    }
}

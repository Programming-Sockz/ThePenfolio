using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThePenfolio.Server.Models;
using ThePenfolio.Server.Services;
using ThePenfolio.Shared.DTOs;
namespace ThePenfolio.Server.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    public class GenresController : ControllerBase
    {
        private readonly ThePenfolioDbContext _context;
        private readonly IMapper _mapper;

        public GenresController(ThePenfolioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<List<GenreDTO>> GetGenres()
        {
            var genres = await _context.Genres.ToListAsync();

            return _mapper.Map<List<GenreDTO>>(genres);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDTO>> GetGenreById(Guid id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GenreDTO>(genre));
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateGenre(GenreDTO genreDTO)
        {
            var genre = _mapper.Map<Genre>(genreDTO);
            
            _context.Genres.Add(genre);

            await _context.SaveChangesAsync();

            return Ok();
        }
        
        [HttpPut]
        public async Task<IActionResult> PutGenre(GenreDTO genreDTO)
        {
            var genre = await _context.Genres.FindAsync(genreDTO.Id);

            if (genre == null)
            {
                return NotFound();
            }

            _mapper.From(genreDTO).AdaptTo(genre);

            await _context.SaveChangesAsync();

            return Ok();
        }
        
        [HttpGet("book/{genreId}")]
        public async Task<List<BookDTO>> GetGenresByBook(Guid genreId)
        {
            var books = await _context.Books
                .Include(x => x.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Include(tg=>tg.BookTags)
                .ThenInclude(t=>t.Tag)
                .Include(x=>x.Chapters)
                .Where(x => x.BookGenres != null && x.BookGenres.Any(bg => bg.GenreId == genreId))
                .ToListAsync();
            
            foreach(var chapter in books.SelectMany(x=>x.Chapters))
            {
                chapter.AuthorNoteBottom = null;
                chapter.AuthorNoteTop = null;
                chapter.Content = "";
            }
            
            return _mapper.Map<List<BookDTO>>(books);
        }
    }
}

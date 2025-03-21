using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ThePenfolio.Server.Models;
using ThePenfolio.Server.Services;
using ThePenfolio.Shared.DTOs;
namespace ThePenfolio.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ChapterController : ControllerBase
    {
        private readonly ThePenfolioDbContext _context;
        private readonly IMapper _mapper;

        public ChapterController(ThePenfolioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateChapter(ChapterDTO chapterDTO)
        {
            var chapter = _mapper.Map<Chapter>(chapterDTO);
            _context.Chapters.Add(chapter);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutChapter(ChapterDTO chapterDTO)
        {
            var chapter = await _context.Chapters.FindAsync(chapterDTO.Id);

            _mapper.From(chapter).AdaptTo(chapterDTO);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapter(Guid id)
        {
            var chapter = await _context.Chapters.FindAsync(id);

            if (chapter == null)
            {
                return NotFound();
            }

            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChapterDTO>> GetChapterById(Guid id)
        {
            var chapter = await _context.Chapters
                .Include(x => x.Book)
                    .ThenInclude(x => x.Author)
                .Include(x => x.Book)
                    .ThenInclude(x => x.Chapters)
                        .ThenInclude(x => x.ChapterUserLikes)
                .Include(x=>x.ChapterUserLikes)
                .Include(x=>x.Book)
                .FirstOrDefaultAsync(x=>x.Id == id);

            if (chapter == null)
            {
                return NotFound();
            }

            chapter.Book.Chapters = chapter.Book.Chapters
                    .OrderBy(x=>x.ReleasedOn)
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
                    })
                    .ToList();

            
            var chapterDTO = _mapper.Map<ChapterDTO>(chapter);
            
            chapterDTO.Book = chapter.Book.Adapt<BookDTO>();
            
            return Ok(chapterDTO);
        }
    }
}

using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThePenfolio.Server.Models;
using ThePenfolio.Server.Services;
using ThePenfolio.Shared.DTOs;
namespace ThePenfolio.Server.Controllers
{
    [ApiController]
    [Route("/api/{Controller}")]
    public class TagsController : ControllerBase
    {
        private readonly ThePenfolioDbContext _context;
        private readonly IMapper _mapper;

        public TagsController(ThePenfolioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<List<TagDTO>> GetTags()
        {
            var tags = await _context.Tags.ToListAsync();

            return _mapper.Map<List<TagDTO>>(tags);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<TagDTO>> GetTagById(Guid id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TagDTO>(tag));
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateTag(TagDTO tagDTO)
        {
            var tag = _mapper.Map<Tag>(tagDTO);
            
            _context.Tags.Add(tag);

            await _context.SaveChangesAsync();

            return Ok();
        }
        
        [HttpPut]
        public async Task<IActionResult> PutTag(TagDTO tagDTO)
        {
            var tag = await _context.Tags.FindAsync(tagDTO.Id);

            if (tag == null)
            {
                return NotFound();
            }

            _mapper.From(tagDTO).AdaptTo(tag);

            await _context.SaveChangesAsync();

            return Ok();
        }
        
        [HttpGet("book/{tagId}")]
        public async Task<List<BookDTO>> GetTagsByBook(Guid tagId)
        {
            var books = await _context.Books
                .Include(x => x.Author)
                .Include(b => b.BookTags)
                .ThenInclude(bg => bg.Tag)
                .Include(tg=>tg.BookGenres)
                .ThenInclude(t=>t.Genre)
                .Include(x=>x.Chapters)
                .Where(x => x.BookTags != null && x.BookTags.Any(bg => bg.TagId == tagId))
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

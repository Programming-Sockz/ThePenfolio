using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using ThePenfolio.Server.Models;
using ThePenfolio.Server.Services;
using ThePenfolio.Shared.DTOs;
namespace ThePenfolio.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ChapterUserLikesController(IMapper mapper, ThePenfolioDbContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ToggleChapterUserLike(ChapterUserLikesDTO chapterUserLikesDTO)
        {
            var chapterUserLike = context.ChapterUserLikes.FirstOrDefault(x => x.ChapterId == chapterUserLikesDTO.ChapterId);

            if (chapterUserLike == null)
            {
                chapterUserLike = mapper.Map<ChapterUserLikes>(chapterUserLikesDTO);
                context.ChapterUserLikes.Add(chapterUserLike);
            }
            else
            {
                context.ChapterUserLikes.Remove(chapterUserLike);
            }
            
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}

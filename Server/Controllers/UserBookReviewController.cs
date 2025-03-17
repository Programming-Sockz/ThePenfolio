using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using ThePenfolio.Server.Interfaces;
using ThePenfolio.Server.Models;
using ThePenfolio.Shared.DTOs;
namespace ThePenfolio.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserBookReviewController(IMapper mapper, IThePenfolioDbContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateReview(UserBookReviewsDTO userBookReviewDTO)
        {
            if(context.UserBookReviews.Any(x => x.BookId == userBookReviewDTO.BookId && x.UserId == userBookReviewDTO.UserId))
            {
                return BadRequest();
            }
            
            var userBookReview = mapper.Map<UserBookReviews>(userBookReviewDTO);
            
            context.UserBookReviews.Add(userBookReview);

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}

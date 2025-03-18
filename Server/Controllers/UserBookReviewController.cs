using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using ThePenfolio.Server.Interfaces;
using ThePenfolio.Server.Models;
using ThePenfolio.Server.Services;
using ThePenfolio.Shared.DTOs;
namespace ThePenfolio.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserBookReviewController(IMapper mapper, ThePenfolioDbContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateReview(UserBookReviewsDTO userBookReviewDTO)
        {
            if(context.UserBookReviews.Any(x => x.BookId == userBookReviewDTO.BookId && x.UserId == userBookReviewDTO.UserId))
            {
                return BadRequest("User has already reviewed this book");
            }
            
            var userBookReview = mapper.Map<UserBookReviews>(userBookReviewDTO);
            
            context.UserBookReviews.Add(userBookReview);

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReview(UserBookReviewsDTO userBookReviewDTO)
        {
            var userBookReview = context.UserBookReviews.FirstOrDefault(x => x.BookId == userBookReviewDTO.BookId && x.UserId == userBookReviewDTO.UserId);

            if (userBookReview == null)
            {
                return NotFound();
            }

            context.UserBookReviews.Remove(userBookReview);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}

using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTO;
using MovieAPI.Errors;
using MovieAPI.Model;
using MovieAPI.Repository.Contract;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IMovieRepository _movieRepo;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepo, IMovieRepository movieRepo, IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _movieRepo = movieRepo;
            _mapper = mapper;
        }
        [SwaggerOperation(
            Summary = "Get All Movie's Reviews"
        )]
        [HttpGet("{MovieId:int}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetMovieReviews(int MovieId)
        {
            var movieFound = await _movieRepo.MovieFoundById(MovieId);
            if (!movieFound) return NotFound(new ApiResponse(404, "Movie cannot be found"));
            var reviews = await _reviewRepo.GetReviews(MovieId);
            return Ok(new ApiResponse(200, "Reviews of Movie Retrived Successfully!", _mapper.Map<IReadOnlyList<ReviewToReturnDTO>>(reviews)));
        }
        [SwaggerOperation(
            Summary = "Add New Review to a movie"
        )]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddMovieReview(ReviewToAddDTO newReview)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var movieFound = await _movieRepo.MovieFoundById(newReview.MovieId);
            if (!movieFound) return NotFound(new ApiResponse(404, "Movie cannot be found"));
            var MovieReviewed = await _reviewRepo.MovieReviewed(newReview.MovieId, UserId);
            if (MovieReviewed) return BadRequest(new ApiResponse(400, "Movie reviewed before!"));
            var review = _mapper.Map<Review>(newReview);
            review.UserId = UserId;

            await _reviewRepo.AddReview(review);
            return Ok(new ApiResponse(200, "Review Added to Movie Successfully!"));
        }
        [SwaggerOperation(
            Summary = "Update your Review of a movie"
        )]
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateMovieReview(int Id, double rate, string comment)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var review = await _reviewRepo.GetReviewById(Id);
            if (review == null) return NotFound(new ApiResponse(404, "Review cannot be found"));
            if (review.UserId != UserId) return BadRequest(new ApiResponse(400, "You are not the owner of that review!"));
            review.Rate = rate;
            review.Comment = comment;
            await _reviewRepo.UpdateReview(Id, review);
            return Ok(new ApiResponse(200, "Review Updated Successfully!"));
        }
        [SwaggerOperation(
            Summary = "Delete your Review of a movie"
        )]
        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteMovieReview(int Id)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var review = await _reviewRepo.GetReviewById(Id);
            if (review == null) return NotFound(new ApiResponse(404, "Review cannot be found"));
            if (review.UserId != UserId) return BadRequest(new ApiResponse(400, "You are not the owner of that review!"));

            await _reviewRepo.DeleteReview(Id);
            return Ok(new ApiResponse(200, "Review Deleted from Movie Successfully!"));
        }
    }
}

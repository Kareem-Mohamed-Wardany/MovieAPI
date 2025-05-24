using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTO;
using MovieAPI.Errors;
using MovieAPI.Repository.Contract;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteRepository _favoriteRepo;
        private readonly IMovieRepository _movieRepo;
        private readonly IMapper _mapper;

        public FavoriteController(IFavoriteRepository favoriteRepo, IMovieRepository movieRepo, IMapper mapper)
        {
            _favoriteRepo = favoriteRepo;
            _movieRepo = movieRepo;
            _mapper = mapper;
        }
        [SwaggerOperation(
            Summary = "Get a user's favorite movies",
            Description = "Returns a list of movies favorited by the specified user, including director details."
        )]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<IActionResult> GetAllFavorites()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fav = await _favoriteRepo.GetFavoritesAsync(UserId);

            return Ok(new ApiResponse(200, "User Favorite Movies Retrived Successfully!", _mapper.Map<IReadOnlyList<FavoriteToReturnDTO>>(fav)));
        }
        [SwaggerOperation(
            Summary = "Add a movie to current logged in user's favorite movies List"
        )]
        [HttpPost("{MovieId:int}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> AddFavorite(int MovieId)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool found = await _movieRepo.MovieFoundById(MovieId);
            if (!found) return NotFound(new ApiResponse(404, "Movie cannot be found"));
            var MovieInFav = await _favoriteRepo.MovieInFavorite(MovieId, UserId);
            if (MovieInFav) return BadRequest(new ApiResponse(400, "Movie in favorite list!"));
            await _favoriteRepo.AddToFavoriteAsync(MovieId, UserId);
            return Ok(new ApiResponse(200, "Movie Added to User Favorite List Successfully!"));
        }
        [SwaggerOperation(
            Summary = "Remove a movie from current logged in user's favorite movies List"
        )]
        [HttpDelete("{Id:int}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> DeleteFavorite(int Id)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fav = await _favoriteRepo.GetFavoriteById(Id);
            if (fav == null) return NotFound(new ApiResponse(404, "Movie is not in User Favorite List"));
            if (fav.UserId != UserId) return BadRequest(new ApiResponse(400, "Movie is not in your Favorite List"));
            await _favoriteRepo.DeleteFromFavoriteAsync(Id);
            return Ok(new ApiResponse(200, "Movie Removed from User Favorite List Successfully!"));

        }
    }
}

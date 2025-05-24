using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTO;
using MovieAPI.Errors;
using MovieAPI.Repository.Contract;
using Swashbuckle.AspNetCore.Annotations;
using Talabat.APIs.Helpers;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IActorRepository _actorRepo;
        private readonly IMapper _mapper;

        public MoviesController(IMovieRepository movieRepo, IActorRepository actorRepo, IMapper mapper)
        {
            _movieRepo = movieRepo;
            _actorRepo = actorRepo;
            _mapper = mapper;
        }
        [Cached(600)]
        [SwaggerOperation(
            Summary = "Get All Movies in the system with pagination"
        )]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> GetMovies(int Page = 1, int Limit = 3)
        {
            if (Page <= 0 || Limit <= 0)
            {
                return BadRequest(new ApiResponse(400, "Page and Limit must be greater than 0."));
            }

            var totalMovies = await _movieRepo.MoviesCountAsync();
            var totalPages = (int)Math.Ceiling(totalMovies / (double)Limit);

            if (Page > totalPages)
            {
                return BadRequest(new ApiResponse(400, "Page number exceeds the total number of pages."));
            }
            var movies = _mapper.Map<IReadOnlyList<MovieToReturnDTO>>(await _movieRepo.GetAllMoviesAsync(Page, Limit));
            return Ok(new ApiResponse(200, "Movies Retrived Successfully!", new { Page = Page, Limit = Limit, movies = movies, totalPages = totalPages }));
        }
        [SwaggerOperation(
            Summary = "Get Movie Details with all actors assigned in that movie"
        )]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetMovie(int id)
        {
            bool found = await _movieRepo.MovieFoundById(id);
            if (!found) return NotFound(new ApiResponse(404, "Movie cannot be found"));
            var movie = _mapper.Map<MovieToReturnDTO>(await _movieRepo.GetMovieByIdAsync(id));
            var actors = _mapper.Map<IReadOnlyList<ActorToReturnDTO>>(await _actorRepo.GetMovieActors(id));
            return Ok(new ApiResponse(200, "Movie Retrived Successfully!", new { movie = movie, actors = actors }));
        }
    }
}

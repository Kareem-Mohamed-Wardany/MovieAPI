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
    public class ActorsController : ControllerBase
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IActorRepository _actorRepo;
        private readonly IMapper _mapper;

        public ActorsController(IMovieRepository movieRepo, IActorRepository actorRepo, IMapper mapper)
        {
            _movieRepo = movieRepo;
            _actorRepo = actorRepo;
            _mapper = mapper;
        }
        [Cached(600)]
        [SwaggerOperation(
            Summary = "Get All Actors in the system",
            Description = "You will get Data of Actors with Pagination"
        )]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> GetActors(int Page = 1, int Limit = 3)
        {
            if (Page <= 0 || Limit <= 0)
            {
                return BadRequest(new ApiResponse(400, "Page and Limit must be greater than 0."));
            }

            var totalActors = await _actorRepo.ActorsCountAsync();
            var totalPages = (int)Math.Ceiling(totalActors / (double)Limit);

            if (Page > totalPages)
            {
                return BadRequest(new ApiResponse(400, "Page number exceeds the total number of pages."));
            }
            var actors = _mapper.Map<IReadOnlyList<ActorToReturnDTO>>(await _actorRepo.GetActorsAsync(Page, Limit));
            return Ok(new ApiResponse(200, "Actors Retrived Successfully!", new { Page = Page, Limit = Limit, actors = actors, totalPages = totalPages }));
        }
        [SwaggerOperation(
            Summary = "Get Data for an Actor",
            Description = "Get Actor Data with all his movies"
        )]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetActor(int id)
        {
            bool found = _actorRepo.ActorFoundById(id);
            if (!found) return NotFound(new ApiResponse(404, "Actor cannot be found"));
            var actor = _mapper.Map<ActorToReturnDTO>(_actorRepo.GetActorById(id));
            var actormovies = _mapper.Map<IReadOnlyList<MovieToReturnDTO>>(await _movieRepo.GetActorMoviesAsync(id));
            return Ok(new ApiResponse(200, "Actor with his Movies Retrived Successfully!", new { actor = actor, movies = actormovies }));
        }
    }
}

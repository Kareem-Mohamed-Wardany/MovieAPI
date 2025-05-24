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
    public class DirectorsController : ControllerBase
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IDirectorRepository _directorRepo;
        private readonly IMapper _mapper;

        public DirectorsController(IMovieRepository movieRepo, IDirectorRepository directorRepo, IMapper mapper)
        {
            _movieRepo = movieRepo;
            _directorRepo = directorRepo;
            _mapper = mapper;
        }
        [Cached(600)]
        [SwaggerOperation(
            Summary = "Get All Directors in the system",
            Description = "You will get Data of Directors with Pagination"
        )]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> GetDirectors(int Page = 1, int Limit = 3)
        {
            if (Page <= 0 || Limit <= 0)
            {
                return BadRequest(new ApiResponse(400, "Page and Limit must be greater than 0."));
            }

            var totalDirectors = await _directorRepo.DirectorsCountAsync();
            var totalPages = (int)Math.Ceiling(totalDirectors / (double)Limit);

            if (Page > totalPages)
            {
                return BadRequest(new ApiResponse(400, "Page number exceeds the total number of pages."));
            }
            var directors = _mapper.Map<IReadOnlyList<DirectorToReturnDTO>>(await _directorRepo.GetDirectorsAsync(Page, Limit));
            return Ok(new ApiResponse(200, "Directors Retrived Successfully!", new { Page = Page, Limit = Limit, directors = directors, totalPages = totalPages }));
        }
        [SwaggerOperation(
            Summary = "Get Data for a Director",
            Description = "Get Director Data with all his movies"
        )]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetDirector(int id)
        {
            bool found = _directorRepo.DirectorFoundById(id);
            if (!found) return NotFound(new ApiResponse(404, "Director cannot be found"));
            var director = _mapper.Map<DirectorToReturnDTO>(_directorRepo.GetDirectorById(id));
            var directormovies = _mapper.Map<IReadOnlyList<DirectorMovieToReturnDTO>>(await _movieRepo.GetDirectorMoviesAsync(id));
            return Ok(new ApiResponse(200, "Director with his Movies Retrived Successfully!", new { director = director, movies = directormovies }));
        }
    }
}

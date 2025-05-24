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
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IActorRepository _actorRepo;
        private readonly IDirectorRepository _directorRepo;
        private readonly IMovieRepository _movieRepo;
        private readonly IMapper _mapper;

        public AdminController(IActorRepository actorRepo, IDirectorRepository directorRepo, IMovieRepository movieRepo, IMapper mapper)
        {
            _actorRepo = actorRepo;
            _directorRepo = directorRepo;
            _movieRepo = movieRepo;
            _mapper = mapper;
        }
        [SwaggerOperation(
            Summary = "Add Actor to the system"
        )]
        [HttpPost("actor")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddActor([FromForm] AddUpdateDTO newActor, IFormFile imageFile)
        {
            string imageURL = await _actorRepo.UploadImageAsync(imageFile);
            if (imageURL == null) return NotFound(new ApiResponse(404, "Image cannot be found"));
            var actor = _mapper.Map<Actor>(newActor);
            actor.Image = imageURL;

            await _actorRepo.AddActorAsync(actor);
            return Ok(new ApiResponse(201, "Actor Created Successfully!", _mapper.Map<ActorToReturnDTO>(actor)));
        }
        [SwaggerOperation(
            Summary = "Delete Actor from the system"
        )]
        [HttpDelete("actor/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> deleteActor(int id)
        {

            bool found = _actorRepo.ActorFoundById(id);
            if (!found) return NotFound(new ApiResponse(404, "Actor cannot be found"));
            await _actorRepo.DeleteActorAsync(id);
            return Ok(new ApiResponse(201, "Actor deleted Successfully!"));
        }
        [SwaggerOperation(
            Summary = "Update Actor in the system"
        )]
        [HttpPut("actor/{Id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateActor([FromRoute] int Id, [FromForm] AddUpdateDTO newActor, IFormFile imageFile)
        {
            var Actor = _actorRepo.GetActorById(Id);
            if (Actor == null) return NotFound(new ApiResponse(404, "Actor cannot be found"));
            string imageURL = await _actorRepo.UploadImageAsync(imageFile);
            if (imageURL == null) return NotFound(new ApiResponse(404, "Image cannot be found"));
            var newAct = _mapper.Map<Actor>(newActor);
            newAct.Image = imageURL;
            await _actorRepo.UpdateActorByIdAsync(Id, newAct);
            return Ok(new ApiResponse(200, "Actor Updated Successfully!", _mapper.Map<ActorToReturnDTO>(newAct)));
        }


        [SwaggerOperation(
            Summary = "Add Director to the system"
        )]
        [HttpPost("director")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddDirector([FromForm] AddUpdateDTO newDirector, IFormFile imageFile)
        {
            string imageURL = await _directorRepo.UploadImageAsync(imageFile);
            if (imageURL == null) return NotFound(new ApiResponse(404, "Image cannot be found"));

            var director = _mapper.Map<Director>(newDirector);
            director.Image = imageURL;

            await _directorRepo.AddDirectorAsync(director);
            return Ok(new ApiResponse(201, "Director Created Successfully!", _mapper.Map<DirectorToReturnDTO>(director)));
        }
        [SwaggerOperation(
           Summary = "Delete Director from the system"
       )]
        [HttpDelete("director/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> deleteDirector(int id)
        {

            bool found = _directorRepo.DirectorFoundById(id);
            if (!found) return NotFound(new ApiResponse(404, "Director cannot be found"));
            await _directorRepo.DeleteDirectorAsync(id);
            return Ok(new ApiResponse(201, "Director deleted Successfully!"));
        }
        [SwaggerOperation(
           Summary = "Update Director in the system"
       )]
        [HttpPut("director/{Id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateDirector([FromRoute] int Id, [FromForm] AddUpdateDTO newDirector, IFormFile imageFile)
        {
            var Director = _directorRepo.GetDirectorById(Id);
            if (Director == null) return NotFound(new ApiResponse(404, "Director cannot be found"));
            string imageURL = await _directorRepo.UploadImageAsync(imageFile);
            if (imageURL == null) return NotFound(new ApiResponse(404, "Image cannot be found"));
            var newDir = _mapper.Map<Director>(newDirector);
            newDir.Image = imageURL;
            await _directorRepo.UpdateDirectorByIdAsync(Id, newDir);
            return Ok(new ApiResponse(200, "Director Updated Successfully!", _mapper.Map<DirectorToReturnDTO>(newDir)));
        }

        [SwaggerOperation(
           Summary = "Add Movie to the system"
        )]
        [HttpPost("movie")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddMovie([FromForm] MovieToAddDTO newMovie, IFormFile imageFile)
        {
            string imageURL = await _movieRepo.UploadImageAsync(imageFile);
            if (imageURL == null) return NotFound(new ApiResponse(404, "Image cannot be found"));
            var found = _directorRepo.DirectorFoundById(newMovie.DirectorId);
            if (!found) return NotFound(new ApiResponse(404, "Director cannot be found"));
            var movie = _mapper.Map<Movie>(newMovie);
            movie.Image = imageURL;

            await _movieRepo.AddMovie(movie);
            return Ok(new ApiResponse(201, "Movie Created Successfully!", _mapper.Map<MovieToReturnDTO>(movie)));
        }
        [SwaggerOperation(
           Summary = "Delete Movie from the system"
        )]
        [HttpDelete("movie/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> deleteMovie(int id)
        {

            bool found = await _movieRepo.MovieFoundById(id);
            if (!found) return NotFound(new ApiResponse(404, "Movie cannot be found"));
            await _movieRepo.DeleteMovieByIdAsync(id);
            return Ok(new ApiResponse(201, "Movie deleted Successfully!"));
        }
        [SwaggerOperation(
           Summary = "Update Movie in the system"
        )]
        [HttpPut("movie/{Id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateMovie([FromRoute] int Id, [FromForm] MovieToAddDTO newMovie, IFormFile imageFile)
        {
            var movie = await _movieRepo.GetMovieByIdAsync(Id);
            if (movie == null) return NotFound(new ApiResponse(404, "Movie cannot be found"));
            string imageURL = await _movieRepo.UploadImageAsync(imageFile);
            if (imageURL == null) return NotFound(new ApiResponse(404, "Image cannot be found"));
            var newMov = _mapper.Map<Movie>(newMovie);
            newMov.Image = imageURL;
            await _movieRepo.UpdateMovieByIdAsync(Id, newMov);
            var updatedMovie = await _movieRepo.GetMovieByIdAsync(Id);
            return Ok(new ApiResponse(200, "Movie Updated Successfully!", _mapper.Map<MovieToReturnDTO>(updatedMovie)));
        }
        [SwaggerOperation(
           Summary = "Add Actor to a Movie in the system"
        )]
        [HttpPost("movieactor")]
        [ProducesResponseType(typeof(ApiResponse), 201)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddMovieActor(int MovieId, int ActorId)
        {
            if (!_actorRepo.ActorFoundById(ActorId)) return NotFound(new ApiResponse(404, "Actor cannot be found"));
            var mFound = await _movieRepo.MovieFoundById(MovieId);
            if (!mFound) return NotFound(new ApiResponse(404, "Movie cannot be found"));
            await _movieRepo.AddActorToMovieAsync(MovieId, ActorId);
            return Ok(new ApiResponse(201, "Actor Added Successfully to that Movie!"));
        }

    }
}

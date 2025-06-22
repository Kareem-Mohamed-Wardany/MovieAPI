using System.IO;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DBContext;
using MovieAPI.Model;
using MovieAPI.Repository.Contract;

namespace MovieAPI.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieRepository(Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task AddMovie(Movie newMovie)
        {
            await _context.Movies.AddAsync(newMovie);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteMovieByIdAsync(int id)
        {
            var movie = await GetMovieByIdAsync(id);
            if (!string.IsNullOrEmpty(movie.Image))
            {
                DeleteImageAsync(movie.Image);
            }
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
        public void DeleteImageAsync(string Image)
        {
            var imageRelativePath = Image.TrimStart('/'); // remove the leading slash
            var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, imageRelativePath);

            // Check if the old file exists, then delete it
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
        }

        public async Task<IReadOnlyList<Movie>> GetActorMoviesAsync(int ActorId)
        => await _context.ActorsMovies.Where(x => x.ActorId == ActorId).Include(x => x.Movie).ThenInclude(m => m.Director).Select(x => x.Movie!).AsNoTracking().ToListAsync();

        public async Task<IReadOnlyList<Movie>> GetAllMoviesAsync(int Page, int ItemsPerPage)
        => await _context.Movies.Include(m => m.Director).Skip((Page - 1) * ItemsPerPage).Take(ItemsPerPage).AsNoTracking().ToListAsync();
        public async Task<IReadOnlyList<Movie>> GetAllMoviesAsync()
        => await _context.Movies.Include(m => m.Director).AsNoTracking().ToListAsync();

        public async Task<IReadOnlyList<Movie>> GetDirectorMoviesAsync(int DirectorId)
        => await _context.Movies.Where(x => x.DirectorId == DirectorId).AsNoTracking().ToListAsync();

        public async Task<Movie> GetMovieByIdAsync(int id)
        => await _context.Movies.Include(m => m.Director).FirstOrDefaultAsync(x => x.Id == id);

        public async Task UpdateMovieByIdAsync(int id, Movie newMovie)
        {
            var movie = await GetMovieByIdAsync(id);
            if (!string.IsNullOrEmpty(movie.Image) && !string.IsNullOrEmpty(newMovie.Image))
            {
                DeleteImageAsync(movie.Image);
                movie.Image = newMovie.Image;
            }
            movie.Title = newMovie.Title;
            movie.Description = newMovie.Description;
            movie.Genre = newMovie.Genre;
            movie.Duration = newMovie.Duration;
            movie.Rate = newMovie.Rate;
            movie.DirectorId = newMovie.DirectorId;
            await _context.SaveChangesAsync();
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return "";


            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            string file = "/images/" + fileName;
            return file;
        }

        public async Task<bool> MovieFoundById(int id)
        => await GetMovieByIdAsync(id) != null;

        public async Task<int> MoviesCountAsync()
        => await _context.Movies.CountAsync();

        public async Task AddActorToMovieAsync(int MovieId, int ActorId)
        {
            ActorsMovies actorMovie = new ActorsMovies();
            actorMovie.ActorId = ActorId;
            actorMovie.MovieId = MovieId;
            await _context.ActorsMovies.AddAsync(actorMovie);
            await _context.SaveChangesAsync();
        }
    }
}

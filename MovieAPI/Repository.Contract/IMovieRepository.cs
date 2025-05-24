using MovieAPI.Model;

namespace MovieAPI.Repository.Contract
{
    public interface IMovieRepository
    {
        Task<IReadOnlyList<Movie>> GetAllMoviesAsync(int Page, int ItemsPerPage);
        Task<IReadOnlyList<Movie>> GetActorMoviesAsync(int ActorId);
        Task<IReadOnlyList<Movie>> GetDirectorMoviesAsync(int DirectorId);
        Task<Movie> GetMovieByIdAsync(int id);
        Task AddMovie(Movie newMovie);
        Task DeleteMovieByIdAsync(int id);
        Task UpdateMovieByIdAsync(int id, Movie newMovie);
        Task<string> UploadImageAsync(IFormFile imageFile);
        void DeleteImageAsync(string Image);

        Task<bool> MovieFoundById(int id);

        Task<int> MoviesCountAsync();
        Task AddActorToMovieAsync(int MovieId, int ActorId);

    }
}

using MovieAPI.Model;

namespace MovieAPI.Repository.Contract
{
    public interface IDirectorRepository
    {
        Task<IReadOnlyList<Director>> GetDirectorsAsync(int Page, int ItemsPerPage);
        Director GetDirectorById(int id);
        Task AddDirectorAsync(Director NewDirector);
        Task DeleteDirectorAsync(int id);
        Task<string> UploadImageAsync(IFormFile imageFile);
        void DeleteImageAsync(string Image);
        bool DirectorFoundById(int Id);
        Task UpdateDirectorByIdAsync(int Id, Director NewDirector);
        Task<int> DirectorsCountAsync();
    }
}

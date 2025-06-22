using MovieAPI.Model;

namespace MovieAPI.Repository.Contract
{
    public interface IActorRepository
    {
        Task<IReadOnlyList<Actor>> GetActorsAsync(int Page, int ItemsPerPage);
        Task<IReadOnlyList<Actor>> GetActorsAsync();

        Actor GetActorById(int id);
        Task AddActorAsync(Actor NewActor);
        Task DeleteActorAsync(int id);
        Task<string> UploadImageAsync(IFormFile imageFile);
        void DeleteImageAsync(string Image);
        bool ActorFoundById(int Id);
        Task UpdateActorByIdAsync(int Id, Actor NewActor);
        Task<int> ActorsCountAsync();
        Task<IReadOnlyList<Actor>> GetMovieActors(int MovieId);
    }
}

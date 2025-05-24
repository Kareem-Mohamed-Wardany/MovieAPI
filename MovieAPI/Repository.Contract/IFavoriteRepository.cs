using MovieAPI.Model;

namespace MovieAPI.Repository.Contract
{
    public interface IFavoriteRepository
    {
        Task AddToFavoriteAsync(int MovieId, string UserId);
        Task DeleteFromFavoriteAsync(int Id);
        Task<Favorite> GetFavoriteById(int Id);
        Task<IReadOnlyList<Favorite>> GetFavoritesAsync(string UserId);
        Task<bool> FavoriteFoundByIdAsync(int Id);
        Task<bool> MovieInFavorite(int MovieId, string UserId);
    }
}

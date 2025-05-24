using Microsoft.EntityFrameworkCore;
using MovieAPI.DBContext;
using MovieAPI.Model;
using MovieAPI.Repository.Contract;

namespace MovieAPI.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly Context _context;

        public FavoriteRepository(Context context)
        {
            _context = context;
        }
        public async Task AddToFavoriteAsync(int MovieId, string UserId)
        {
            Favorite favorite = new Favorite
            {
                UserId = UserId,
                MovieId = MovieId
            };
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFromFavoriteAsync(int Id)
        {
            var item = await _context.Favorites.FirstOrDefaultAsync(x => x.Id == Id);
            _context.Favorites.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> FavoriteFoundByIdAsync(int Id)
        => await _context.Favorites.FirstOrDefaultAsync(x=> x.Id == Id) != null;

        public async Task<Favorite> GetFavoriteById(int Id)
        => await _context.Favorites.FirstOrDefaultAsync(x=>x.Id == Id);

        public async Task<IReadOnlyList<Favorite>> GetFavoritesAsync(string UserId)
        => await _context.Favorites.Where(x => x.UserId == UserId).Include(x => x.Movie).ThenInclude(m => m.Director).AsNoTracking().ToListAsync();

        public async Task<bool> MovieInFavorite(int MovieId, string UserId)
        => await _context.Favorites.FirstOrDefaultAsync(x=> x.MovieId==MovieId && x.UserId == UserId) != null;
    }
}

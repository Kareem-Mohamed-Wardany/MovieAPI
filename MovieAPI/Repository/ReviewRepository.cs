using Microsoft.EntityFrameworkCore;
using MovieAPI.DBContext;
using MovieAPI.Model;
using MovieAPI.Repository.Contract;

namespace MovieAPI.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly Context _context;

        public ReviewRepository(Context context)
        {
            _context = context;
        }
        public async Task<bool> MovieReviewed(int MovieId, string UserId)
        => await _context.Reviews.FirstOrDefaultAsync(x => x.MovieId == MovieId && x.UserId == UserId) != null;

        public async Task AddReview(Review newReview)
        {
            _context.Reviews.Add(newReview);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReview(int Id, Review updatedReview)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == Id);
            review.Rate = updatedReview.Rate;
            review.Comment = updatedReview.Comment;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReview(int Id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == Id);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Review>> GetReviews(int MovieId)
        => await _context.Reviews.Where(x => x.MovieId == MovieId).Include(x => x.User).AsNoTracking().ToListAsync();

        public async Task<bool> ReviewFoundById(int Id)
        => await _context.Reviews.FirstOrDefaultAsync(x=>x.Id== Id) != null;

        public async Task<Review> GetReviewById(int Id)
        => await _context.Reviews.FirstOrDefaultAsync(x => x.Id == Id);

    }
}

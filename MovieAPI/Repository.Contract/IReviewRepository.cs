using MovieAPI.Model;

namespace MovieAPI.Repository.Contract
{
    public interface IReviewRepository
    {
        Task AddReview(Review newReview);
        Task UpdateReview(int Id,Review updatedReview);
        Task DeleteReview(int Id);
        Task<bool> MovieReviewed(int MovieId, string UserId);
        Task<IReadOnlyList<Review>> GetReviews(int MovieId);
        Task<bool> ReviewFoundById(int Id);
        Task<Review> GetReviewById(int Id);
    }
}

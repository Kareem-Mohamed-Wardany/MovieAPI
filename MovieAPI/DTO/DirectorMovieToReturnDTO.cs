using MovieAPI.Model;

namespace MovieAPI.DTO
{
    public class DirectorMovieToReturnDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public MovieGenre Genre { get; set; }
        public string Duration { get; set; }
        public string? Image { get; set; }
        public double Rate { get; set; }
    }
}

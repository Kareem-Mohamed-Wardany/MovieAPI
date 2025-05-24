using MovieAPI.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.DTO
{
    public class MovieToReturnDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public MovieGenre Genre { get; set; }
        public string Duration { get; set; }
        public string? Image { get; set; }
        public double Rate { get; set; }
        public string DirectorName { get; set; }
    }
}

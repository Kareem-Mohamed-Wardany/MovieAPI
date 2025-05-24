using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Model
{
    public class Movie
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please provide Title")]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters")]
        [MaxLength(50, ErrorMessage = "Title must be at most 50 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please provide Description")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
        [MaxLength(100, ErrorMessage = "Description must be at most 100 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please provide Genre")]

        public MovieGenre Genre { get; set; }
        [Required(ErrorMessage = "Please provide Duration")]
        public string Duration { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Please provide Rate")]
        [Range(0.0,10.0)]
        public double Rate { get; set; }
        [ForeignKey("Director")]
        public int DirectorId { get; set; }

        public Director Director { get; set; }


    }
}

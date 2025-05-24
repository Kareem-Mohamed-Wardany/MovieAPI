using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Model
{
    public class Director
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please provide Name")]
        [MinLength(5, ErrorMessage = "Name must be at least 5 characters")]
        [MaxLength(50, ErrorMessage = "Name must be at most 50 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please provide Birth Date")]
        public  DateOnly BirthDate { get; set; }
        public string? Image { get; set; }
    }
}

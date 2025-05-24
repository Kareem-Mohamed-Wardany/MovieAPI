using System.ComponentModel.DataAnnotations;

namespace MovieAPI.DTO
{
    public class AddUpdateDTO
    {
        [Required(ErrorMessage = "Please provide Name")]
        [MinLength(5, ErrorMessage = "Name must be at least 5 characters")]
        [MaxLength(50, ErrorMessage = "Name must be at most 50 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please provide Birth Date")]
        public DateOnly BirthDate { get; set; }
    }
}

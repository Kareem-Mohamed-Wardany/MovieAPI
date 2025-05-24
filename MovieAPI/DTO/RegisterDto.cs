using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}

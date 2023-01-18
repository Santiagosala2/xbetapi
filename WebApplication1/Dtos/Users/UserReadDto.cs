using System.ComponentModel.DataAnnotations;

namespace Users.Dtos
{
    public class UserReadDto
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public string FirstName { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Users.Dtos
{
    public class CreateBetDto
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;


        [Required]
        public string LastName { get; set; } = null!;


        [Required]
        public string DateOfBirth { get; set; } = null!;
    }
}
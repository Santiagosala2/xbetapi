using System.ComponentModel.DataAnnotations;

namespace Users.Dtos
{
    public class UserProfileDto
    {
        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public decimal Balance { get; set; }
    }
}

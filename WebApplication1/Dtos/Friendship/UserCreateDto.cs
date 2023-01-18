using System.ComponentModel.DataAnnotations;

namespace Friends.Dtos
{
    public class FriendshipCreateDto
    {
        [Required]
        public string ToEmail { get; set; } = null!;

        [Required]
        public string FromEmail { get; set; } = null!;

     
    }
}
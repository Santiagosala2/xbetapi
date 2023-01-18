using System.ComponentModel.DataAnnotations;

namespace Friends.Dtos
{
    public class FriendshipRequestDto
    {
        [Required]
        public int FriendshipId { get; set; }


     
    }
}
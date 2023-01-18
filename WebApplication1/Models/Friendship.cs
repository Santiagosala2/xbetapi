using System.ComponentModel.DataAnnotations;
using Users.Models;

namespace Friends.Models
{
    public class Friendship
    {
        [Key]
        public int FriendshipId { get; set; }
        [Required]
        public int FromUserID { get; set; }

        [Required]
        public int ToUserID { get; set; }

        [Required]
        public bool Pending { get; set; }

        public virtual User FromUser { get; set; } = null!;
        public virtual User ToUser { get; set; } = null!;
    }
}

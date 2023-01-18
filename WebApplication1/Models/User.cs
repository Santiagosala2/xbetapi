using Wallet.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Friends.Models;

namespace Users.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string? Username { get; set; }
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string DateOfBirth { get; set; } = null!;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }

        public virtual ICollection<Transaction> SystemTransactions { get; set; } = null!;
        public virtual ICollection<Transaction> UserTransactions { get; set; } = null!;

        public virtual ICollection<Friendship> FromUserFriendships { get; set; } = null!;
        public virtual ICollection<Friendship> ToUserFriendships { get; set; } = null!;
    }
}

using CustomAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Users.Models;

namespace Bets.Models
{
    public class Bet
    {
        [Key]
        public int BetID { get; set; }

        [Required]
        [StringOptions(AllowableValues = new[] { "Manual", "Weather" }, ErrorMessage = "Type must be either Manual or Weather")]
        public string Type { get; set; } = null!;

        [Required]
        [StringOptions(AllowableValues = new[] { "Pending", "Completed" }, ErrorMessage = "Status must be either Pending or Completed")]
        public string Status { get; set; } = null!;

        [Column(TypeName = "decimal(18, 2)")]
        [Range(1, Double.PositiveInfinity, ErrorMessage = "The wager amount must be greater than 1")]
        public decimal Wager { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }

        public string? Climate { get; set; } 

        public string? FriendClimate { get; set; }

        public DateTime When { get; set; }

        public DateTime? Completition { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int FriendID { get; set; }
  
        public int? JudgeID { get; set; }
     
        public int? WinnerID { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual User Friend { get; set; } = null!;
        public virtual User? Judge { get; set; } 
        public virtual User? Winner { get; set; } 

    }
}

using CustomAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bets.Dtos
{
    public class CreateManualBetDto
    {
        [Required]
        public string Type { get; set; } = null!;

        [Required]
        public string Status { get; set; } = null!;

        [Required]
        public decimal Wager { get; set; }

        [Required]
        public string Name { get; set; } = null!;

       [Required]
        public DateTime When { get; set; }
 
        [Required]
        public int UserID { get; set; }

        [Required]
        public int FriendID { get; set; }

        [Required]
        public int JudgeID { get; set; }

       
    }
}
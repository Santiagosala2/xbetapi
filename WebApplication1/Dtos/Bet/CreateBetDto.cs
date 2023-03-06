using CustomAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bets.Dtos
{
    public class CreateBetDto
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
        public string When { get; set; } = null!;

        public string Location { get; set; } = null!;
        public string Climate { get; set; } = null!;


        [Required]
        public string UserEmail { get; set; } = null!;
        [Required]
        public string FriendEmail { get; set; } = null!;
        [Required]
        public string JudgeEmail { get; set; } = null!;


    }
}
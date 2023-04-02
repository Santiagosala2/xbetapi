using CustomAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bets.Dtos
{
    public class AnswerClimateBetDto
    {
        [Required]
        public string FriendClimate { get; set; } = null!;


    }
}
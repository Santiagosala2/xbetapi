namespace Bets.Dtos
{
    public class ReadBetDto
    {

        public int BetID { get; set; }
        public string Type { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal Wager { get; set; }
        public string? Name { get; set; } = null!;
        public string? Location { get; set; }
        public string? Climate { get; set; }
        public string? FriendClimate { get; set; }
        public DateTime When { get; set; } 
        public DateTime Completition { get; set; }

        public int UserID { get; set; }

        public int FriendID { get; set; }

        public int? JudgeID { get; set; }

        public int? WinnerID { get; set; }
  
    }
}
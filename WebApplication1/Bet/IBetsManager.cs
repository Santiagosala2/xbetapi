using Search.Users.Dtos;
using Bets.Models;
using Users.Models;

namespace Bets.Manager
{
    public interface IBetsManager
    {
        Task<(bool, int)> CreateBetAsync(Bet bet);
        Task<Bet?> GetBetAsync(int betId, int userId);
    }
}
using Search.Users.Dtos;
using Bets.Models;
using Users.Models;
using Bets.Dtos;

namespace Bets.Manager
{
    public interface IBetsManager
    {
        Task<(bool, int)> CreateBetAsync(CreateBetDto bet);
        Task<Bet?> GetBetAsync(int betId, int userId);

        Task<(List<Bet>, List<Bet>)> GetUserBetsAsync(int userId);
    }
}
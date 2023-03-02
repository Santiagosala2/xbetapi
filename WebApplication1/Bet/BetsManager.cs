using DataStore;
using Microsoft.AspNetCore.Identity;
using AuthServer.Helpers.Hasher;
using Users.Models;
using Bets.Models;

namespace Bets.Manager
{
    public class BetsManager : IBetsManager
    {
        private DataStoreContext _context;

        public BetsManager(DataStoreContext context)
        {
            _context = context;
        }

        public async Task<(bool,int)> CreateBetAsync(Bet bet)
        {
            await _context.Bets.AddAsync(bet);
            int betSaved = await _context.SaveChangesAsync();
            return (betSaved == 1,bet.BetID);

        }

        public async Task<Bet?> GetBetAsync(int betId, int userId)
        {
            var getUserBet = await _context.Bets.FirstOrDefaultAsync(b => b.UserID == userId && b.BetID == betId);
            return getUserBet;
  
        }

    }
}

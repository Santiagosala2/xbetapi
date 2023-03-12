using DataStore;
using Microsoft.AspNetCore.Identity;
using AuthServer.Helpers.Hasher;
using Users.Models;
using Bets.Models;
using Bets.Dtos;

namespace Bets.Manager
{
    public class BetsManager : IBetsManager
    {
        private DataStoreContext _context;

        public BetsManager(DataStoreContext context)
        {
            _context = context;
        }

        public async Task<(bool, int)> CreateBetAsync(CreateBetDto bet)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == bet.UserEmail);
            var friend = await _context.Users.FirstOrDefaultAsync(user => user.Email == bet.FriendEmail);


            if (user != null && friend != null)
            {
                Bet createBet = new Bet
                {
                    Type = bet.Type,
                    Status = bet.Status,
                    Wager = bet.Wager,
                    When = DateTime.Parse(bet.When),
                    UserID = user.UserID,
                    FriendID = friend.UserID

                };

                if (createBet.Type == "weather")
                {
                    createBet.Location = bet.Location;
                    createBet.Climate = bet.Climate;
                }

                if (createBet.Type == "manual")
                {
                    var judge = await _context.Users.FirstOrDefaultAsync(user => user.Email == bet.JudgeEmail);
                    if (judge != null)
                    {
                        createBet.JudgeID = judge.UserID;
                        createBet.Name = bet.Name;
                    } else
                    {
                        return (false, 0);
                    }

                }

                await _context.Bets.AddAsync(createBet);
                int betSaved = await _context.SaveChangesAsync();
                return (betSaved == 1, createBet.BetID);

            }

            return (false, 0);


        }
        public async Task<(List<Bet>,List<Bet>)> GetUserBetsAsync( int userId)
        {
            var getUserBets = await _context.Bets.Where(b => b.UserID == userId && b.Status == "Pending").ToListAsync();
            var awaitingUserBets = await _context.Bets.Where(b => b.FriendID == userId && b.Status == "Pending").ToListAsync();

            return (getUserBets,awaitingUserBets); 

        }

        public async Task<Bet?> GetBetAsync(int betId, int userId)
        {
            var getUserBet = await _context.Bets.FirstOrDefaultAsync(b => b.UserID == userId && b.BetID == betId);
            return getUserBet;
  
        }

    }
}

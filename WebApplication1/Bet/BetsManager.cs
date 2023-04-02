using DataStore;
using Microsoft.AspNetCore.Identity;
using AuthServer.Helpers.Hasher;
using Users.Models;
using Bets.Models;
using Bets.Dtos;
using Friends.Models;

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

            var getUserBets = await (from bet in _context.Set<Bet>()
                                join friend in _context.Set<User>()
                                on bet.FriendID equals friend.UserID into grouping                   
                                from friend in grouping.DefaultIfEmpty()
                                join judge in _context.Set<User>()
                                on bet.JudgeID equals judge.UserID into grouping2
                                from judge in grouping2.DefaultIfEmpty()
                                where (bet.UserID == userId && bet.Status == "Pending") || (bet.Status == "Pending - Ready")
                                select new
                                {
                                    bet,
                                    friend,
                                    judge
                                }).ToListAsync();

            var awaitingUserBets = await (from bet in _context.Set<Bet>()
                                          join friend in _context.Set<User>()
                                          on bet.FriendID equals friend.UserID into grouping
                                          from friend in grouping.DefaultIfEmpty()
                                          join judge in _context.Set<User>()
                                          on bet.JudgeID equals judge.UserID into grouping2
                                          from judge in grouping2.DefaultIfEmpty()
                                          where bet.FriendID == userId && bet.Status == "Pending"
                                          select new
                                          {
                                              bet,
                                              friend
                                          }).ToListAsync();

            var resultGetUserBets = getUserBets.Select(b => b.bet).ToList();

           // var getUserBets = await _context.Bets.Where(b => b.UserID == userId && b.Status == "Pending").ToListAsync();

            var resultAwaitingUserBets = awaitingUserBets.Select(b => b.bet).ToList();

            return (resultGetUserBets, resultAwaitingUserBets); 

        }

        public async Task<Bet?> GetBetAsync(int betId, int userId)
        {
            var getUserBet = await _context.Bets.FirstOrDefaultAsync(b => b.UserID == userId && b.BetID == betId);
            return getUserBet;
  
        }

        public async Task<bool> AcceptBetAsync(int betId , int friendId, string climateAnswer)
        {
            var getUpdateUserBet = await _context.Bets.FirstOrDefaultAsync(b => b.FriendID == friendId && b.BetID == betId);
            bool saveChanges = false;
            if (getUpdateUserBet?.Type == "weather")
            {
                getUpdateUserBet.FriendClimate = climateAnswer;
                getUpdateUserBet.Status = "Pending - Ready";
                saveChanges = true;
            }

            if (saveChanges)
            {
                return await _context.SaveChangesAsync() == 1;
            }

            return false;
        }

        public async Task<bool> RejectBetAsync(int betId)
        {
            var getUpdateUserBet = await _context.Bets.FirstOrDefaultAsync(b => b.BetID == betId);
            bool saveChanges = false;
            if (getUpdateUserBet is not null)
            {
                getUpdateUserBet.Status = "Rejected";
                saveChanges = true;
            }

            if (saveChanges)
            {
                return await _context.SaveChangesAsync() == 1;
            }

            return false;
        }

    }
}

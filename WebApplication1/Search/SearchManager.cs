using DataStore;
using Microsoft.AspNetCore.Identity;
using AuthServer.Helpers.Hasher;
using Users.Models;
using Search.Users.Dtos;
using Friends.Models;

namespace Search.Manager
{
    public class SearchManager : ISearchManager
    {
        private DataStoreContext _context;

        public SearchManager(DataStoreContext context)
        {
            _context = context;
        }

        public async Task<List<SearchReadUsersDto>> GetUsersAsync(string keyword, string userEmail)
        {
            var currentUser = (await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail))!;

            var currentUserRequests = await (from user in _context.Set<User>()
                                             join friendship in _context.Set<Friendship>()
                                             on user.UserID equals friendship.FromUserID into grouping
                                             where user.UserID == currentUser.UserID
                                             from friendship in grouping.DefaultIfEmpty()
                                             select new
                                             {
                                                 user,
                                                 friendship
                                             }).ToListAsync();



            var searchMatches = await (from user in _context.Set<User>()
                                join friendship in _context.Set<Friendship>()
                                on user.UserID equals friendship.FromUserID into grouping
                                where user.Email.Contains(keyword) || user.FirstName.Contains(keyword)
                                from friendship in grouping.DefaultIfEmpty()                               
                                select new {
                                    user,
                                    friendship
                                }).ToListAsync();


            // get friends
            List<SearchReadUsersDto> friendsRequests = new List<SearchReadUsersDto>();
            List<SearchReadUsersDto> friends = new List<SearchReadUsersDto>();
            List<SearchReadUsersDto> requests = new List<SearchReadUsersDto>();
            List<SearchReadUsersDto> nonFriends = new List<SearchReadUsersDto>();
       

            var matchesUsersIds = searchMatches.Select(fq => fq.user.UserID).Distinct().ToList();

            currentUserRequests.ForEach(ur => {
                var foundFriendRequestId = matchesUsersIds.FirstOrDefault(mu => mu == ur.friendship?.ToUserID);
                if (foundFriendRequestId > 0)
                {
                    var u = (searchMatches.FirstOrDefault(sm => sm.user.UserID == foundFriendRequestId))!;
                    friendsRequests.Add(
                        new SearchReadUsersDto
                        {
                            FriendshipId = ur.friendship.FriendshipId,
                            Email = u.user.Email,
                            FirstName = u.user.FirstName,
                            Type = ur.friendship!.Pending == false ? "Friend" : "Request"
                        });
                }
            });

            friends = searchMatches.Where(r => r.user.UserID != currentUser.UserID && (r.friendship?.ToUserID == currentUser.UserID) && !r.friendship.Pending)

                .Select(nF => new SearchReadUsersDto
                {
                    FriendshipId = nF.friendship.FriendshipId,
                    Email = nF.user.Email,
                    FirstName = nF.user.FirstName,
                    Type = "Friend"
                }).ToList();

            requests = searchMatches.Where(r => r.user.UserID != currentUser.UserID && (r.friendship?.ToUserID == currentUser.UserID) && r.friendship.Pending)
                .Select(nF => new SearchReadUsersDto
                {
                    FriendshipId = nF.friendship.FriendshipId,
                    Email = nF.user.Email,
                    FirstName = nF.user.FirstName,
                    Type = "Pending"
                }).ToList();

            nonFriends = searchMatches.Where(r => !friends.Exists(f => f.Email == r.user.Email) 
                                           && !requests.Exists(rr => rr.Email == r.user.Email)
                                           && !friendsRequests.Exists(fr => fr.Email == r.user.Email)
                                           && !nonFriends.Exists(n => n.Email == r.user.Email)
                                           && r.user.Email != currentUser.Email
                                           && r.friendship is not null)
                .Select(nF => new SearchReadUsersDto
                {
                    FriendshipId = nF.friendship.FriendshipId,
                    Email = nF.user.Email,
                    FirstName = nF.user.FirstName,
                    Type = "None"
                }).ToList().DistinctBy(x => x.Email).ToList();
            

            var finalsearchMatches = friendsRequests.Union(friends.Union(requests).Union(nonFriends).ToList()).ToList();

            return finalsearchMatches;


        }

    }
}

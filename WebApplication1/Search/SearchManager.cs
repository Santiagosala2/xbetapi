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

            var result = await (from user in _context.Set<User>()
                                join friendship in _context.Set<Friendship>()
                                on user.UserID equals friendship.FromUserID into grouping
                                from friendship in grouping.DefaultIfEmpty()
                                
                                select new {
                                    user,
                                    friendship
                                }).ToListAsync();

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            // get friends
            List<SearchReadUsersDto> friends = new List<SearchReadUsersDto>();
            List<SearchReadUsersDto> requests = new List<SearchReadUsersDto>();
            List<SearchReadUsersDto> nonFriends = new List<SearchReadUsersDto>();

            var friendsAndRequests = result.Where(r => r.user.UserID == currentUser.UserID && (r.friendship?.FromUserID == currentUser.UserID || r.friendship?.ToUserID == currentUser.UserID))
                     .Select(rQ =>
                     {
                         var u = result.FirstOrDefault(o => (o.user.UserID == rQ.friendship?.FromUserID &&  rQ.friendship?.FromUserID != rQ.user.UserID) || (o.user.UserID == rQ.friendship?.ToUserID && rQ.friendship?.ToUserID != rQ.user.UserID));

                             return new SearchReadUsersDto
                             {
                                 FriendshipId = rQ.friendship.FriendshipId,
                                 Email = u.user.Email,
                                 FirstName = u.user.FirstName,
                                 Type = rQ.friendship!.Pending == false ? "Friend" : "Request"
                             };
                                           
            }).ToList().Where(u => u != null).ToList();

            friends = result.Where(r => r.user.UserID != currentUser.UserID && (r.friendship?.FromUserID == currentUser.UserID || r.friendship?.ToUserID == currentUser.UserID) && !r.friendship.Pending)

                .Select(nF => new SearchReadUsersDto
                {
                    FriendshipId = nF.friendship.FriendshipId,
                    Email = nF.user.Email,
                    FirstName = nF.user.FirstName,
                    Type = "Friend"
                }).ToList();

            requests = result.Where(r => r.user.UserID != currentUser.UserID && (r.friendship?.ToUserID == currentUser.UserID) && r.friendship.Pending)
                .Select(nF => new SearchReadUsersDto
                {
                    FriendshipId = nF.friendship.FriendshipId,
                    Email = nF.user.Email,
                    FirstName = nF.user.FirstName,
                    Type = "Pending"
                }).ToList();

            nonFriends = result.Where(r => !friends.Exists(f => f.Email == r.user.Email) 
                                           && !requests.Exists(rr => rr.Email == r.user.Email)
                                           && !friendsAndRequests.Exists(fr => fr.Email == r.user.Email)
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
            

            var finalResult = friendsAndRequests.Union(friends.Union(requests).Union(nonFriends).ToList()).ToList();

            finalResult = finalResult.Where(r => r.Email.Contains(keyword)).ToList();

            return finalResult;


        }

    }
}

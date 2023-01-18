using DataStore;
using Microsoft.AspNetCore.Identity;
using AuthServer.Helpers.Hasher;
using Users.Models;
using Friends.Models;
using Search.Users.Dtos;

namespace Auth.UserManager
{
    public class UserManager : IUserManager
    {
        private DataStoreContext _context;

        public UserManager(DataStoreContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUserAsync(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userEmailExists = await CheckEmailExists(user.Email);

            if (!userEmailExists)
            {
                var hashResult = PasswordHasher.HashPasswordV3(user.Password);
                user.Password = hashResult;
                await _context.Users.AddAsync(user);
                int userSaved = await _context.SaveChangesAsync();
                return userSaved == 1;
            }

            return false;

        }

        public async Task<bool> VerifyUserAsync(string email, string password)
        {

            if (email == null || password == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            var findUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            bool authResult = false;

            if (findUser is not null)
            {
                authResult = PasswordHasher.VerifyHashedPasswordV3(findUser.Password, password);
            }

            if (authResult)
            {
                return true;
            }

            return false;

        }

        public async Task<User> GetUserAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
            return user!;
        }

        public async Task<(bool,string)> AddFriendRequest(string fromEmail ,string toEmail)
        {
            var errorMessage = "Friendship already exists"; 
            var toUser = await _context.Users.FirstOrDefaultAsync(user => user.Email == toEmail);
            var fromUser = await _context.Users.FirstOrDefaultAsync(user => user.Email == fromEmail);
            var findFriendship = await _context.Friendships.FirstOrDefaultAsync(t => t.FromUserID == fromUser.UserID && t.ToUserID == toUser.UserID);
            if (findFriendship is null)
            {
                errorMessage = "One of the users is null";


                if (toUser != null && fromUser != null )
                {
                    Friendship friendship = new Friendship
                    {
                        FromUserID = fromUser.UserID,
                        ToUserID = toUser.UserID,
                        Pending = true
                    };
                    await _context.Friendships.AddAsync(friendship);
                    errorMessage = "";
                    return (await _context.SaveChangesAsync() == 1, errorMessage);

                }                     
            }
            return (false,errorMessage);       
        }

        public async Task<(bool, string)> AcceptRequest(int friendshipId , string userEmail )
        {
            var user = await this.GetUserAsync(userEmail);
            var errorMessage = "Friendship does not exist";
            var findFriendship = await _context.Friendships.FirstOrDefaultAsync(f => f.FriendshipId == friendshipId && (f.FromUserID == user.UserID || f.ToUserID == user.UserID));
            if (findFriendship is not null)
            {
                findFriendship.Pending = false;
                errorMessage = "";
                return (await _context.SaveChangesAsync() == 1, errorMessage);
                
            }
            return (false, errorMessage);

        }

        public async Task<(bool, string)> RejectRequest(int friendshipId , string userEmail)
        {
            var user = await this.GetUserAsync(userEmail);
            var errorMessage = "Friendship does not exist";
            var findFriendship = await _context.Friendships.FirstOrDefaultAsync(f => f.FriendshipId == friendshipId && (f.FromUserID == user.UserID || f.ToUserID == user.UserID));
            if (findFriendship is not null)
            {
                errorMessage = "";
                _context.Remove(findFriendship);
                return (await _context.SaveChangesAsync() == 1, errorMessage);

            }
            return (false, errorMessage);

        }

        public async Task<List<SearchReadUsersDto>> GetFriendsAsync(string userEmail)
        {

            var result = await (from user in _context.Set<User>()
                                join friendship in _context.Set<Friendship>()
                                on user.UserID equals friendship.FromUserID into grouping
                                from friendship in grouping.DefaultIfEmpty()

                                select new
                                {
                                    user,
                                    friendship
                                }).ToListAsync();

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            // get friends
            List<SearchReadUsersDto> friends = new List<SearchReadUsersDto>();

            var friendsAndRequests = result.Where(r => r.user.UserID == currentUser.UserID && (r.friendship?.FromUserID == currentUser.UserID || r.friendship?.ToUserID == currentUser.UserID) && (r.friendship?.Pending == false))
                     .Select(rQ =>
                     {
                         var u = result.FirstOrDefault(o => (o.user.UserID == rQ.friendship?.FromUserID && rQ.friendship?.FromUserID != rQ.user.UserID) || (o.user.UserID == rQ.friendship?.ToUserID && rQ.friendship?.ToUserID != rQ.user.UserID));

                         return new SearchReadUsersDto
                         {
                             FriendshipId = rQ.friendship.FriendshipId,
                             Email = u.user.Email,
                             FirstName = u.user.FirstName,
                             Type = "Friend"
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

            return friends.Union(friendsAndRequests).ToList();


        }

        private async Task<bool> CheckEmailExists(string email)
        {
            bool exist = await _context.Users.AnyAsync(otherUser => otherUser.Email == email);
            return exist;
        }

    }
}

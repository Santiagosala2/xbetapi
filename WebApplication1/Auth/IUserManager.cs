using Search.Users.Dtos;
using Users.Models;

namespace Auth.UserManager
{
    public interface IUserManager
    {
        Task<bool> VerifyUserAsync(string email, string password);
        Task<bool> CreateUserAsync(User user);
        Task<User> GetUserAsync(string email);
        Task<(bool, string)> AddFriendRequest(string fromEmail, string toEmail);
        Task<(bool, string)> AcceptRequest(int friendshipId, string userEmail);
        Task<(bool, string)> RejectRequest(int friendshipId, string userEmail);
        Task<List<SearchReadUsersDto>> GetFriendsAsync(string userEmail);
    }
}
using Search.Users.Dtos;
using Users.Models;

namespace Search.Manager
{
    public interface ISearchManager
    {
        Task<List<SearchReadUsersDto>> GetUsersAsync(string keyword, string userEmail);
    }
}
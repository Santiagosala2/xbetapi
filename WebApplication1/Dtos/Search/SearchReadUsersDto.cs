using System.ComponentModel.DataAnnotations;

namespace Search.Users.Dtos
{
    public class SearchReadUsersDto
    {
        public int FriendshipId { get; set; }
        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string Type { get; set; } = null!;
    }
}
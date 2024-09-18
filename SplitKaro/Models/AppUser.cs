using Microsoft.AspNetCore.Identity;

namespace SplitKaro.Models
{
    public class AppUser : IdentityUser
    {

        public List<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
    }
}

namespace SplitKaro.Dtos.GroupMember
{
    public class GroupMemberDto
    {
        public string UserId { get; set; } // The user's ID
        public string UserName { get; set; } // The user's name
        public string Email { get; set; } // The user's email
        public string Role { get; set; } // Role in the group (Admin, Member, etc.)
        public DateTime DateJoined { get; set; } // Date the user joined the group
    }
}

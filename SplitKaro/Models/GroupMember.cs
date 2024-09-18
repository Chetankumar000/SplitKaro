using System.ComponentModel.DataAnnotations.Schema;

namespace SplitKaro.Models
{
    [Table("GroupMembers")]
    public class GroupMember
    {
        public string? AppUserId { get; set; }
        public AppUser AppUser { get; set; } = new AppUser();

        public int GroupId { get; set; }

        public Group Group { get; set; } = new Group();

        public string Role { get; set; } = "Member";
    }
}

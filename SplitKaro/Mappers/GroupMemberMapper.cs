using SplitKaro.Dtos.GroupMember;
using SplitKaro.Models;

namespace SplitKaro.Mappers
{
    public static class GroupMemberMapper
    {
        public static GroupMemberDto ToGroupMemberDto(this GroupMember groupMember)
        {
            var newGM =  new GroupMemberDto
            {
                UserId = groupMember.AppUserId,
                UserName = groupMember.AppUser.UserName,
                Email = groupMember.AppUser.Email,
                Role = groupMember.Role,
                DateJoined = DateTime.Now,
            };
            return newGM;
        }
    }
}

using SplitKaro.Models;

namespace SplitKaro.Interfaces
{
    public interface IGroupMemberRepository
    {
        Task<List<GroupMember>> GetAllAsync(int GroupId);

        Task<GroupMember> GetGroupMemberAsync(string userId, int groupId);

        Task<GroupMember> CreateAsync(GroupMember groupMember);

        Task<GroupMember> DeleteAsync(GroupMember groupMember);
    }
}

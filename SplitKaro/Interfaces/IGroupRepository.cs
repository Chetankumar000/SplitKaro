using SplitKaro.Dtos.Group;
using SplitKaro.Helpers;
using SplitKaro.Models;

namespace SplitKaro.Interfaces
{
    public interface IGroupRepository
    {
        Task<List<Group>> GetAllGroupsAsync(QueryObject queryObject);

        Task<Group> GetGroupByIdAsync(int Groupid);

        Task<Group> CreateGroupAsync(Group groupModel);
        
        Task<Group> UpdateGroupAsync(int groupId, UpdateGroupDto groupModel);

        Task<Group> DeleteGroupAsync(int GroupId);
        Task<IEnumerable<Expense>> GetExpensesByGroupIdAsync(int groupId);

        Task<bool> GroupExists(int Groupid);
    }
}

using Microsoft.EntityFrameworkCore;
using SplitKaro.Data;
using SplitKaro.Dtos.Group;
using SplitKaro.Helpers;
using SplitKaro.Interfaces;
using SplitKaro.Models;

namespace SplitKaro.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDBContext _context;

        public GroupRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Group> CreateGroupAsync(Group groupModel)
        {
            await _context.AddAsync(groupModel);
            await _context.SaveChangesAsync();
            return groupModel;
        }

        public async Task<Group> DeleteGroupAsync(int GroupId)
        {
            var existingGroup = await _context.Groups.FirstOrDefaultAsync(global => global.GroupId == GroupId);

            if (existingGroup == null)
            {
                return null;
            }
            _context.Groups.Remove(existingGroup);
            await _context.SaveChangesAsync();
            return existingGroup;
        }

        public async Task<List<Group>> GetAllGroupsAsync(QueryObject query)
        {
            var groups =  _context.Groups.Include(g=> g.Expenses).Include(g=> g.CreatedBy).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.GroupName))
            {
                groups = groups.Where(g=> g.GroupName.Contains(query.GroupName));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy == "GroupName")
                {
                    groups = query.IsDecending ? groups.OrderByDescending(g=> g.GroupName) : groups.OrderBy(g=> g.GroupName);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize; 

            return await groups.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesByGroupIdAsync(int groupId)
        {
            return await _context.Expenses.Where(e => e.GroupId == groupId).ToListAsync();
        }

        public async Task<Group> GetGroupByIdAsync(int Groupid)
        {
            return await _context.Groups.Include(g=> g.Expenses).Include(g=> g.CreatedBy).FirstOrDefaultAsync(g => g.GroupId == Groupid);
        }

        public async Task<bool> GroupExists(int Groupid)
        {
            return await _context.Groups.AnyAsync(g => g.GroupId == Groupid);
        }

       

        public async Task<Group> UpdateGroupAsync(int GroupId , UpdateGroupDto groupModel)
        {
            var existingGroup = await _context.Groups.FirstOrDefaultAsync(global => global.GroupId == GroupId);

            if(existingGroup == null)
            {
                return null;
            }

            existingGroup.GroupName = groupModel.GroupName;
            await _context.SaveChangesAsync();
            return existingGroup;


        }
    }
}

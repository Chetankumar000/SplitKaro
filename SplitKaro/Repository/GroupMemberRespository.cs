using Microsoft.EntityFrameworkCore;
using SplitKaro.Data;
using SplitKaro.Interfaces;
using SplitKaro.Models;

namespace SplitKaro.Repository
{
    public class GroupMemberRespository : IGroupMemberRepository
    {
        private readonly AppDBContext _context;

        public GroupMemberRespository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<GroupMember> CreateAsync(GroupMember groupMember)
        {
            await _context.GroupMembers.AddAsync(groupMember);
            await _context.SaveChangesAsync();
            return groupMember;
        }

        public async Task<List<GroupMember>> GetAllAsync(int GroupId)
        {
            var users = await _context.GroupMembers.Where(g => g.GroupId == GroupId).ToListAsync();
            return users;
        }

        public async Task<GroupMember> GetGroupMemberAsync(string userId, int groupId)
        {
            var groupMember =  await _context.GroupMembers.FirstOrDefaultAsync(gm => gm.AppUserId == userId && gm.GroupId == groupId);
            return groupMember;
        }

        public async Task<GroupMember> DeleteAsync(GroupMember groupMember)
        {
            _context.GroupMembers.Remove(groupMember);
            await _context.SaveChangesAsync();
            return groupMember;

        }
    }
}

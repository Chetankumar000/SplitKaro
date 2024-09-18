using Microsoft.AspNetCore.Http.HttpResults;
using SplitKaro.Dtos.Expense;
using SplitKaro.Dtos.Group;
using SplitKaro.Models;

namespace SplitKaro.Mappers
{
    public static class GroupMapper
    {
        public static GroupDto toGroupDto(this Group GroupModel)
        {
            return new GroupDto
            {
                GroupId = GroupModel.GroupId,
                GroupName = GroupModel.GroupName,
                CreatedBy = GroupModel.CreatedBy.UserName,
                Expenses = GroupModel.Expenses?.Select(e => e.ToExpenseDto()).ToList()
            };
        }

        public static Group toGroupFromCreateDto(this CreateGroupDto CreateGroupDto)
        {
            return new Group
            {
                GroupName = CreateGroupDto.GroupName,
              
            };
            

        }
    }
}

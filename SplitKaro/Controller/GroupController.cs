using api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SplitKaro.Dtos.Expense;
using SplitKaro.Dtos.Group;
using SplitKaro.Helpers;
using SplitKaro.Interfaces;
using SplitKaro.Mappers;
using SplitKaro.Models;
using SplitKaro.Repository;
using System.Security.Claims;

namespace SplitKaro.Controller
{
    [ApiController]
    [Route("api/group")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGroupMemberRepository _groupMemberRepository;

        public GroupController(IGroupRepository groupRepository, UserManager<AppUser> userManager, IGroupMemberRepository groupMemberRepository)
        {
            _groupRepository = groupRepository;
            _userManager = userManager;
            _groupMemberRepository = groupMemberRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var groups = await _groupRepository.GetAllGroupsAsync(query);
            var groupDto = groups.Select(s => s.toGroupDto()).ToList();

            return Ok(groupDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(GroupDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var group = await _groupRepository.GetGroupByIdAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group.toGroupDto());
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(201, Type = typeof(GroupDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create([FromBody] CreateGroupDto groupDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get the logged-in user's ID
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.GetUserEmail();
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return Unauthorized("User is not logged in.");
            }

            // Convert the DTO to the domain model
            var groupModel = groupDto.toGroupFromCreateDto();

            // Set the CreatedById to the current user's ID
            groupModel.CreatedById = user.Id;

            // Optionally, add the creator as a member of the group
           
  

            // Create the group using the repository
            var createdGroup = await _groupRepository.CreateGroupAsync(groupModel);


            var groupMember = new GroupMember
            {
                AppUserId = user.Id,
                GroupId = createdGroup.GroupId,
                Group = groupModel,
                Role = "Admin", // Assuming role-based membership,
                AppUser = user,
            };
            await _groupMemberRepository.CreateAsync(groupMember);

            return CreatedAtAction(nameof(GetById), new { id = createdGroup.GroupId }, createdGroup.toGroupDto());
        }

        [HttpPut("{GroupId:int}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(GroupDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] int GroupId, [FromBody] UpdateGroupDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Ensure the user is the creator or has permissions
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingGroup = await _groupRepository.GetGroupByIdAsync(GroupId);
            if (existingGroup == null)
            {
                return NotFound();
            }

            if (existingGroup.CreatedById != userId)
            {
                return Unauthorized("Only the group creator can update the group.");
            }

            var groupModel = await _groupRepository.UpdateGroupAsync(GroupId, updateDto);
            return Ok(groupModel.toGroupDto());
        }

        [HttpDelete("{GroupId:int}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete([FromRoute] int GroupId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Ensure the user is the creator or has permissions
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingGroup = await _groupRepository.GetGroupByIdAsync(GroupId);
            if (existingGroup == null)
            {
                return NotFound();
            }

            if (existingGroup.CreatedById != userId)
            {
                return Unauthorized("Only the group creator can delete the group.");
            }

            var groupModel = await _groupRepository.DeleteGroupAsync(GroupId);
            return NoContent();
        }

        [HttpGet("expenses/{GroupId:int}")]
        [ProducesResponseType(200, Type = typeof(List<ExpenseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetExpensesByGroupId([FromRoute] int GroupId)
        {
            var expenses = await _groupRepository.GetExpensesByGroupIdAsync(GroupId);
            if (expenses == null || !expenses.Any())
            {
                return NotFound("No expenses found for this group.");
            }

            var expensesDto = expenses.Select(e => e.ToExpenseDto()).ToList();
            return Ok(expensesDto);
        }
    }
}

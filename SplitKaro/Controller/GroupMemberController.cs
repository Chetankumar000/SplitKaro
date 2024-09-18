using api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SplitKaro.Interfaces;
using SplitKaro.Mappers;
using SplitKaro.Models;
using System.Security.Claims;

namespace SplitKaro.Controller
{
    public class GroupMemberController : ControllerBase
    {
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGroupRepository _groupRepository;

        public GroupMemberController(IGroupMemberRepository groupMemberRepository, UserManager<AppUser> userManager, IGroupRepository groupRepository )
        {
            _groupMemberRepository = groupMemberRepository;
            _userManager = userManager;
            _groupRepository = groupRepository;
        }
        [HttpGet]
        [Authorize]
        [Route("{GroupId}/members")]
        public async Task<IActionResult> GetAll([FromRoute] int GroupId)
        {
            var groupmembers = await _groupMemberRepository.GetAllAsync(GroupId);
            var groupMembersDto = groupmembers.Select(g=> g.ToGroupMemberDto()).ToList();
            return Ok(groupmembers);
        }

        [HttpPost]
        [Authorize]
        [Route("{GroupId}")]
        public async Task<IActionResult> AddAsync([FromRoute] int GroupId, [FromQuery] string email)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userEmail = User.GetUserEmail();
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return Unauthorized("User is not logged in.");
            }

            if (string.IsNullOrEmpty(user.Id))
            {
                return Unauthorized("User is not logged in.");
            }

            // Check if the current user is an admin of the group
            var currentGroupMember = await _groupMemberRepository.GetGroupMemberAsync(user.Id, GroupId);

            if (currentGroupMember == null || currentGroupMember.Role != "Admin")
            {
                return Forbid("Only admins can add users to the group.");
            }

            // Find the user to be added by email
            var userToAdd = await _userManager.FindByEmailAsync(email);

            if (userToAdd == null)
            {
                return NotFound("User with the provided email does not exist.");
            }

            // Check if the user is already a member of the group
            var existingGroupMember = await _groupMemberRepository.GetGroupMemberAsync(userToAdd.Id, GroupId);
            if (existingGroupMember != null)
            {
                return BadRequest("User is already a member of the group.");
            }
            var Group = await _groupRepository.GetGroupByIdAsync(GroupId);

            // Add the user to the group
            var newGroupMember = new GroupMember
            {
                AppUserId = userToAdd.Id,
                GroupId = GroupId,
                Role = "Member", // Default role for the new user
                AppUser = userToAdd,
                Group = Group,
            };

             var groupMember = await _groupMemberRepository.CreateAsync(newGroupMember);
            return Ok(groupMember.ToGroupMemberDto());

        }

        [HttpDelete]
        [Authorize]
        [Route("{GroupId}")]
        public async Task<IActionResult> RemoveAsync([FromRoute] int GroupId, [FromQuery] string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userEmail = User.GetUserEmail();
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return Unauthorized("User is not logged in.");
            }

            if (string.IsNullOrEmpty(user.Id))
            {
                return Unauthorized("User is not logged in.");
            }

            // Check if the current user is an admin of the group
            var currentGroupMember = await _groupMemberRepository.GetGroupMemberAsync(user.Id, GroupId);

            if (currentGroupMember == null || currentGroupMember.Role != "Admin")
            {
                return Forbid("Only admins can add users to the group.");
            }

            // Find the user to be added by email
            var userToRemove = await _userManager.FindByEmailAsync(email);

            if (userToRemove == null)
            {
                return NotFound("GroupMember with the provided email does not exist.");
            }

            // Check if the user is already a member of the group
            var existingGroupMember = await _groupMemberRepository.GetGroupMemberAsync(userToRemove.Id, GroupId);
            if (existingGroupMember == null)
            {
                return BadRequest("User is not a member of the group.");
            }
            var Group = await _groupRepository.GetGroupByIdAsync(GroupId);

            // Add the user to the group
       

            var groupMember = await _groupMemberRepository.DeleteAsync(existingGroupMember);
            return Ok("GroupMember Deleted Successfully");

        }

    }
}

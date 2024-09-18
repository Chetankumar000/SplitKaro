using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitKaro.Models
{
    [Table("Groups")]
    public class Group
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; } = string.Empty;

        // Foreign key for the user who created the group

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow; // Use UtcNow for consistency

        public string CreatedById { get; set; } = string.Empty; // Ensure a default value is provided


        public AppUser CreatedBy { get; set; }  // Navigation property

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

        public List<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
    }
} 

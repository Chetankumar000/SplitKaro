using SplitKaro.Dtos.Expense;
using SplitKaro.Models;
using System.ComponentModel.DataAnnotations;

namespace SplitKaro.Dtos.Group
{
    public class GroupDto
    {
        [Required]
        public int GroupId { get; set; }

        [Required]
        public string GroupName { get; set; } = string.Empty;

        [Required]
        public string? CreatedBy { get; set; }

        public List<ExpenseDto> Expenses { get; set; } = new List<ExpenseDto>();
    }
}

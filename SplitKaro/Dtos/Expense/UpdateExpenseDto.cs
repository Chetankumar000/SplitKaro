using System.ComponentModel.DataAnnotations;

namespace SplitKaro.Dtos.Expense
{
    public class UpdateExpenseDto
    {
        [Required]
        public string ExpenseName { get; set; } = string.Empty;
        [Required]
        public string PaidBy { get; set; } = string.Empty;
        [Required]
        public decimal Amount { get; set; }
    }
}

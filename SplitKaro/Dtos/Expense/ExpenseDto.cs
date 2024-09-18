using System.ComponentModel.DataAnnotations;

namespace SplitKaro.Dtos.Expense
{
    public class ExpenseDto
    {
        [Required]

        public int ExpenseId { get; set; }
        [Required]

        public string ExpenseName { get; set; } = string.Empty;
        [Required]

        public string PaidBy { get; set; } = string.Empty;
        [Required]

        public decimal Amount { get; set; }
        [Required]

        public int GroupId { get; set; }
    }
}

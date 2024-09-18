using System.ComponentModel.DataAnnotations.Schema;

namespace SplitKaro.Models
{
    [Table("Expenses")]
    public class Expense
    {
        public int ExpenseId { get; set; }

        public string ExpenseName { get; set;} = string.Empty;

        public string PaidBy { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public int GroupId { get; set; }

        public Group? Group { get; set; }

    }
}

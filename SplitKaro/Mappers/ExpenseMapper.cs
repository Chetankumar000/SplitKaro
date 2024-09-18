using SplitKaro.Dtos.Expense;
using SplitKaro.Models;

namespace SplitKaro.Mappers
{
    public static class ExpenseMapper
    {
        public static ExpenseDto ToExpenseDto(this Expense expenseDto)
        {
            return new ExpenseDto { 
               ExpenseId = expenseDto.ExpenseId,    
                ExpenseName = expenseDto.ExpenseName,
                Amount = expenseDto.Amount,
                PaidBy = expenseDto.PaidBy,
                GroupId = expenseDto.GroupId,


            };

        }
        public static Expense ToExpenseFromCreateDto(this CreateExpenseDto expenseDto, int GroupId)
        {
            return new Expense
            {
                ExpenseName = expenseDto.ExpenseName,
                Amount = expenseDto.Amount,
                PaidBy = expenseDto.PaidBy,
                GroupId = GroupId,


            };

        }
        public static Expense ToExpenseFromUpdateDto(this UpdateExpenseDto expenseDto)
        {
            return new Expense
            {
                ExpenseName = expenseDto.ExpenseName,
                Amount = expenseDto.Amount,
                PaidBy = expenseDto.PaidBy,
       


            };

        }
    }
}

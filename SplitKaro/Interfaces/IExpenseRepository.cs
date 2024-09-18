using SplitKaro.Models;

namespace SplitKaro.Interfaces
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetExpenses();

        Task<Expense> GetExpenseById(int ExpenseId);

        Task<List<Expense>> GetExpensesByGroupId(int GroupId);

        Task<Expense> CreateExpenseAsync(Expense expense);

        Task<Expense> UpdateExpenseAsync(int ExpenseId, Expense expense);

        Task<Expense> DeleteExpenseAsync(int ExpenseId);


    }
}

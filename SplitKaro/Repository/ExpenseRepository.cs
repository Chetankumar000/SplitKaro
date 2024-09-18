using Microsoft.EntityFrameworkCore;
using SplitKaro.Data;
using SplitKaro.Interfaces;
using SplitKaro.Models;

namespace SplitKaro.Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDBContext _context;

        public ExpenseRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Expense> CreateExpenseAsync(Expense expense)
        {
            await _context.AddAsync(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<Expense> DeleteExpenseAsync(int ExpenseId)
        {
            var existingExpense = await _context.Expenses.FindAsync(ExpenseId);

            if (existingExpense == null)
            {
                return null;
            }
            _context.Remove(existingExpense);
            await _context.SaveChangesAsync();
            return existingExpense;

        }

        public async Task<Expense> GetExpenseById(int ExpenseId)
        {
            return await _context.Expenses.FindAsync(ExpenseId);
        }

        public async Task<List<Expense>> GetExpenses()
        {
            return await _context.Expenses.ToListAsync();
        }

        public async Task<List<Expense>> GetExpensesByGroupId(int GroupId)
        {
            return await _context.Expenses.Where(e=> e.GroupId == GroupId).ToListAsync();
        }

        public async Task<Expense> UpdateExpenseAsync(int ExpenseId, Expense expense)
        {
            var existingExpense = await _context.Expenses.FindAsync(ExpenseId);

            if(existingExpense == null)
            {
                return null;
            }

            existingExpense.Amount = expense.Amount;
            existingExpense.ExpenseName = expense.ExpenseName;
            await _context.SaveChangesAsync();
            return existingExpense;

        }
    }
}

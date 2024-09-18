using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitKaro.Dtos.Expense;
using SplitKaro.Dtos.Group;
using SplitKaro.Interfaces;
using SplitKaro.Mappers;
using SplitKaro.Models;
using SplitKaro.Repository;

namespace SplitKaro.Controller
{
    [ApiController]
    [Route("api/expense")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IGroupRepository _groupRepository;

        public ExpenseController(IExpenseRepository expenseRepository, IGroupRepository groupRepository)
        {
            _expenseRepository = expenseRepository;
            _groupRepository = groupRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var expenses = await _expenseRepository.GetExpenses();
            var expensesDto = expenses.Select(e => e.ToExpenseDto()).ToList();
            return Ok(expensesDto);
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(ExpenseDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var expense = await _expenseRepository.GetExpenseById(id);

            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense.ToExpenseDto());
        }

        [HttpPost("{GroupId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateExpenseDto expenseDto, [FromRoute] int GroupId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var group = await _groupRepository.GroupExists(GroupId);
            if (group == null)
            {
                return BadRequest("Group does not exist");
            }

            var expense = expenseDto.ToExpenseFromCreateDto(GroupId);

            await _expenseRepository.CreateExpenseAsync(expense);

            return CreatedAtAction(nameof(GetById), new { id = expense.ExpenseId }, expense.ToExpenseDto());
        }

        [HttpPut]
        [Route("{ExpenseId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update([FromRoute] int ExpenseId, [FromBody] UpdateExpenseDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateExpense = updateDto.ToExpenseFromUpdateDto();

            var expenseModel = await _expenseRepository.UpdateExpenseAsync(ExpenseId, updateExpense);

            if (expenseModel == null)
            {
                return NotFound();
            }

            return Ok(expenseModel.ToExpenseDto());
        }

        [HttpDelete]
        [Route("{ExpenseId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public async Task<IActionResult> Delete([FromRoute] int ExpenseId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var expenseModel = await _expenseRepository.DeleteExpenseAsync(ExpenseId);

            if (expenseModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }

       

    }
}

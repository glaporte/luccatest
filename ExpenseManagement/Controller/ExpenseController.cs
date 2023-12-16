using ExpenseManagement.DataModel.Context;
using ExpenseManagement.DataModel.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ExpenseManagement.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExpenseController : ControllerBase
	{
		[BindProperties]
		public class GetRequestFilter
		{
			[JsonConverter(typeof(JsonStringEnumConverter))]
			public enum Sort
			{
				[EnumMember(Value = "none")]
				None,
				[EnumMember(Value = "Asc")]
				Ascending,
				[EnumMember(Value = "Desc")]
				Descending
			}
			public Sort SortByDate { get; set; } = Sort.None;
			public Sort SortByAmount { get; set; } = Sort.None;
		}

		private readonly ExpenseContext _expenseContext;
		private readonly UserContext _userContext;

		public ExpenseController(ExpenseContext expenseContext, UserContext userContext)
		{
			_expenseContext = expenseContext ?? throw new ArgumentNullException(nameof(expenseContext));
			_userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
		}

		[HttpGet]
		[Route("all")]
		[Produces(typeof(IEnumerable<Expense>))]
		public async Task<IActionResult> GetExpenses([FromQuery] GetRequestFilter getFilter)
		{
			var expenses = await _expenseContext.Select(null, getFilter);

			return Ok(expenses);
		}

		[HttpGet]
		[Route("id")]
		[Produces(typeof(Expense))]
		public async Task<IActionResult> GetExpense(int id)
		{
			Expense? expense = await _expenseContext.Expenses.FindAsync(id);
			if (expense == null)
			{
				return NotFound();
			}

			return Ok(expense);
		}

		[HttpGet]
		[Route("user")]
		[Produces(typeof(IEnumerable<Expense>))]
		public async Task<IActionResult> GetExpenseForUser(int userId, [FromQuery] GetRequestFilter getFilter)
		{
			if (_userContext.Users.Count(u => u.Id == userId) == 0)
			{
				return BadRequest();
			}

			var expenses = await _expenseContext.Select(x => x.UserId == userId, getFilter);
			return Ok(expenses);
		}

		[HttpPost]
		[Route("add")]
		[Produces(typeof(Expense))]
		public async Task<IActionResult> AddExpense(Expense expense)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_expenseContext.Expenses.Add(expense);
			await _expenseContext.SaveChangesAsync();

			return Ok(expense);
		}


		[HttpDelete]
		[Route("del")]
		[Produces(typeof(Expense))]
		public async Task<IActionResult> DeleteExpense(int id)
		{
			Expense? expense = await _expenseContext.Expenses.FindAsync(id);
			if (expense == null)
			{
				return NotFound();
			}

			_expenseContext.Expenses.Remove(expense);
			await _expenseContext.SaveChangesAsync();

			return Ok(expense);
		}
	}
}

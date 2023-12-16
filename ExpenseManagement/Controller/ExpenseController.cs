﻿using ExpenseManagement.DataModel.Context;
using ExpenseManagement.DataModel.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ExpenseManagement.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExpenseController : ControllerBase
	{
		private readonly ExpenseContext _expenseContext;

		public ExpenseController(ExpenseContext context)
		{
			_expenseContext = context ?? throw new ArgumentNullException(nameof(context));
		}

		[HttpGet]
		[Route("all")]
		[Produces(typeof(IEnumerable<Expense>))]
		public async Task<IActionResult> GetExpenses()
		{
			var expenses = await _expenseContext.Expenses.ToListAsync();

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
		public async Task<IActionResult> GetExpenseForUser(int userId)
		{
			var expenses = await _expenseContext.Select(x => x.UserId == userId);
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

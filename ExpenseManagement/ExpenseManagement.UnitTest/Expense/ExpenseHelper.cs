using ExpenseManagement.DataModel;
using ExpenseManagement.DataModel.Entity;

namespace ExpenseManagement.UnitTest
{
	public static class ExpenseHelper
	{
		public static async Task AddExpense(TestDbContext db, Expense e)
		{
			db.ExpenseContext.Add(e);
			await db.ExpenseContext.SaveChangesAsync();
		}

		public static async Task AddExpense(TestDbContext db, int userId, DateTime date, ExpenseType type,
			float amount, Currency currency, string annotation)
		{
			await AddExpense(db, new Expense()
			{
				UserId = userId,
				Date = date,
				ExpenseType = type,
				Amount = amount,
				Currency = currency,
				Annotation = annotation
			});
		}

	}
}

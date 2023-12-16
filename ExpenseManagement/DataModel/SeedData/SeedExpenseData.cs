using ExpenseManagement.DataModel.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.DataModel.SeedData
{
	public static class SeedExpenseData
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			using (var context = new ExpenseContext(
				serviceProvider.GetRequiredService<
					DbContextOptions<ExpenseContext>>()))
			{
				context.Database.EnsureCreated();
				context.SaveChanges();
			}
		}
	}
}

using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.DataModel.Context
{
	public class ExpenseContext : DbContext
	{
		public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options)
		{ }
	}
}

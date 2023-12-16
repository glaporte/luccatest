using ExpenseManagement.DataModel.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExpenseManagement.DataModel.Context
{
	public class ExpenseContext : DbContext
	{
		public DbSet<Expense> Expenses => Set<Expense>();

		public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options)
		{ }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Expense>().ToTable(nameof(Expense));
			modelBuilder.Entity<Expense>().HasIndex(e => new { e.UserId, e.Date, e.Amount }).IsUnique();
			base.OnModelCreating(modelBuilder);
		}

		public async Task<List<Expense>> Select(Expression<Func<Expense, bool>> filter)
		{
			var result = Expenses.AsQueryable();
			result = result.Where(filter);
			return await result.ToListAsync();
		}

		public async Task<List<Expense>> Filter(Expression<Func<Expense, bool>> filter)
		{
			var result = Expenses.AsQueryable();
			result = result.OrderBy(filter);
			return await result.ToListAsync();
		}
	}
}

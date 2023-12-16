using ExpenseManagement.DataModel.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static ExpenseManagement.Controller.ExpenseController;

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

			modelBuilder.Entity<Expense>()
			  .HasOne(e => e.User)
			  .WithMany(e => e.Expenses)
			  .HasForeignKey(e => e.UserId)
			  .IsRequired();

			base.OnModelCreating(modelBuilder);
		}

		public async Task<List<Expense>> Select(Expression<Func<Expense, bool>>? filter = null, GetRequestFilter? orderBy = null)
		{
			var result = Expenses.AsQueryable();

			if (filter != null)
			{
				result = result.Where(filter);
			}
			if (orderBy != null)
			{
				if (orderBy.SortByAmount == GetRequestFilter.Sort.Ascending)
				{
					result = result.OrderBy(e => e.Amount);
				}
				else if (orderBy.SortByAmount == GetRequestFilter.Sort.Descending)
				{
					result = result.OrderByDescending(e => e.Amount);
				}

				if (orderBy.SortByDate == GetRequestFilter.Sort.Ascending)
				{
					result = result.OrderBy(e => e.Date);
				}
				else if (orderBy.SortByDate == GetRequestFilter.Sort.Descending)
				{
					result = result.OrderByDescending(e => e.Date);
				}
			}

			return await result.ToListAsync();
		}
	}
}

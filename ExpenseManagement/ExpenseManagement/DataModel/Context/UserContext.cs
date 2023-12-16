using ExpenseManagement.DataModel.Entity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.DataModel.Context
{
	public class UserContext : DbContext
	{
		public DbSet<User> Users => Set<User>();

		public UserContext(DbContextOptions<UserContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().ToTable(nameof(User));

			modelBuilder.Entity<User>()
				.HasMany(e => e.Expenses)
				.WithOne(e => e.User)
				.HasForeignKey(e => e.UserId)
				.IsRequired();

			base.OnModelCreating(modelBuilder);
		}

		public bool UserExist(int uid)
		{
			return Users.Count(u => u.Id == uid) == 1;
		}
	}
}

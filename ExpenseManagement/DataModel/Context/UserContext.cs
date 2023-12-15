using ExpenseManagement.DataModel.Entity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.DataModel.Context
{
	public class UserContext : DbContext
	{
		public  DbSet<User> Users => Set<User>();

		public UserContext(DbContextOptions<UserContext> options) : base(options)
		{
		}
	}
}

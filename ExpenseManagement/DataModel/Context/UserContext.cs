using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.DataModel.Context
{
	public class UserContext : DbContext
	{
		public UserContext(DbContextOptions<UserContext> options) : base(options)
		{ }
	}
}

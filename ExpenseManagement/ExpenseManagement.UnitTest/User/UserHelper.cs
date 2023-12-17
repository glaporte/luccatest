using ExpenseManagement.DataModel;
using ExpenseManagement.DataModel.Entity;

namespace ExpenseManagement.UnitTest
{
	public static class UserHelper
	{
		public static async Task AddUser(TestDbContext db, string firstName, string lastName)
		{
			await AddUser(db, firstName, lastName, 0);
		}

		public static async Task AddUser(TestDbContext db, string firstName, string lastName, Currency currency)
		{
			User user = new()
			{
				Firstname = firstName,
				Lastname = lastName,
				PreferredCurrency = currency
			};

			db.UserContext.Users.Add(user);
			await db.UserContext.SaveChangesAsync();
		}
	}
}

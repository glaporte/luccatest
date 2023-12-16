using ExpenseManagement.DataModel.Context;
using ExpenseManagement.DataModel.Entity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.DataModel.SeedData
{
	public static class SeedUserData
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			using (var context = new UserContext(
				serviceProvider.GetRequiredService<
					DbContextOptions<UserContext>>()))
			{
				context.Database.EnsureCreated();
				
				if (context.Users.Any())
				{
					return;
				}

				context.Users.AddRange(
					new User
					{
						Lastname = "Stark",
						Firstname = "Anthony",
						PreferredCurrency = Currency.USD
					},
					new User
					{
						Lastname = "Romanova",
						Firstname = "Natasha",
						PreferredCurrency = Currency.RUB
					}
				);
				context.SaveChanges();
			}
		}
	}
}

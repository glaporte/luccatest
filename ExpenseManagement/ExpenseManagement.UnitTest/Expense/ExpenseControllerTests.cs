using ExpenseManagement.Controller;
using ExpenseManagement.DataModel.Entity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.UnitTest
{
	public class ExpenseControllerTests
	{
		public static IEnumerable<object[]> SortByAmount()
		{
			yield return new object[] { new ExpenseController.GetRequestFilter() { SortByAmount = ExpenseController.GetRequestFilter.Sort.Ascending } };
			yield return new object[] { new ExpenseController.GetRequestFilter() { SortByAmount = ExpenseController.GetRequestFilter.Sort.Descending } };
		}

		public static IEnumerable<object[]> SortByDate()
		{
			yield return new object[] { new ExpenseController.GetRequestFilter() { SortByDate = ExpenseController.GetRequestFilter.Sort.Ascending } };
			yield return new object[] { new ExpenseController.GetRequestFilter() { SortByDate = ExpenseController.GetRequestFilter.Sort.Descending } };
		}

		public static IEnumerable<object[]> ValidUsersId()
		{
			yield return new object[] { 1, 3 };
			yield return new object[] { 2, 2 };
			yield return new object[] { 3, 4 };
		}

		[Fact]
		public async Task GetExpenses_ReturnsValidExpenses_WhenDbNotEmpty()
		{
			using var db = new TestDbContext();
			// Arrange
			await AddUsers(db);
			await AddExpenses(db);

			ExpenseController expenseController = new(db.ExpenseContext, db.UserContext);

			//Act
			var getExpenseResult = await expenseController.GetExpenses(new ExpenseController.GetRequestFilter());

			//Assert
			Assert.NotNull(getExpenseResult);
			Assert.IsType<OkObjectResult>(getExpenseResult);
			Assert.IsType<List<Expense>>((List<Expense>)((OkObjectResult)getExpenseResult!).Value!);
			List<Expense> result = (List<Expense>)((OkObjectResult)getExpenseResult!).Value!;
			Assert.Equal(9, result.Count);
		}

		[Theory]
		[MemberData(nameof(SortByAmount))]
		[MemberData(nameof(SortByDate))]
		public async Task GetExpenses_ReturnsValidExpenses_WithRequestFiltering(ExpenseController.GetRequestFilter filter)
		{
			using var db = new TestDbContext();
			// Arrange
			await AddUsers(db);
			await AddExpenses(db);

			ExpenseController expenseController = new(db.ExpenseContext, db.UserContext);

			//Act
			var getExpenseResult = await expenseController.GetExpenses(filter);

			//Assert
			Assert.NotNull(getExpenseResult);
			Assert.IsType<OkObjectResult>(getExpenseResult);
			Assert.IsType<List<Expense>>((List<Expense>)((OkObjectResult)getExpenseResult!).Value!);
			List<Expense> result = (List<Expense>)((OkObjectResult)getExpenseResult!).Value!;
			Assert.Equal(9, result.Count);

			List<Expense> copy = new(result);

			if (filter.SortByAmount == ExpenseController.GetRequestFilter.Sort.Ascending)
			{
				copy = result.OrderBy(e => e.Amount).ToList();
			}
			else if (filter.SortByAmount == ExpenseController.GetRequestFilter.Sort.Descending)
			{
				copy = result.OrderByDescending(e => e.Amount).ToList();
			}

			if (filter.SortByDate == ExpenseController.GetRequestFilter.Sort.Ascending)
			{
				copy = result.OrderBy(e => e.Date).ToList();
			}
			else if (filter.SortByDate == ExpenseController.GetRequestFilter.Sort.Descending)
			{
				copy = result.OrderByDescending(e => e.Date).ToList();
			}

			Assert.Equal(copy, result);
		}

		// TODO add Combinatorial test system
		[Theory]
		[MemberData(nameof(ValidUsersId))]
		public async Task GetExpensesForUser_ReturnsValidExpenses_WithRequestFiltering(int userId, int expectedResult)
		{
			using var db = new TestDbContext();
			// Arrange
			await AddUsers(db);
			await AddExpenses(db);

			ExpenseController expenseController = new(db.ExpenseContext, db.UserContext);

			//Act
			// TODO replace by Combinatorial test data
			var getExpenseResult = await expenseController.GetExpenseForUser(userId, new ExpenseController.GetRequestFilter() { SortByAmount = ExpenseController.GetRequestFilter.Sort.Descending });

			//Assert
			Assert.NotNull(getExpenseResult);
			Assert.IsType<OkObjectResult>(getExpenseResult);
			Assert.IsType<List<Expense>>((List<Expense>)((OkObjectResult)getExpenseResult!).Value!);
			List<Expense> result = (List<Expense>)((OkObjectResult)getExpenseResult!).Value!;
			Assert.Equal(expectedResult, result.Count);
			foreach (Expense e in result)
			{
				Assert.Equal(userId, e.UserId);
			}
			List<Expense> copy = result.OrderByDescending(e => e.Amount).ToList();
			Assert.Equal(copy, result);
		}

		[Fact]
		public async Task GetExpenses_ForInvalidUser_ReturnsNotFound()
		{
			using var db = new TestDbContext();
			// Arrange
			await AddUsers(db);
			await AddExpenses(db);

			ExpenseController expenseController = new(db.ExpenseContext, db.UserContext);

			//Act
			var getExpenseResult = await expenseController.GetExpenseForUser(4648, new ExpenseController.GetRequestFilter() { SortByAmount = ExpenseController.GetRequestFilter.Sort.Descending });

			//Assert
			Assert.NotNull(getExpenseResult);
			Assert.IsType<BadRequestResult>(getExpenseResult);
		}

		[Fact]
		public void DeleteExpense_ForInvalidId_ReturnsNotFound()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void DeleteExpense_ForValidId_ReturnsSuccess()
		{
			throw new NotImplementedException();
		}


		private static async Task AddUsers(TestDbContext db)
		{
			await UserHelper.AddUser(db, "jean", "de la fontaine", DataModel.Currency.EUR);
			await UserHelper.AddUser(db, "ernest", "Hemingway", DataModel.Currency.USD);
			await UserHelper.AddUser(db, "fyodor", "dostoevsky", DataModel.Currency.RUB);
		}

		private static async Task AddExpenses(TestDbContext db)
		{
			await ExpenseHelper.AddExpense(db, 1, DateTime.Now.AddDays(-2), DataModel.ExpenseType.Restaurant, 30, DataModel.Currency.EUR, "professional");
			await ExpenseHelper.AddExpense(db, 2, DateTime.Now.AddDays(-2), DataModel.ExpenseType.Misc, 80, DataModel.Currency.USD, "good");
			await ExpenseHelper.AddExpense(db, 3, DateTime.Now.AddDays(-2), DataModel.ExpenseType.Hotel, 80, DataModel.Currency.RUB, "good");
			await ExpenseHelper.AddExpense(db, 3, DateTime.Now.AddDays(-2), DataModel.ExpenseType.Hotel, 90, DataModel.Currency.RUB, "good");
			await ExpenseHelper.AddExpense(db, 3, DateTime.Now.AddDays(-3), DataModel.ExpenseType.Hotel, 45, DataModel.Currency.RUB, "good");
			await ExpenseHelper.AddExpense(db, 2, DateTime.Now.AddDays(-5), DataModel.ExpenseType.Hotel, 445, DataModel.Currency.RUB, "good");
			await ExpenseHelper.AddExpense(db, 1, DateTime.Now.AddDays(-6), DataModel.ExpenseType.Hotel, 455, DataModel.Currency.RUB, "good");
			await ExpenseHelper.AddExpense(db, 1, DateTime.Now.AddDays(-50), DataModel.ExpenseType.Hotel, 988, DataModel.Currency.RUB, "good");
			await ExpenseHelper.AddExpense(db, 3, DateTime.Now.AddDays(-1), DataModel.ExpenseType.Misc, 1, DataModel.Currency.RUB, "good");
		}
	}
}

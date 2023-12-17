using ExpenseManagement.DataModel.Entity;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.UnitTest
{
	public class ExpenseTests
	{
		private Expense? _expenseUnderTest;
		private TestDbContext? _dbContext;
		private ServiceCollection? _serviceCollection;

		[Fact]
		public async Task Expense_RequiresUser_Existence()
		{
			// Arrange
			await InitializeExpenseTestContext();
			_expenseUnderTest!.UserId = 8987;
			ValidationContext validationContext = new(_expenseUnderTest, _serviceCollection!.BuildServiceProvider(), null);
			ICollection<ValidationResult> results = new List<ValidationResult>();
			// Act
			Validator.TryValidateObject(_expenseUnderTest, validationContext, results, validateAllProperties: true);
			// Assert
			Assert.Equal(1, results.Count);
			Assert.Contains("Expense user does not exist", results.ElementAt(0).ErrorMessage);

			// Arrange
			_expenseUnderTest.UserId = 1;
			results = new List<ValidationResult>();
			// Act
			Validator.TryValidateObject(_expenseUnderTest, validationContext, results, validateAllProperties: true);
			// Assert
			Assert.Equal(0, results.Count);
		}

		[Fact]
		public async Task Expense_RequiresDate_Clamping()
		{
			// Arrange
			await InitializeExpenseTestContext();
			_expenseUnderTest!.Date = DateTime.Now.AddDays(1);
			ValidationContext validationContext = new ValidationContext(_expenseUnderTest, _serviceCollection!.BuildServiceProvider(), null);
			ICollection<ValidationResult> results = new List<ValidationResult>();
			// Act
			Validator.TryValidateObject(_expenseUnderTest, validationContext, results, validateAllProperties: true);
			// Assert
			Assert.Equal(1, results.Count);
			Assert.Contains("Expense date cannot be in the future", results.ElementAt(0).ErrorMessage);

			// Arrange
			_expenseUnderTest.Date = DateTime.Now.AddDays(-200);
			results = new List<ValidationResult>();
			// Act
			Validator.TryValidateObject(_expenseUnderTest, validationContext, results, validateAllProperties: true);
			// Assert
			Assert.Equal(1, results.Count);
			Assert.Contains("Expense date cannot be more than 3 months old", results.ElementAt(0).ErrorMessage);
		}

		[Fact]
		public async Task Expense_RequiresCurrency_UserIdentity()
		{
			// Arrange
			await InitializeExpenseTestContext();
			_expenseUnderTest!.Currency = DataModel.Currency.USD;
			ValidationContext validationContext = new(_expenseUnderTest, _serviceCollection!.BuildServiceProvider(), null);
			ICollection<ValidationResult> results = new List<ValidationResult>();
			// Act
			Validator.TryValidateObject(_expenseUnderTest, validationContext, results, validateAllProperties: true);
			// Assert
			Assert.Equal(1, results.Count);
			Assert.Contains("Expense currency must be the same as User currency", results.ElementAt(0).ErrorMessage);

			// Arrange
			_expenseUnderTest.Currency = DataModel.Currency.EUR;
			results = new List<ValidationResult>();
			// Act
			Validator.TryValidateObject(_expenseUnderTest, validationContext, results, validateAllProperties: true);
			// Assert
			Assert.Equal(0, results.Count);
		}

		[Fact]
		public async Task Expense_RequiresAnnotation_Validity()
		{
			// Arrange
			await InitializeExpenseTestContext();
			_expenseUnderTest!.Annotation = String.Empty;
			ValidationContext validationContext = new(_expenseUnderTest, _serviceCollection!.BuildServiceProvider(), null);
			ICollection<ValidationResult> results = new List<ValidationResult>();
			// Act
			Validator.TryValidateObject(_expenseUnderTest, validationContext, results, validateAllProperties: true);
			// Assert
			Assert.Equal(1, results.Count);
			Assert.Contains("The Annotation field is required.", results.ElementAt(0).ErrorMessage);
		
			// Arrange
			_expenseUnderTest!.Annotation = " dazd az";
			results = new List<ValidationResult>();
			// Act
			Validator.TryValidateObject(_expenseUnderTest, validationContext, results, validateAllProperties: true);
			// Assert
			Assert.Equal(0, results.Count);
		}

		[Fact]
		public async Task Expense_RequiresUserAndDateAndAmount_ToBeUnique()
		{
			// Arrange
			await InitializeExpenseTestContext();
			_dbContext!.ExpenseContext.Expenses.Add(_expenseUnderTest!);
			_dbContext!.ExpenseContext.SaveChanges();
			ValidationContext validationContext = new(_expenseUnderTest!, _serviceCollection!.BuildServiceProvider(), null);
			ICollection<ValidationResult> results = new List<ValidationResult>();
			// Act
			Validator.TryValidateObject(_expenseUnderTest!, validationContext, results, validateAllProperties: true);
			// Assert
			Assert.Equal(1, results.Count);
			Assert.Contains("Expense with same amount and date already exist for user", results.ElementAt(0).ErrorMessage);
		}

		private async Task InitializeExpenseTestContext()
		{
			_dbContext = new TestDbContext();
			await UserHelper.AddUser(_dbContext, "jean", "delafontaine", DataModel.Currency.EUR);
			_serviceCollection = new ServiceCollection();
			_serviceCollection.AddSingleton(_dbContext.UserContext);
			_serviceCollection.AddSingleton(_dbContext.ExpenseContext);

			_expenseUnderTest = new Expense
			{
				UserId = 1,
				Currency = DataModel.Currency.EUR,
				Date = DateTime.Now.AddDays(-1),
				Annotation = "sdza"
			};
		}
	}
}

using ExpenseManagement.Controller;
using ExpenseManagement.DataModel.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.UnitTest
{
	public class UserTests
	{
		[Fact]
		public void CheckUser_Properties_Validity()
		{
			// test 1

			User user = new User();
			ValidationContext dbContext = new ValidationContext(user);
			ICollection<ValidationResult> results = new List<ValidationResult>();

			Validator.TryValidateObject(user, dbContext, results, validateAllProperties: true);

			Assert.Equal(2, results.Count);
			Assert.Contains("Lastname", results.ElementAt(0).ErrorMessage);
			Assert.Contains("Firstname", results.ElementAt(1).ErrorMessage);


			// test 2

			user = new User();
			user.Firstname = "tto";
			dbContext = new ValidationContext(user);
			results = new List<ValidationResult>();

			Validator.TryValidateObject(user, dbContext, results, validateAllProperties: true);

			Assert.Equal(1, results.Count);
			Assert.Contains("Lastname", results.ElementAt(0).ErrorMessage);
			Assert.DoesNotContain("Firstname", results.ElementAt(0).ErrorMessage);

			// test 3

			user = new User();
			user.Firstname = "tt^@@o";
			user.Lastname = "plop";
			dbContext = new ValidationContext(user);
			results = new List<ValidationResult>();

			Validator.TryValidateObject(user, dbContext, results, validateAllProperties: true);

			Assert.Equal(1, results.Count);
			Assert.Contains("Use \"valid latin naming\" convention:", results.ElementAt(0).ErrorMessage);
			Assert.Contains(User.NamingValidity, results.ElementAt(0).ErrorMessage);

			// test 4

			user = new User();
			user.Firstname = "tto";
			user.Lastname = "plop";
			dbContext = new ValidationContext(user);
			results = new List<ValidationResult>();

			Validator.TryValidateObject(user, dbContext, results, validateAllProperties: true);

			Assert.Equal(0, results.Count);
		}

		[Fact]
		public async Task AddUser_WhenCalled_Returns_Success()
		{
			using (var db = new TestDbContext())
			{
				// Arrange
				User user = new();
				user.Firstname = "glop";
				user.Lastname = "plop";
				UserController userController = new(db.UserContext);

				//Act
				var addUserResult = await userController.AddUser(user);

				//Assert
				Assert.NotNull(addUserResult);
				Assert.IsType<OkObjectResult>(addUserResult);
				Assert.IsType<User>((User)((OkObjectResult)addUserResult!).Value!);
				Assert.Equal("glop", ((User)((OkObjectResult)addUserResult!).Value!).Firstname);
				Assert.Equal("plop", ((User)((OkObjectResult)addUserResult!).Value!).Lastname);
				Assert.Equal(1, ((User)((OkObjectResult)addUserResult!).Value!).Id);
			}
		}

		[Fact]
		public void AddUser_WhenCalled_Returns_Error()
		{
			using (var db = new TestDbContext())
			{
				// Arrange
				User user = new();
				user.Firstname = "glop";
				user.Lastname = null!;

				//Act
				UserController userController = new(db.UserContext);

				//Assert
				var error = Assert.ThrowsAsync<DbUpdateException>(async () => await userController.AddUser(user));
				Assert.Contains("Lastname", error.Result.Message);
			}
		}
	}
}
using ExpenseManagement.Controller;
using ExpenseManagement.DataModel.Entity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.UnitTest
{
	public class UserControllerTests
	{
		[Fact]
		public async Task AddUser_ReturnsAValidUser_WhenModelStateIsValid()
		{
			using var db = new TestDbContext();
			// Arrange
			User user = new()
			{
				Firstname = "glop",
				Lastname = "plop"
			};
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

		[Theory]
		[InlineData("glop", null, "Lastname")]
		[InlineData(null, "dzadza", "Firstname")]
		[InlineData("^^@", "dzadza", "Firstname")]
		public async Task AddUser_ReturnsBadRequestResult_WhenModelStateIsInvalid(string firstName, string lastName, string errorMsg)
		{
			using var db = new TestDbContext();
			// Arrange
			User user = new()
			{
				Firstname = firstName,
				Lastname = lastName
			};
			UserController userController = new(db.UserContext);
			userController.ModelState.AddModelError(errorMsg, "invalid field");
			//Act
			var error = await userController.AddUser(user);
			//Assert
			Assert.IsType<BadRequestObjectResult>((BadRequestObjectResult)error);
			Assert.Contains(errorMsg, ((SerializableError)((BadRequestObjectResult)error).Value!).ElementAt(0).Key);
		}

		[Fact]
		public async Task GetUser_ReturnsAValidUser_WhenUserExists()
		{
			using var db = new TestDbContext();
			// Arrange
			await UserHelper.AddUser(db, "greg", "lap");
			UserController userController = new(db.UserContext);

			//Act
			var getUserResult = await userController.GetUser(1);

			//Assert
			Assert.NotNull(getUserResult);
			Assert.IsType<OkObjectResult>(getUserResult);
			Assert.IsType<User>((User)((OkObjectResult)getUserResult!).Value!);
			Assert.Equal("greg", ((User)((OkObjectResult)getUserResult!).Value!).Firstname);
			Assert.Equal("lap", ((User)((OkObjectResult)getUserResult!).Value!).Lastname);
		}

		[Fact]
		public async Task GetUser_ReturnsBadRequestResult_WhenUserDoesNotExist()
		{
			using var db = new TestDbContext();
			// Arrange
			UserController userController = new(db.UserContext);

			//Act
			var getUserResult = await userController.GetUser(1);

			//Assert
			Assert.NotNull(getUserResult);
			Assert.IsType<NotFoundResult>(getUserResult);
		}

		[Fact]
		public async Task GetUsers_ReturnsValidUsers_WhenDbNotEmpty()
		{
			// Arrange
			using var db = new TestDbContext();
			await UserHelper.AddUser(db, "greg", "lap");
			await UserHelper.AddUser(db, "gredzag", "lap");
			await UserHelper.AddUser(db, "gredzag", "ladzdap");
			await UserHelper.AddUser(db, "gred87498zag", "ladzdap");
			await UserHelper.AddUser(db, "dza", "pop");
			UserController userController = new(db.UserContext);

			//Act
			var getUserResult = await userController.GetUsers();

			//Assert
			Assert.NotNull(getUserResult);
			Assert.IsType<OkObjectResult>(getUserResult);
			Assert.IsType<List<User>>((List<User>)((OkObjectResult)getUserResult!).Value!);
			List<User> result = (List<User>)((OkObjectResult)getUserResult!).Value!;
			Assert.Equal(5, result.Count);
		}

		[Fact]
		public void DeleteUser_ForInvalidId_ReturnsNotFound()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void DeleteUser_ForValidId_ReturnsSuccess()
		{
			throw new NotImplementedException();
		}
	}
}

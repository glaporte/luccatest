using ExpenseManagement.DataModel.Entity;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.UnitTest
{
	public class UserTests
	{
		[Fact]
		public void CheckUser_ModelProperties_Validity()
		{
			// test 1 invalid
			User user = new();
			ValidationContext dbContext = new(user);
			ICollection<ValidationResult> results = new List<ValidationResult>();

			Validator.TryValidateObject(user, dbContext, results, validateAllProperties: true);

			Assert.Equal(2, results.Count);
			Assert.Contains("Lastname", results.ElementAt(0).ErrorMessage);
			Assert.Contains("Firstname", results.ElementAt(1).ErrorMessage);

			// test 2 invalid
			user = new User() { Firstname =  string.Empty, Lastname = string.Empty };
			dbContext = new ValidationContext(user);
			results = new List<ValidationResult>();

			Validator.TryValidateObject(user, dbContext, results, validateAllProperties: true);

			Assert.Equal(2, results.Count);
			Assert.Contains("Lastname", results.ElementAt(0).ErrorMessage);
			Assert.Contains("Firstname", results.ElementAt(1).ErrorMessage);


			// test 2 invalid
			user = new User
			{
				Firstname = "tto"
			};
			dbContext = new ValidationContext(user);
			results = new List<ValidationResult>();

			Validator.TryValidateObject(user, dbContext, results, validateAllProperties: true);

			Assert.Equal(1, results.Count);
			Assert.Contains("Lastname", results.ElementAt(0).ErrorMessage);
			Assert.DoesNotContain("Firstname", results.ElementAt(0).ErrorMessage);

			// test 3 invalid
			user = new User
			{
				Firstname = "tt^@@o",
				Lastname = "plop"
			};
			dbContext = new ValidationContext(user);
			results = new List<ValidationResult>();

			Validator.TryValidateObject(user, dbContext, results, validateAllProperties: true);

			Assert.Equal(1, results.Count);
			Assert.Contains("Use \"valid latin naming\" convention:", results.ElementAt(0).ErrorMessage);
			Assert.Contains(User.NamingValidity, results.ElementAt(0).ErrorMessage);

			// test 4 valid
			user = new User
			{
				Firstname = "tto",
				Lastname = "plop"
			};
			dbContext = new ValidationContext(user);
			results = new List<ValidationResult>();

			Validator.TryValidateObject(user, dbContext, results, validateAllProperties: true);

			Assert.Equal(0, results.Count);
		}
	}
}
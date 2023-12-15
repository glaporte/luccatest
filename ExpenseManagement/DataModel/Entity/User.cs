using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.DataModel.Entity
{
	public class User
	{
		public const string NamingValidity = "^[a-zA-Z0-9-_ ]+$";

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[RegularExpression(NamingValidity, ErrorMessage = $"Use \"valid latin naming\" convention: {NamingValidity}.")]
		public string Lastname { get; set; }

		[Required]
		[RegularExpression(NamingValidity, ErrorMessage = $"Use \"valid latin naming\" convention: {NamingValidity}.")]
		public string Surname { get; set; }

		public Currency PreferredCurrency { get; set; }

		public ICollection<Expense> Expenses { get; }

		public User(string lastName, string surname)
		{
			Lastname = lastName;
			Surname = surname;
			Expenses = Array.Empty<Expense>();
		}
	}
}

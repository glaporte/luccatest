using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.DataModel.Entity
{
	[Table(nameof(User))]
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
		public string Firstname { get; set; }

		public Currency PreferredCurrency { get; set; }

		public ICollection<Expense> Expenses { get; } = new List<Expense>();

		public User(string lastName, string firstName)
		{
			Lastname = lastName;
			Firstname = firstName;
		}

		public User(): this(string.Empty, string.Empty)
		{
		}
	}
}

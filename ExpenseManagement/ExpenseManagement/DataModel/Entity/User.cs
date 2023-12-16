using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ExpenseManagement.DataModel.Entity
{
	[Table(nameof(User))]
	public class User
	{
		private ILazyLoader? LazyLoader { get; set; }
		private ICollection<Expense>? _expenses;

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

		[JsonIgnore]
		public ICollection<Expense>? Expenses
		{
			get => LazyLoader.Load(this, ref _expenses)!;
			set => _expenses = value;
		}

		public User(string lastName, string firstName)
		{
			Lastname = lastName;
			Firstname = firstName;
		}

		public User() : this(string.Empty, string.Empty)
		{
		}

		private User(ILazyLoader lazyLoader) : this()
		{
			LazyLoader = lazyLoader;
		}
	}
}

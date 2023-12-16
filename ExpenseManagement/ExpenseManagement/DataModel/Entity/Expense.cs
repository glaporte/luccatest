using ExpenseManagement.DataModel.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ExpenseManagement.DataModel.Entity
{
	[Table(nameof(Expense))]

	public class Expense : IValidatableObject
	{
		private User? _user;
		private ILazyLoader? LazyLoader { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[ForeignKey("User")]
		public int UserId { get; set; }

		[JsonIgnore]
		public User? User
		{
			get => LazyLoader.Load(this, ref _user)!;
			set => _user = value;
		}

		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime Date { get; set; }

		public ExpenseType ExpenseType { get; set; }

		[DataType(DataType.Currency)]
		[Column(TypeName = "decimal(18, 2)")]
		public float Amount { get; set; }

		public Currency Currency { get; set; }
		[Required]
		public string? Annotation { get; set; }

		public string Username => $"{User?.Firstname} {User?.Lastname}";

		public Expense(User user, string annotation)
		{
			User = user;
			Annotation = annotation;
		}

		public Expense()
		{ }

		private Expense(ILazyLoader lazyLoader)
		{
			LazyLoader = lazyLoader;
		}


		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var user = validationContext.GetRequiredService<UserContext>();
			
			if (!user.UserExist(UserId))
			{
				yield return new ValidationResult($"Expense user '{UserId}' does not exist.", new[] { nameof(UserId) });
			}

			if (Date > DateTime.Now)
			{
				yield return new ValidationResult($"Expense date cannot be in the future '{Date}' > '{DateTime.Now}'.", new[] { nameof(Date) });
			}

			if (Date < DateTime.Now.AddMonths(-3))
			{
				yield return new ValidationResult($"Expense date cannot be more than 3 months old '{Date}'.", new[] { nameof(Date) });
			}

			if (User != null && User.PreferredCurrency != Currency)
			{
				yield return new ValidationResult($"Expense currency must be the same as User currency '{Currency}' != '{User.PreferredCurrency}'.", new[] { nameof(Currency) });
			}
		}
	}
}

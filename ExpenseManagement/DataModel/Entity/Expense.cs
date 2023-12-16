using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.DataModel.Entity
{

	[Table(nameof(Expense))]

	public class Expense : IValidatableObject
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int UserId { get; set; }

		[Required]
		public User? User { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime Date { get; set; }

		public ExpenseType ExpenseType { get; set; }

		[DataType(DataType.Currency)]
		[Column(TypeName = "decimal(18, 2)")]
		public float Amount { get; set; }

		public Currency Currency { get; set; }
		[Required]
		public string? Annotation { get; set; }

		public Expense(User user, string annotation)
		{
			User = user;
			Annotation = annotation;
		}

		public Expense()
		{
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (Date > DateTime.Now)
			{
				yield return new ValidationResult($"Expense date cannot be int the future '{Date}' > '{DateTime.Now}'.", new[] { nameof(Date) });
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

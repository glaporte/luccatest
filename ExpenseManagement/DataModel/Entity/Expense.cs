using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.DataModel.Entity
{
	public class Expense
	{
		public int UserId { get; set; }

		public User User { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime Date { get; set; }

		public ExpenseType ExpenseType { get; set; }

		[DataType(DataType.Currency)]
		[Column(TypeName = "decimal(18, 2)")]
		public float Amount { get; set; }

		public Currency Currency { get; set; }
		[Required]
		public string Annotation { get; set; }

		public Expense(User user, string annotation)
		{
			User = user;
			Annotation = annotation;
		}
	}
}

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ExpenseManagement.DataModel
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum Currency
	{
		[EnumMember(Value = "EUR")]
		EUR,
		[EnumMember(Value = "USD")]
		USD,
		[EnumMember(Value = "RUB")]
		RUB
	}
}

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ExpenseManagement.DataModel
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum ExpenseType
	{
		[EnumMember(Value = "Restaurant")]
		Restaurant,
		[EnumMember(Value = "Hotel")]
		Hotel,
		[EnumMember(Value = "Misc")]
		Misc
	}
}

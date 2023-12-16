namespace ExpenseManagement.DataModel.SeedData
{
	public static class SeedData
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			SeedExpenseData.Initialize(serviceProvider);
			SeedUserData.Initialize(serviceProvider);
		}
	}
}

using ExpenseManagement.DataModel.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.UnitTest
{
	public class TestDbContext : IDisposable
	{
		private bool _isDisposed;
		public UserContext UserContext { get; private set; }
		public ExpenseContext ExpenseContext { get; private set; }

		public TestDbContext()
		{
			UserContext = new UserContext(new DbContextOptionsBuilder<UserContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			   .Options);

			ExpenseContext = new ExpenseContext(new DbContextOptionsBuilder<ExpenseContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			   .Options);
		}

		// The bulk of the clean-up code is implemented in Dispose(bool)
		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed)
			{
				return;
			}

			if (disposing)
			{
				UserContext.Dispose();
				ExpenseContext.Dispose();
			}

			_isDisposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}

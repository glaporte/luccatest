using ExpenseManagement.DataModel.Context;
using ExpenseManagement.DataModel.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ExpenseManagement.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly UserContext _userContext;

		public UserController(UserContext context)
		{
			_userContext = context ?? throw new ArgumentNullException(nameof(context));
		}

		[HttpGet]
		[Route("all")]
		[Produces(typeof(IEnumerable<User>))]
		public async Task<OkObjectResult> All()
		{
			var users = await _userContext.Users.ToListAsync();

			return Ok(users);
		}
	}
}

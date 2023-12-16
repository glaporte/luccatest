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
		public async Task<IActionResult> GetUsers()
		{
			var users = await _userContext.Users.ToListAsync();

			return Ok(users);
		}

		[HttpGet]
		[Route("id")]
		[Produces(typeof(User))]
		public async Task<IActionResult> GetUser(int id)
		{
			User? user = await _userContext.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			return Ok(user);
		}

		[HttpPost]
		[Route("add")]
		[Produces(typeof(User))]
		public async Task<IActionResult> AddUser(User user)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_userContext.Users.Add(user);
			await _userContext.SaveChangesAsync();

			return Ok(user);
		}


		[HttpDelete]
		[Route("del")]
		[Produces(typeof(User))]
		public async Task<IActionResult> DeleteUser(int id)
		{
			User? user = await _userContext.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			_userContext.Users.Remove(user);
			await _userContext.SaveChangesAsync();

			return Ok(user);
		}
	}
}

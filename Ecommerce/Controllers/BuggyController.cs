using Ecommerce.APIs.Errors;
using Ecommerce.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.APIs.Controllers
{
	
	public class BuggyController : BaseApiController
	{
		private readonly StoreDbContext context;

		public BuggyController( StoreDbContext context )
		{
			this.context = context;
		}

		[HttpGet("notfound")] // GET : /api/Buggy/notfound
		public ActionResult GetNotFound()
		{
			var res = context.Products.Find(1000);
			if (res == null)
				return NotFound(new ApiResponse(404 ));
		
			return Ok(res);
		}
		[HttpGet("servererror")] // GET : /api/Buggy/servererror
		public ActionResult GetServerError()
		{
			var res = context.Products.Find(2000);
			var p = res.ToString(); // will throw an exception of [nullrefernenceException]
			return Ok(p);

		}
		[HttpGet("badrequest")]
		public ActionResult GetBadRequest()
		{
			return BadRequest(new ApiResponse(400));
		}

		[HttpGet("badrequest/{id}")]
		public ActionResult GetBadRequest(int? id) // Validation Error => Error happens because of the ModelState
		{
			return Ok(); 
		}

		[HttpGet("unauthorized")]
		public ActionResult GetUnAuthorizerError()
		{
			return Unauthorized(new ApiResponse(401));
		}
	}
}

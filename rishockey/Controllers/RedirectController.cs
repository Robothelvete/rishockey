using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace rishockey.Controllers
{
	[ApiController]
	public class RedirectController : ControllerBase
	{
		[Route("")]
		public async Task<ActionResult> Get()
		{
			return this.RedirectPermanent("https://www.reddit.com/r/Ishockey/");
		}
	}
}

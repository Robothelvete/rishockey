using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace rishockey.App_Start
{
	public class RedirectController : ApiController
	{
		[Route("")]
		public HttpResponseMessage Get() {
			var response = Request.CreateResponse(HttpStatusCode.MovedPermanently);
			response.Headers.Location = new Uri("https://www.reddit.com/r/Ishockey/");
			return response;
		}
	}
}

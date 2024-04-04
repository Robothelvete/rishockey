using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SweHockey;
using System.Text;

namespace rishockey.Controllers
{
	[Route("ical/")]
	[ApiController]
	public class IcalController : ControllerBase
	{
		[HttpGet("{tabellId}/{teamName}")]
		public async Task<ActionResult> GetByTeam(int tabellId, string teamName)
		{
			var html = await ScheduleParser.FetchScheduleHtml("https://stats.swehockey.se/ScheduleAndResults/Schedule/" + tabellId);
			var games = ScheduleParser.GamesScheduleFromHtml(html, ParserMode.Slutspel).Where(g => g.Lag.Contains(teamName));

			return new ContentResult
			{
				Content = ScheduleParser.OutputToIcal(games),
				ContentType = "text/calendar; charset=utf-8",
				
			};
		}

		[HttpGet("{tabellId}")]
		public async Task<ActionResult> GetByTabell(int tabellId)
		{
			var html = await ScheduleParser.FetchScheduleHtml("https://stats.swehockey.se/ScheduleAndResults/Schedule/" + tabellId);
			var games = ScheduleParser.GamesScheduleFromHtml(html, ParserMode.Slutspel);

			return new ContentResult
			{
				Content = ScheduleParser.OutputToIcal(games),
				ContentType = "text/calendar; charset=utf-8",

			};
		}
	}
}

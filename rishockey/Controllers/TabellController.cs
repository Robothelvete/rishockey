using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SweHockey;
using System.Net;
using System.Text;

namespace rishockey.Controllers
{
	[ApiController]
	public class TabellController : ControllerBase
	{
		[Route("domare/tabell/{tableName}"), HttpGet]
		public async Task<ActionResult> getTabellMarkdown(string tableName)
		{
			try
			{
				var standings = StandingsParser.StandingsFromHtml(StandingsParser.FetchStandingsHtml(ParserServices.mapLeagueToSifID[tableName]));
				StringBuilder sb = new StringBuilder();

				sb.AppendLine("|Rank|Lag|GP|Poäng|Poängsnitt|");
				sb.AppendLine("|:-|:-|:-|:-|:-|");
				foreach (var row in standings)
				{
					sb.AppendFormat("{0}|{1}|{2}|{3}|{4}|", row.Rank, row.Team, row.GamesPlayed, row.Points, row.pointsAverage.ToString("f2", System.Globalization.CultureInfo.InvariantCulture));
					sb.AppendLine();
				}

				return new ContentResult
				{
					Content = sb.ToString(),
					ContentType = "text/plain; charset=utf-8"
				};
			} catch (Exception ex)
			{
				return this.Problem("Sorry, this doesn't work");
			}
		}

		[Route("domare/tabell/"), HttpGet]
		public async Task<ActionResult> listTabeller()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<html><head><title>Tabeller</title></head><body><ul>");
			foreach (var league in ParserServices.mapLeagueToSifID.Keys)
			{
				sb.AppendFormat("<li><a href=\"/domare/tabell/{0}\">{0}</a></li>", league);
			}
			sb.AppendLine("</ul></body></html>");
			return new ContentResult
			{
				Content = sb.ToString(),
				ContentType = "text/html; charset=utf-8"
			};
		}

	}
}

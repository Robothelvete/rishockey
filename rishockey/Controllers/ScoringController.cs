using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SweHockey;
using System.Net;
using System.Text;

namespace rishockey.Controllers
{
	[ApiController]
	public class ScoringController : ControllerBase
	{
		[Route("domare/poang/{tableName}"), HttpGet]
		public async Task<ActionResult> getPoängMarkdown(string tableName)
		{
			var scoringStats = ScoringParser.ScoringLeadersFromHtml(ScoringParser.FetchScoringHtml(ParserServices.mapLeagueToSifID[tableName]));
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("|Lag|Namn|GP|G|A|P|Avg|");
			sb.AppendLine("|:-|:-|:-|:-|:-|:-|:-|");
			foreach (var stat in scoringStats)
			{
				sb.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}|", stat.Team, stat.Name, stat.GamesPlayed, stat.Goals, stat.Assists, stat.Points, stat.AveragePoints.ToString("f2", System.Globalization.CultureInfo.InvariantCulture));
				sb.AppendLine();
			}

			return new ContentResult
			{
				Content = sb.ToString(),
				ContentType = "text/plain; charset=utf-8"
			};
		}

		[Route("domare/poang/"), HttpGet]
		public async Task<ActionResult> listPoangTabeller()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<html><head><title>Poängledare</title></head><body><ul>");
			foreach (var league in ParserServices.mapLeagueToSifID.Keys)
			{
				sb.AppendFormat("<li><a href=\"/domare/poang/{0}\">{0}</a></li>", league);
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

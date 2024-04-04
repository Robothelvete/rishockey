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
		public HttpResponseMessage getTabellMarkdown(string tableName)
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

				return new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StringContent(sb.ToString(), Encoding.UTF8, "text/plain")
				};
			} catch (Exception ex)
			{
				return new HttpResponseMessage(HttpStatusCode.NotImplemented)
				{ //can happen .e.g. when trying to view a table of playoffs
					Content = new StringContent("Sorry, this doesn't work", Encoding.UTF8, "text/plain")
				};
			}
		}

		[Route("domare/tabell/"), HttpGet]
		public HttpResponseMessage listTabeller()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<html><head><title>Tabeller</title></head><body><ul>");
			foreach (var league in ParserServices.mapLeagueToSifID.Keys)
			{
				sb.AppendFormat("<li><a href=\"/domare/tabell/{0}\">{0}</a></li>", league);
			}
			sb.AppendLine("</ul></body></html>");
			return new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(sb.ToString(), Encoding.UTF8, "text/html")
			};
		}

	}
}

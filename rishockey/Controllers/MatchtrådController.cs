using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using SweHockey;

namespace rishockey.Controllers
{
	[ApiController]
	public class MatchtrådController : ControllerBase
	{
		[Route("domare/matchtrad/"), HttpGet]
		public async Task<ActionResult> getCurrentMatchtrad()
		{
			var leagueGames = ScheduleParser.GamesScheduleFromDailyHtml(await ScheduleParser.FetchScheduleHtml());
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("##DAGENS MATCHER");

			foreach (LeagueGames league in leagueGames.OrderBy(l => sortOrderByLeage(l.League)))
			{
				sb.AppendFormat("**{0}**\n\n", league.League);
				sb.AppendLine("|Lag | Tid|");
				sb.AppendLine("|:-|:-|");
				foreach (Game game in league.Games)
				{
					sb.AppendFormat("|{0} | {1}|\n", game.Lag.Trim(), game.Tid.ToString("HH:mm"));
				}
				sb.AppendLine("\n___");
			}

			return new ContentResult
			{
				Content = sb.ToString(),
				ContentType = "text/plain; charset=utf-8"
			};
		}


		[Route("domare/eftermatchtrad/"), HttpGet]
		public async Task<ActionResult> getCurrentEfterMatchtrad()
		{
			var leagueGames = ResultsParser.GamesResultsFromHtml(ResultsParser.FetchResultsHtml());
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("##DAGENS RESULTAT");

			foreach (LeagueGames league in leagueGames.OrderBy(l => sortOrderByLeage(l.League)))
			{
				sb.AppendFormat("**{0}**\n\n", league.League);
				sb.AppendLine("|Lag|Resultat|");
				sb.AppendLine("|:-|:-|");
				foreach (Game game in league.Games)
				{
					sb.AppendFormat("|{0} | {1}|\n", game.Lag.Trim(), game.Results);
				}
				sb.AppendLine("\n___");
			}

			return new ContentResult
			{
				Content = sb.ToString(),
				ContentType = "text/plain; charset=utf-8"
			};
		}

		private int sortOrderByLeage(string league)
		{
			string[] order = new string[] {"SHL",
				"HockeyAllsvenskan",
				"SDHL",
				"AllEttan Norra",
				"AllEttan Södra",
				"ATG Hockeyettan Norra",
				"ATG Hockeyettan Västra",
				"ATG Hockeyettan Östra",
				"ATG Hockeyettan Södra",
				"ATG Hockeyettan Norra vår",
				"ATG Hockeyettan Västra vår",
				"ATG Hockeyettan Östra vår",
				"ATG HockeyEttan Södra vår",
				"J20 SuperElit Top 10",
				"J20 - Nationell Norra",
				"J20 - Nationell Södra",
				"J20 SuperElit Forts." };
			if (order.Contains(league))
			{
				return Array.IndexOf(order, league);
			} else
			{
				return order.Length + 2;
			}
		}
	}
}

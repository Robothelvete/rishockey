using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SweHockey;
using System.Text;

namespace rishockey.App_Start
{
	public class MatchtrådController : ApiController
	{
		[Route("domare/matchtrad/"), HttpGet]
		public HttpResponseMessage getCurrentMatchtrad() {
			var leagueGames = ScheduleParser.GamesScheduleFromDailyHtml(ScheduleParser.FetchScheduleHtml());
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("##DAGENS MATCHER");

			foreach (LeagueGames league in leagueGames.OrderBy(l => sortOrderByLeage(l.League))) {
				sb.AppendFormat("**{0}**\n\n", league.League);
				sb.AppendLine("|Lag | Tid|");
				sb.AppendLine("|:-|:-|");
				foreach(Game game in league.Games) {
					sb.AppendFormat("|{0} | {1}|\n",game.Lag.Trim(), game.Tid.ToString("HH:mm"));
				}
				sb.AppendLine("\n___");
			}

			return new HttpResponseMessage(HttpStatusCode.OK) {
				Content = new StringContent(sb.ToString(), Encoding.UTF8, "text/plain")
			};
		}


		[Route("domare/eftermatchtrad/"), HttpGet]
		public HttpResponseMessage getCurrentEfterMatchtrad() {
			var leagueGames = ResultsParser.GamesResultsFromHtml(ResultsParser.FetchResultsHtml());
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("##DAGENS RESULTAT");

			foreach (LeagueGames league in leagueGames.OrderBy(l => sortOrderByLeage(l.League))) {
				sb.AppendFormat("**{0}**\n\n", league.League);
				sb.AppendLine("|Lag|Resultat|");
				sb.AppendLine("|:-|:-|");
				foreach (Game game in league.Games) {
					sb.AppendFormat("|{0} | {1}|\n", game.Lag.Trim(), game.Results);
				}
				sb.AppendLine("\n___");
			}

			return new HttpResponseMessage(HttpStatusCode.OK) {
				Content = new StringContent(sb.ToString(), Encoding.UTF8, "text/plain")
			};
		}

		private int sortOrderByLeage(string league) {
			string[] order = new string[] {"SHL",
				"HockeyAllsvenskan",
				"SDHL",
				"ATG Allettan Norra",
				"ATG Allettan Södra",
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
			if(order.Contains(league)) {
				return Array.IndexOf(order, league);
			} else {
				return order.Length + 2;
			}
		}
	}
}

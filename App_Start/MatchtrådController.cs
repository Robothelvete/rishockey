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
			//TODO: stats.swehockey.se sorts the leagues terribly
			foreach (LeagueGames league in leagueGames) {
				sb.AppendFormat("**{0}**\n\n", league.League);
				sb.AppendLine("|Lag | Tid|");
				sb.AppendLine("|:-----------|------------:|");
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
			//TODO: stats.swehockey.se sorts the leagues terribly
			foreach (LeagueGames league in leagueGames) {
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
	}
}

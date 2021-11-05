using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using SweHockey;


namespace rishockey.Controllers
{
	public class ScoringController : ApiController
	{
		[Route("domare/poang/{tableName}"), HttpGet]
		public HttpResponseMessage getPoängMarkdown(string tableName) {
			var scoringStats = ScoringParser.ScoringLeadersFromHtml(ScoringParser.FetchScoringHtml(ParserServices.mapLeagueToSifID[tableName]));
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("|Lag|Namn|GP|G|A|P|Avg|");
			sb.AppendLine("|:-|:-|:-|:-|:-|:-|:-|");
			foreach (var stat in scoringStats) {
				sb.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}|", stat.Team, stat.Name, stat.GamesPlayed, stat.Goals, stat.Assists, stat.Points, stat.AveragePoints.ToString("g3", System.Globalization.CultureInfo.InvariantCulture));
				sb.AppendLine();
			}

			return new HttpResponseMessage(HttpStatusCode.OK) {
				Content = new StringContent(sb.ToString(), Encoding.UTF8, "text/plain")
			};
		}

		[Route("domare/poang/"), HttpGet]
		public HttpResponseMessage listPoangTabeller() {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<html><head><title>Poängledare</title></head><body><ul>");
			foreach (var league in ParserServices.mapLeagueToSifID.Keys) {
				sb.AppendFormat("<li><a href=\"/domare/poang/{0}\">{0}</a></li>", league);
			}
			sb.AppendLine("</ul></body></html>");
			return new HttpResponseMessage(HttpStatusCode.OK) {
				Content = new StringContent(sb.ToString(), Encoding.UTF8, "text/html")
			};
		}
	}
}

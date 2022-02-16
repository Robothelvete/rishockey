using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace rishockey.Controllers
{
	public class FilteredRSSController : ApiController
	{
		[Route("transfers")]

		public HttpResponseMessage Get() {
			//fetch https://www.eliteprospects.com/rss_confirmed-transfers.php
			XmlReader reader = XmlReader.Create("https://www.eliteprospects.com/rss_confirmed-transfers.php");
			XmlDocument doc = new XmlDocument();
			doc.Load(reader);
			reader.Close();
			var items = doc.DocumentElement.SelectNodes("/rss/channel/item");
			foreach (XmlNode item in items) {
				//filter it for SHL/HA/HE
				if (!InvolvesSwedishTeam(item.SelectSingleNode("description").InnerText)) {
					item.ParentNode.RemoveChild(item);
				} else {
					var pubDate = item.SelectSingleNode("pubDate");
					var origDate = DateTime.ParseExact(pubDate.InnerText, "ddd, d MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
					var guid = item.SelectSingleNode("guid").InnerText;
					pubDate.InnerText = origDate.AddSeconds(int.Parse(guid.Replace("https://eliteprospects.com/t/", "")) % 86400).ToString("R");
				}
			}

			return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(doc.OuterXml, System.Text.Encoding.UTF8, "application/rss+xml") };

		}


		bool InvolvesSwedishTeam(string description) {
			foreach (string team in SHLTeams.Keys) {
				if (description.Contains(team) && description.Contains("https://www.eliteprospects.com" + team)) { return true; }
			}
			foreach (string team in AllsvenskanTeams.Keys) {
				if (description.Contains(team) && description.Contains("https://www.eliteprospects.com" + team)) { return true; }
			}
			foreach (string team in HockeyEttanTeams.Keys) {
				if (description.Contains(team) && description.Contains("https://www.eliteprospects.com" + team)) { return true; }
			}
			foreach (string team in SDHLTeams.Keys) {
				if (description.Contains(team) && description.Contains("https://www.eliteprospects.com" + team)) { return true; }
			}
			return false;
		}
		static Dictionary<string, string> SHLTeams = new Dictionary<string, string>() {
			{ "/team/2/", "Brynäs" },
			{ "/team/3/", "Djurgården" },
			{"/team/12/", "Frölunda" },
			{"/team/4/", "Färjestad" },
			{"/team/5/", "HV71" },
			{"/team/31/", "Oskarshamn" },
			{"/team/28/", "Leksand" },
			{"/team/6/", "Linköping" },
			{"/team/7/", "Luleå" },
			{"/team/8/", "Malmö" },
			{"/team/32/", "Rögle" },
			{"/team/22/", "Skelleftå" },
			{"/team/339/", "Växjö" },
			{"/team/36/", "Örebro" }
		};
		static Dictionary<string, string> AllsvenskanTeams = new Dictionary<string, string>() {
			{ "/team/1/", "AIK" },
			{ "/team/13/", "Almtuna" },
			{"/team/25/", "Karlskoga" },
			{"/team/322/", "Vita hästen" },
			{"/team/15/", "Björklöven" },
			{"/team/509/", "Karlskrona" },
			{"/team/333/", "Kristianstad" },
			{"/team/9/", "MOOD" },
			{"/team/29/", "Mora" },
			{"/team/10/", "Södertälje" },
			{"/team/11/", "Timrå" },
			{"/team/33/", "Tyngsryd" },
			{"/team/500/", "Västervik" },
			{"/team/308/", "Västerås" }
		};

		static Dictionary<string, string> HockeyEttanTeams = new Dictionary<string, string>() {
			{"/team/1596/","Bodens HF"},
			{"/team/301/","Borlänge HF"},
			{"/team/320/","Borås HC"},
			{"/team/946/","Enköpings SK"},
			{"/team/645/","Forshaga IF"},
			{"/team/311/","Grums IK"},
			{"/team/1594/","Halmstad Hammers HC"},
			{"/team/15147/","Hammarby IF"},
			{"/team/481/","Hanhals IF"},
			{"/team/330/","HC Dalen"},
			{"/team/18/","Huddinge IK"},
			{"/team/304/","Hudiksvalls HC"},
			{"/team/23/","IF Sundsvall Hockey"},
			{"/team/35/","IF Troja-Ljungby"},
			{"/team/2654/","Kalix HC"},
			{"/team/1040/","Kallinge/Ronneby IF"},
			{"/team/778/","Kalmar HC"},
			{"/team/910/","Kiruna AIF"},
			{"/team/19/","Kiruna IF"},
			{"/team/315/","Kumla HC"},
			{"/team/807/","Köping HC"},
			{"/team/295/","Linden Hockey"},
			{"/team/486/","Lindlövens IF"},
			{"/team/360/","Malungs IF"},
			{"/team/323/","Mariestad BoIS"},
			{"/team/30/","Mörrums GoIS"},
			{"/team/335/","Nybro IF"},
			{"/team/4095/","Nyköpings SK"},
			{"/team/21/","Piteå HC"},
			{"/team/367/","Segeltorps IF"},
			{"/team/271/","SK Lejon"},
			{"/team/328/","Skövde IK"},
			{"/team/494/","Sollentuna HC"},
			{"/team/371/","Strömsbro IF"},
			{"/team/306/","Surahammars IF"},
			{"/team/272/","Tegs SK"},
			{"/team/34/","Tranås AIF"},
			{"/team/1291/","Tyresö/Hanviken"},
			{"/team/338/","Tyringe SoSS"},
			{"/team/291/","Vallentuna BK"},
			{"/team/498/","Vimmerby HC"},
			{"/team/1226/","Visby/Roma"},
			{"/team/273/","Vännäs HC"},
			{"/team/290/","Väsby IK"},
			{"/team/285/","Wings HC"},
			{"/team/6879/","Örnsköldsvik HF"},
			{"/team/1595/","Östersunds IK"}
		};

		static Dictionary<string, string> SDHLTeams = new Dictionary<string, string> {
			{"/team/19175/","AIK"},
			{"/team/19174/","Brynäs"},
			{"/team/19176/","Djurgården"},
			{"/team/19194/","Göteborg"},
			{"/team/19177/","HV71"},
			{"/team/19179/","Leksand"},
			{"/team/19180/","Linköping"},
			{"/team/19181/","Luleå"},
			{"/team/19182/","MODO"},
			{"/team/19183/","SDE"}
		};
	}
}

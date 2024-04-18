using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml;

namespace rishockey.Controllers
{
	[ApiController]
	public class FilteredRSSController : ControllerBase
	{
		[Route("transfers")]

		public async Task<ActionResult> Get()
		{
			//fetch https://www.eliteprospects.com/rss_confirmed-transfers.php
			XmlReader reader = XmlReader.Create("https://www.eliteprospects.com/rss_confirmed-transfers.php");
			XmlDocument doc = new XmlDocument();
			doc.Load(reader);
			reader.Close();
			var items = doc.DocumentElement.SelectNodes("/rss/channel/item");
			foreach (XmlNode item in items)
			{
				//filter it for SHL/HA/HE
				if (!InvolvesSwedishTeam(item.SelectSingleNode("description").InnerText))
				{
					item.ParentNode.RemoveChild(item);
				} else
				{
					var pubDate = item.SelectSingleNode("pubDate");
					var origDate = DateTime.ParseExact(pubDate.InnerText, "ddd, d MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
					var guid = item.SelectSingleNode("guid").InnerText;
					pubDate.InnerText = origDate.AddSeconds(int.Parse(guid.Replace("https://eliteprospects.com/t/", "")) % 86400).ToString("R");
				}
			}
			return new ContentResult
			{
				Content = doc.OuterXml,
				ContentType = "application/rss+xml; charset=utf-8"
			};


		}


		bool InvolvesSwedishTeam(string description)
		{
			foreach (string team in SHLTeams.Keys)
			{
				if (description.Contains(team) && description.Contains("https://www.eliteprospects.com" + team)) { return true; }
			}
			foreach (string team in AllsvenskanTeams.Keys)
			{
				if (description.Contains(team) && description.Contains("https://www.eliteprospects.com" + team)) { return true; }
			}
			foreach (string team in HockeyEttanTeams.Keys)
			{
				if (description.Contains(team) && description.Contains("https://www.eliteprospects.com" + team)) { return true; }
			}
			foreach (string team in SDHLTeams.Keys)
			{
				if (description.Contains(team) && description.Contains("https://www.eliteprospects.com" + team)) { return true; }
			}
			return false;
		}
		static Dictionary<string, string> SHLTeams = new Dictionary<string, string>() {
			{"/team/12/", "Frölunda" },
			{"/team/4/", "Färjestad" },
			{"/team/5/", "HV71" },
			{"/team/28/", "Leksand" },
			{"/team/6/", "Linköping" },
			{"/team/7/", "Luleå" },
			{"/team/8/", "Malmö" },
			{"/team/9/", "MOOD" },
			{"/team/32/", "Rögle" },
			{"/team/22/", "Skelleftå" },
			{"/team/11/", "Timrå" },
			{"/team/339/", "Växjö" },
			{"/team/36/", "Örebro" }
		};
		static Dictionary<string, string> AllsvenskanTeams = new Dictionary<string, string>() {
			{ "/team/1/", "AIK" },
			{ "/team/13/", "Almtuna" },
			{"/team/15/", "Björklöven" },
			{ "/team/2/", "Brynäs" },
			{ "/team/3/", "Djurgården" },
			{"/team/31/", "Oskarshamn" },
			{"/team/25/", "Karlskoga" },
			{"/team/322/", "Vita hästen" },
			{"/team/778/","Kalmar HC"},
			{"/team/29/", "Mora" },
			{"/team/335/","Nybro IF"},
			{"/team/10/", "Södertälje" },
			{"/team/33/", "Tingsryd" },
			{"/team/308/", "Västerås" },
			{"/team/1595/","Östersunds IK"}
		};

		static Dictionary<string, string> HockeyEttanTeams = new Dictionary<string, string>() {
			{"/team/603/", "Alvesta SK" },
			{"/team/1596/","Bodens HF"},
			{"/team/301/","Borlänge HF"},
			{"/team/320/","Borås HC"},
			{"/team/321/", "Bäcken HC" },
			{"/team/946/","Enköpings SK"},
			{"/team/350/", "Falu IF" },
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
			{"/team/509/", "Karlskrona" },
			{"/team/1040/","Kallinge/Ronneby IF"},
			{"/team/910/","Kiruna AIF"},
			{"/team/19/","Kiruna IF"},
			{"/team/333/", "Kristianstad" },
			{"/team/315/","Kumla HC"},
			{"/team/807/","Köping HC"},
			{"/team/295/","Linden Hockey"},
			{"/team/486/","Lindlövens IF"},
			{"/team/360/","Malungs IF"},
			{"/team/323/","Mariestad BoIS"},
			{"/team/30/","Mörrums GoIS"},
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
			{"/team/500/", "Västervik" },
			{"/team/273/","Vännäs HC"},
			{"/team/290/","Väsby IK"},
			{"/team/285/","Wings HC"},
			{"/team/6879/","Örnsköldsvik HF"}
		};

		static Dictionary<string, string> SDHLTeams = new Dictionary<string, string> {
			{"/team/19175/","AIK"},
			{"/team/19174/","Brynäs"},
			{"/team/19176/","Djurgården"},
			{"/team/34469", "Frölunda HC" },
			{"/team/19194/","Göteborg"},
			{"/team/19177/","HV71"},
			{"/team/19179/","Leksand"},
			{"/team/19180/","Linköping"},
			{"/team/19181/","Luleå"},
			{"/team/19182/","MODO"},
			{"/team/19183/","SDE"},
			{"/team/19241/", "Skellefteå AIK" }
		};
	}
}

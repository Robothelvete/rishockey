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
			//TODO:
			//fetch https://www.eliteprospects.com/rss_confirmed-transfers.php
			XmlReader reader = XmlReader.Create("https://www.eliteprospects.com/rss_confirmed-transfers.php");
			//filter it for SHL/HA/HE
			XmlDocument doc = new XmlDocument();
			doc.Load(reader);
			reader.Close();	
			var items = doc.DocumentElement.SelectNodes("/rss/channel/item");
			foreach (XmlNode item in items) {
				if (!InvolvesSwedishTeam(item.SelectSingleNode("description").InnerText)) {
					item.ParentNode.RemoveChild(item);
				}
			}

			return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(doc.OuterXml, System.Text.Encoding.UTF8, "application/rss+xml") };

		}


		bool InvolvesSwedishTeam(string description) {
			foreach (string team in SHLTeams.Keys) {
				if (description.Contains(team)) { return true; }
			}
			foreach (string team in AllsvenskanTeams.Keys) {
				if (description.Contains(team)) { return true; }
			}
			foreach (string team in HockeyEttanTeams.Keys) {
				if (description.Contains(team)) { return true; }
			}
			foreach (string team in SDHLTeams.Keys) {
				if (description.Contains(team)) { return true; }
			}
			return false;
		}

		static Dictionary<string, string> SHLTeams = new Dictionary<string, string>() {
			{ "/team.php?team=2\"", "Brynäs" },
			{ "/team.php?team=3\"", "Djurgården" },
			{"/team.php?team=12\"", "Frölunda" },
			{"/team.php?team=4\"", "Färjestad" },
			{"/team.php?team=5\"", "HV71" },
			{"/team.php?team=31\"", "Oskarshamn" },
			{"/team.php?team=28\"", "Leksand" },
			{"/team.php?team=6\"", "Linköping" },
			{"/team.php?team=7\"", "Luleå" },
			{"/team.php?team=8\"", "Malmö" },
			{"/team.php?team=32\"", "Rögle" },
			{"/team.php?team=22\"", "Skelleftå" },
			{"/team.php?team=339\"", "Växjö" },
			{"/team.php?team=36\"", "Örebro" }
		};
		static Dictionary<string, string> AllsvenskanTeams = new Dictionary<string, string>() {
			{ "/team.php?team=1\"", "AIK" },
			{ "/team.php?team=13\"", "Almtuna" },
			{"/team.php?team=25\"", "Karlskoga" },
			{"/team.php?team=322\"", "Vita hästen" },
			{"/team.php?team=15\"", "Björklöven" },
			{"/team.php?team=509\"", "Karlskrona" },
			{"/team.php?team=333\"", "Kristianstad" },
			{"/team.php?team=9\"", "MOOD" },
			{"/team.php?team=29\"", "Mora" },
			{"/team.php?team=10\"", "Södertälje" },
			{"/team.php?team=11\"", "Timrå" },
			{"/team.php?team=33\"", "Tyngsryd" },
			{"/team.php?team=500\"", "Västervik" },
			{"/team.php?team=308\"", "Västerås" }
		};

		static Dictionary<string, string> HockeyEttanTeams = new Dictionary<string, string>() {
			{"/team.php?team=1596\"","Bodens HF"},
			{"/team.php?team=301\"","Borlänge HF"},
			{"/team.php?team=320\"","Borås HC"},
			{"/team.php?team=946\"","Enköpings SK"},
			{"/team.php?team=645\"","Forshaga IF"},
			{"/team.php?team=311\"","Grums IK"},
			{"/team.php?team=1594\"","Halmstad Hammers HC"},
			{"/team.php?team=15147\"","Hammarby IF"},
			{"/team.php?team=481\"","Hanhals IF"},
			{"/team.php?team=330\"","HC Dalen"},
			{"/team.php?team=18\"","Huddinge IK"},
			{"/team.php?team=304\"","Hudiksvalls HC"},
			{"/team.php?team=23\"","IF Sundsvall Hockey"},
			{"/team.php?team=35\"","IF Troja-Ljungby"},
			{"/team.php?team=2654\"","Kalix HC"},
			{"/team.php?team=1040\"","Kallinge/Ronneby IF"},
			{"/team.php?team=778\"","Kalmar HC"},
			{"/team.php?team=910\"","Kiruna AIF"},
			{"/team.php?team=19\"","Kiruna IF"},
			{"/team.php?team=315\"","Kumla HC"},
			{"/team.php?team=807\"","Köping HC"},
			{"/team.php?team=295\"","Linden Hockey"},
			{"/team.php?team=486\"","Lindlövens IF"},
			{"/team.php?team=360\"","Malungs IF"},
			{"/team.php?team=323\"","Mariestad BoIS"},
			{"/team.php?team=30\"","Mörrums GoIS"},
			{"/team.php?team=335\"","Nybro IF"},
			{"/team.php?team=4095\"","Nyköpings SK"},
			{"/team.php?team=21\"","Piteå HC"},
			{"/team.php?team=367\"","Segeltorps IF"},
			{"/team.php?team=271\"","SK Lejon"},
			{"/team.php?team=328\"","Skövde IK"},
			{"/team.php?team=494\"","Sollentuna HC"},
			{"/team.php?team=371\"","Strömsbro IF"},
			{"/team.php?team=306\"","Surahammars IF"},
			{"/team.php?team=272\"","Tegs SK"},
			{"/team.php?team=34\"","Tranås AIF"},
			{"/team.php?team=1291\"","Tyresö/Hanviken"},
			{"/team.php?team=338\"","Tyringe SoSS"},
			{"/team.php?team=291\"","Vallentuna BK"},
			{"/team.php?team=498\"","Vimmerby HC"},
			{"/team.php?team=1226\"","Visby/Roma"},
			{"/team.php?team=273\"","Vännäs HC"},
			{"/team.php?team=290\"","Väsby IK"},
			{"/team.php?team=285\"","Wings HC"},
			{"/team.php?team=6879\"","Örnsköldsvik HF"},
			{"/team.php?team=1595\"","Östersunds IK"}
		};

		static Dictionary<string, string> SDHLTeams = new Dictionary<string, string> {
			{"/team.php?team=19175\"","AIK"},
			{"/team.php?team=19174\"","Brynäs"},
			{"/team.php?team=19176\"","Djurgården"},
			{"/team.php?team=19194\"","Göteborg"},
			{"/team.php?team=19177\"","HV71"},
			{"/team.php?team=19179\"","Leksand"},
			{"/team.php?team=19180\"","Linköping"},
			{"/team.php?team=19181\"","Luleå"},
			{"/team.php?team=19182\"","MODO"},
			{"/team.php?team=19183\"","SDE"}
		};
	}
}

using System.Text;
using System.IO;

namespace PogBot
{
	public class Global
	{
		public static Global Instance { get; private set; }

		public string token = String.Empty;
		public string gKey = String.Empty;
		public string gCx = String.Empty;
		public string pToken = String.Empty;

		public HttpClient Client { get; private set; }

		public static string discordCommand = "pog";
		public static string cleanSaveCommand = " -c";
		public static string saveFile = "src/saved_images.txt";
		public static string qOW = "overwatch";
		public static string qLOL = "league of legends";
		public static string noImageMessage = "No image found:pensive::heart:";
		public static string deletedSavesMessage = "Deleted saved images:flushed:";
		public static int queriesToChooseFrom = 20;

		public static string[] queriesOW =
		{
			"Ana",
			"Ashe",
			"Brigitte",
			"Dva",
			"Echo",
			"Mei",
			"Mercy",
			"Moira",
			"Orisa",
			"Pharah",
			"Sombra",
			"Symmetra",
			"Tracer",
			"Widowmaker",
			"Zarya",
		};
		public static string[] queriesLOL =
		{
			"Aatrox",
			"Ahri",
			"Akali",
			"Alistar",
			"Amumu",
			"Anivia",
			"Annie",
			"Ashe",
			"Azir",
			"Blitzcrank",
			"Brand",
			"Braum",
			"Caitlyn",
			"Cassiopeia",
			"Cho'Gath",
			"Corki",
			"Darius",
			"Diana",
			"Dr. Mundo",
			"Draven",
			"Elise",
			"Evelynn",
			"Ezreal",
			"Fiddlesticks",
			"Fiora",
			"Fizz",
			"Galio",
			"Gangplank",
			"Garen",
			"Gnar",
			"Gragas",
			"Graves",
			"Hecarim",
			"Heimerdinger",
			"Irelia",
			"Janna",
			"Jarvan IV",
			"Jax",
			"Jayce",
			"Jinx",
			"Kalista",
			"Karma",
			"Karthus",
			"Kassadin",
			"Katarina",
			"Kayle",
			"Kennen",
			"Kha'Zix",
			"Kog'Maw",
			"LeBlanc",
			"Lee Sin",
			"Leona",
			"Lissandra",
			"Lucian",
			"Lulu",
			"Lux",
			"Malphite",
			"Malzahar",
			"Maokai",
			"Master Yi",
			"Miss Fortune",
			"Mordekaiser",
			"Morgana",
			"Nami",
			"Nasus",
			"Nautilus",
			"Nidalee",
			"Nocturne",
			"Nunu",
			"Olaf",
			"Orianna",
			"Pantheon",
			"Poppy",
			"Quinn",
			"Rammus",
			"Rek'Sai",
			"Renekton",
			"Rengar",
			"Riven",
			"Rumble",
			"Ryze",
			"Sejuani",
			"Shaco",
			"Shen",
			"Shyvana",
			"Singed",
			"Sion",
			"Sivir",
			"Skarner",
			"Sona",
			"Soraka",
			"Swain",
			"Syndra",
			"Talon",
			"Taric",
			"Teemo",
			"Thresh",
			"Tristana",
			"Trundle",
			"Tryndamere",
			"Twisted Fate",
			"Twitch",
			"Udyr",
			"Urgot",
			"Varus",
			"Vayne",
			"Veigar",
			"Vel'Koz",
			"Vi",
			"Viktor",
			"Vladimir",
			"Volibear",
			"Warwick",
			"Wukong",
			"Xerath",
			"Xin Zhao",
			"Yasuo",
			"Yorick",
			"Zac",
			"Zed",
			"Ziggs",
			"Zilean",
			"Zyra",
			"bard",
			"ekko",
			"tahm kench",
			"kindred",
			"illaoi",
			"jhin",
			"aurelion sol",
			"taliyah",
			"kled",
			"ivern",
			"camille",
			"rakan",
			"xayah",
			"kayn",
			"ornn",
			"zoe",
			"kai'sa",
			"pyke",
			"neeko",
			"sylas",
			"yuumi",
			"qiyana",
			"senna",
			"aphelios",
			"sett",
			"lillia",
			"yone",
			"samira",
			"seraphine",
			"rell",
			"viego",
			"gwen",
			"akshan",
			"vex",
		};

		public static bool googleSearch = true;
		public static bool pinterestSearch = false;

		public static async void Setup()
		{
			if (Instance == null)
				Instance = new Global();

			var keys = await File.ReadAllLinesAsync("src/keys.txt");

			foreach (var key in keys)
			{
				var tokenQuery = "token=";
				var gKeyQuery = "googlekey=";
				var gCxQuery = "googlecx=";

				if (key.StartsWith(tokenQuery))
				{
					Instance.token = key.Substring(tokenQuery.Length);
				}
				else if (key.StartsWith(gKeyQuery))
				{
					Instance.gKey = key.Substring(gKeyQuery.Length);
				}
				else if (key.StartsWith(gCxQuery))
				{
					Instance.gCx = key.Substring(gCxQuery.Length);
				}
			}

			Global.Instance.Client = new HttpClient();
		}

		public static void Shutdown()
        {
			Global.Instance.Client.Dispose();
        }

		public static string StringBetween(string source, string start, string end)
		{
			string result = "";
			if (source.Contains(start) && source.Contains(end))
			{
				int startIndex = source.IndexOf(start, 0) + start.Length;
				int endIndex = source.IndexOf(end, startIndex);
				result = source.Substring(startIndex, endIndex - startIndex);
				return result;
			}

			return result;
		}
		public static string StringFirstToMatch(string source, string match)
		{
			string result = "";
			if (source.Contains(match))
			{
				int endIndex = source.IndexOf(match, 0);
				result = source.Substring(0, endIndex);
				return result;
			}

			return result;
		}

		public static string StringIndexToMatch(string source, int startIndex, string match)
		{
			string result = "";
			if (source.Contains(match))
			{
				int endIndex = source.IndexOf(match, startIndex);
				result = source.Substring(startIndex, endIndex - startIndex);
				return result;
			}

			return result;
		}

		public static string LoadFile(string path)
		{
			return File.ReadAllText(path);
		}

		public static bool IsInSaved(string imageURL)
		{
			if (!File.Exists(Global.saveFile))
            {
				using (File.Create(Global.saveFile)) { }
            }

			foreach (var line in ReadLines(Global.saveFile))
			{
				if (line.Equals(imageURL))
				{
					return true;
				}
			}
			return false;
		}
		public static IEnumerable<string> ReadLines(string path)
		{
			using (StreamReader reader = File.OpenText(path))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					yield return line;
				}
			}
		}

		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}

		public static string RandomSearchQuery(string[] query)
		{
			var random = new Random().Next(0, query.Length);
			return query[random];
		}
	}
}

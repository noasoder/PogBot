using System.Text;

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
		public static string q = "overwatch";
		public static string noImageMessage = "No image found:pensive::heart:";

		private static string[] queries =
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
			"Zarya"
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

		public static async Task<bool> IsInSaved(string imageURL)
		{
			var lines = await File.ReadAllLinesAsync("src/saved_images.txt");
			foreach (var line in lines)
			{
				if (line.Equals(imageURL))
				{
					return true;
				}
			}
			return false;
		}

		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}

		public static string RandomSearchQuery()
		{
			var random = new Random().Next(0, queries.Length);
			return q + " " + queries[random];
		}
	}
}

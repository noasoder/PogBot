using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

public class Global
{
	public static Global Instance { get; private set; }

	public string token = String.Empty;
	public string gKey = String.Empty;
	public string gCx = String.Empty;
	public string pToken = String.Empty;

	private HttpClient client;

	public static string discordCommand = "pog";
	public static string q = "overwatch girl";

	public static bool googleSearch = false;
	public static bool pinterestSearch = true;

	public static async void Setup()
    {
		if(Instance == null)
			Instance = new Global();

		var keys = await File.ReadAllLinesAsync("src/keys.txt");

		foreach(var key in keys)
        {
			var tokenQuery = "token=";
			var gKeyQuery = "googlekey=";
			var gCxQuery = "googlecx=";
			var pTokenQuery = "pinteresttoken=";

			if(key.StartsWith(tokenQuery))
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
			else if (key.StartsWith(pTokenQuery))
			{
				Instance.pToken = key.Substring(pTokenQuery.Length);
			}
		}
		
		Global.Instance.client = new HttpClient();
	}

	public static string StringBetween(string Source, string Start, string End)
	{
		string result = "";
		if (Source.Contains(Start) && Source.Contains(End))
		{
			int StartIndex = Source.IndexOf(Start, 0) + Start.Length;
			int EndIndex = Source.IndexOf(End, StartIndex);
			result = Source.Substring(StartIndex, EndIndex - StartIndex);
			return result;
		}

		return result;
	}

	public static string LoadFile(string path)
	{
		return File.ReadAllText(path);
	}

	public async Task<string> GetImage(string additionalSearch)
	{
		var result = "";
		if (Global.googleSearch)
        {
			var getString = "https://www.googleapis.com/customsearch/v1?key=" 
							+ Instance.gKey + "&cx=" + Instance.gCx + "&q=" + Global.q + additionalSearch;
			result = await client.GetStringAsync(getString);
			result = ParseToImageGoogle(result);
        }
		if(Global.pinterestSearch)
        {
			//var board_id = 0;
			//var getString = "https://api.pinterest.com/v5/boards/" + board_id + "";
			var getString = "https://api.pinterest.com/v5/user_account?access_token=" + Instance.pToken;
			result = await client.GetStringAsync(getString);
			Console.Write(result);
        }
        //Console.WriteLine("Result: " + result);

        //var foundUnique = false;

        //while (foundUnique)
        //{
        //    foundUnique = await IsInSaved(result);
        //    if (foundUnique)
        //    {
        //        break;
        //    }
        //    else
        //    {
        //        result.Remove(0, StringBetween(result, "", "").Length);
        //    }
        //}

        Console.WriteLine("Result: " + result);
		return result;
	}

	private string ParseToImageGoogle(string source)
    {
		return Global.StringBetween(source
				, "\"cse_image\": [\n          {\n            \"src\": \""
				, "\"");
	}

	public static async Task<bool> IsInSaved(string imageURL)
    {
		var lines = await File.ReadAllLinesAsync("src/saved_images.txt");
		var found = false;
		foreach (var line in lines)
		{
			if (line.Equals(imageURL))
			{
				found = true;
			}
		}
		return found;
	}

	public static string Base64Encode(string plainText)
	{
		var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
		return System.Convert.ToBase64String(plainTextBytes);
	}
}

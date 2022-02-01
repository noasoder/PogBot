
public class Global
{
	public static Global Instance { get; private set; }

	public string key = String.Empty;
	public string cx = String.Empty;
	public string token = String.Empty;

	private HttpClient client;

	public static string discordCommand = "pog";
	public static string q = "filetype: jpg overwatch girl";

	public static void Setup()
    {
		if(Instance == null)
			Instance = new Global();

		Instance.token = LoadFile("src/token.txt");
		Instance.cx = LoadFile("src/cx.txt");
		Instance.key = LoadFile("src/key.txt");
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
		client = new HttpClient();

		client.BaseAddress = new Uri("https://www.googleapis.com/customsearch/v1");
		var getString = "?key=" + key + "&cx=" + cx + "&q=" + Global.q + additionalSearch;
		var result = await client.GetStringAsync(getString);
        //Console.WriteLine("Result: " + result);

        //var foundUnique = false;

        result = ParseToImage(result);
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

	private string ParseToImage(string source)
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
}

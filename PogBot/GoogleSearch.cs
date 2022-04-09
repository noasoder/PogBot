namespace PogBot
{
    public class GoogleSearch
    {
        public static async Task<string> GetImage(HttpClient client, string search)
        {
			var match = "\"kind\": \"customsearch#result\",\n";
			var startIndex = new Random().Next(1, Global.queriesToChooseFrom + 1);

			//Get request
			var getString = "https://www.googleapis.com/customsearch/v1?key="
						+ Global.Instance.gKey + "&cx=" + Global.Instance.gCx + "&start=" + startIndex + "&q=" + search;
			var result = await client.GetStringAsync(getString);

			//Console.WriteLine("Result: " + result);
			Console.WriteLine("Search: " + search);

			//Separate to head and body
			var searchHead = Global.StringFirstToMatch(result, match);
			var searchBody = result.Substring(searchHead.Length);

			//Find all image links
			var images = new List<string>();
			var lastResultLength = match.Length;
			const int resultsPerPage = 9;

			for (int i = 0; i < resultsPerPage; i++)
			{
				var oneResult = Global.StringIndexToMatch(searchBody, lastResultLength, match);
				var oneImage = ParseToImageGoogle(oneResult);
				images.Add(oneImage);
				lastResultLength += oneResult.Length + match.Length;
			}

            foreach (var image in images)
            {
				if(!Global.IsInSaved(image))
                {
					Console.WriteLine("ImageLink: " + image);
					return image;
                }
            }

            return Global.noImageMessage;
		}

		private static string ParseToImageGoogle(string source)
		{
			return Global.StringBetween(source
					, "\"cse_image\": [\n          {\n            \"src\": \""
					, "\"");
		}
	}
}

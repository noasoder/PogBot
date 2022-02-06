
namespace PogBot
{
    class Search
    {
		public static async Task<string> GetImage(string additionalSearch)
		{
			var search = "";
			if (additionalSearch.Equals(""))
				search = Global.RandomSearchQuery();
			else
				search = Global.q + additionalSearch;

			if (Global.googleSearch)
			{
				return await GoogleSearch.GetImage(Global.Instance.Client, search);
			}

			return Global.noImageMessage;
		}

		public static async Task<bool> SaveImageRef(string imageURL)
		{
			var found = Global.IsInSaved(imageURL);

			if (!found)
			{
				File.AppendAllText(Global.saveFile, imageURL + "\n");
			}

			return found;
		}
	}
}

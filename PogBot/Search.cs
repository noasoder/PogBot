namespace PogBot
{
    class Search
    {
		public static async Task<(string, string)> GetImage(string additionalSearch)
		{
			var search = "";
			if (additionalSearch.Equals(""))
            {
				var rand = new Random().Next(0, 3);

				switch (rand)
                {
					case 0: search = Global.qOW + " " + Global.RandomSearchQuery(Global.queriesOW); break;
					case 1: search = Global.qLOL + " " + Global.RandomSearchQuery(Global.queriesLOL); break;
					case 2: search = Global.qValorant + " " + Global.RandomSearchQuery(Global.queriesValorant); break;
                }
            }
			else
				search = additionalSearch;

			if (Global.googleSearch)
			{
				return (search, await GoogleSearch.GetImage(Global.Instance.Client, search));
			}

			return (search, Global.noImageMessage);
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

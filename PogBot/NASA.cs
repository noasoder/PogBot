using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PogBot
{
    public class NASA
    {
        public static async Task<(string, string)> GetAPOD()
        {
            var getString = "https://api.nasa.gov/planetary/apod?api_key=" + Global.Instance.nasaKey;
            var result = await Global.Instance.Client.GetStringAsync(getString);

            var explanation = Global.StringBetween(result, "\"explanation\":\"", "\",\"hdurl\":\"");
            var imageUrl = Global.StringBetween(result, "hdurl\":\"", "\",\"");
            return (explanation, imageUrl);
        }
    }
}

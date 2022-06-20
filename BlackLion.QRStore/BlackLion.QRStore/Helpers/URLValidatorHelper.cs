using System.Text.RegularExpressions;

namespace BlackLion.QRStore.Helpers
{
    public class URLHelper
    {
        public static bool IsValid(string URL)
        {
            string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            
            return Rgx.IsMatch(URL);
        }

        public static string NormalizeURL(string URL)
        {
            if (!URL.StartsWith("http://") || !URL.StartsWith("https://"))
            {
                return "http://" + URL;
            }

            return URL;
        }
    }
}

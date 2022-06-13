using System.Text.RegularExpressions;

namespace BlackLion.QRStore.Helpers
{
    internal class URLValidatorHelper
    {
        public static bool IsValidURL(string URL)
        {
            string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            
            return Rgx.IsMatch(URL);
        }
    }
}

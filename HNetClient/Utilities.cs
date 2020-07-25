using System;
using System.Text.RegularExpressions;

namespace AngularClient
{
    public static class Utilities
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string CleanInput(string input)
        {
            try
            {
                string cleanStr = input.Trim();
                return Regex.Replace(cleanStr, "[^0-9a-zA-Z ]+", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public static bool ContainsLike(string source, string like)
        {
            return Regex.IsMatch(source, LikeToRegular(like));
        }

        private static string LikeToRegular(string value)
        {
            return "^" + Regex.Escape(value).Replace("_", ".").Replace("%", ".*") + "$";
        }
    }
}

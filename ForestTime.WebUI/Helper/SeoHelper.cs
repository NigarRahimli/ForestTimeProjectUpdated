using System.Text.RegularExpressions;

namespace ForestTime.Helpers
{
    public static class SeoHelper
    {
        public static string SeoUrlCreater(string url)
        {
            url = url.ToLower();
            url.Replace("ə", "e").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u").Replace("ç", "c").Replace("ş", "s").Replace("ğ", "g");
            string result = Regex.Replace(url, "[^a-z0-9]", "-");
            return result;
        }
    }
}
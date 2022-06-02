using System.Globalization;

namespace RTS.Database.Extraction
{
    public class Utilities
    {
        public static string ToPascalCase(string text)
        {
            return CultureInfo.InvariantCulture.TextInfo
                .ToTitleCase(text.ToLowerInvariant())
                .Replace("-", "")
                .Replace("_", "")
                .Replace(" ", "");
        }

        public static string ToString(bool pValue)
        {
            if (pValue == true)
                return "true";
            else
                return "false";
        }

    }
}

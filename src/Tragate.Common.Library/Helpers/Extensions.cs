using System.Text.RegularExpressions;

namespace sd
{
    //Extension methods must be defined in a static class
    public static class StringExtension
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static string TrimAndReduce(this string str)
        {
            return ConvertWhitespacesToSingleSpaces(str).Trim();
        }

        public static string ConvertWhitespacesToSingleSpaces(this string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        public static string HtmlTemizle(this string value)
        {
            value = value.Replace("&nbsp;", "");
            char[] array = new char[value.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < value.Length; i++)
            {
                char let = value[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static string Kisalt(this string s, int length)
        {
            if (s.Length > length) return s.Substring(0, length);
            return s;
        }
    }
}
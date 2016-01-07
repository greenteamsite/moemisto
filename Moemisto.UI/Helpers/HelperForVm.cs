using System;
using System.Globalization;

namespace Moemisto.UI.Helpers
{
    public static class HelperForVm
    {
        /// <summary>
        /// Обрізає текст на задану кількість символів по останнє слово
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string TextCut(string text,  int maxLength)
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }
            if (text.Length <= maxLength)
            {
                return text;
            }
            var mas = text.Split(' ','-','–');
            string res = "";
            foreach (var word in mas)
            {
                if ((res + word).Length > maxLength)
                {
                    break;
                }
                res = res + word + " ";
            }
            if (res.Length == 0)
            {
                return string.Empty;
            }
            return text.Substring(0, res.Length - 1) + " ...";
        }

        //SortableDateTimePattern (ISO 8601)
        public static string ToIso8601(DateTime value)
        {
            return value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture);
        }
    }
}
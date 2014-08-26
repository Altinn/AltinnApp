using System;
using System.Collections.Generic;

namespace AltinnApp.Core.Util
{
    public static class Util
    {
        public static bool IsHtml(string summary)
        {
            if (summary.Contains("<p>") || summary.Contains("<html>") || summary.Contains("<body>") ||
                summary.Contains("<br>") || summary.Contains("</a>"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Parses a 401 "insufficient authentication level exception for the levels needed
        /// </summary>
        /// <param name="message">The exception message from the API</param>
        /// <returns>Dictionary with the requred and current security level</returns>
        public static Dictionary<string, char> GetAutLevel(string message)
        {
            //If we get the "Not authorized" message it will look like this: This operation requires authentication level 2 Current level is only 1
            var ints = new List<char>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (char c in message)
            {
                if (Char.IsDigit(c))
                {
                    ints.Add(c);
                }
            }

            var dic = new Dictionary<string, char>();
            if (ints.Count > 0)
            {
                //We know the first one is the "required" and the second one is what we currently have
                dic.Add("required", ints[0]);
                dic.Add("current", ints[1]);
            }

            return dic;
        }

        /// <summary>
        /// Convenience method to add headers
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static Dictionary<string, string> AddHeader(string key, string value, Dictionary<string, string> headers)
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }

            if (!headers.ContainsKey(key))
            {
                headers.Add(key, value);
            }

            return headers;
        }
    }
}
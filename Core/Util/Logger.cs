using System;
using System.Diagnostics;

namespace AltinnApp.Core.Util
{
    /// <summary>
    ///     Util class to logg information
    /// </summary>
    public static class Logger
    {
        /// <summary>
        ///     Log to Console
        /// </summary>
        /// <param name="o"></param>
        public static void Logg(Object o)
        {
            Debug.WriteLine(o.ToString());
        }

        /// <summary>
        ///     Log to Console with additional string s
        /// </summary>
        /// <param name="s"></param>
        /// <param name="o"></param>
        public static void Logg(string s, Object o)
        {
            Debug.WriteLine(s + ": " + o);
        }
    }
}
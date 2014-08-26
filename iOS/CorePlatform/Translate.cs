using System.Globalization;
using System.Resources;
using AltinnApp.Core.Models;

namespace AltinnApp.iOS.CorePlatform
{
    /// <summary>
    /// Class to get translated strings
    /// </summary>
    public class Translate
    {
        /// <summary>
        /// Gets the translated version of the string stringToTranslate
        /// </summary>
        /// <param name="stringToTranslate"></param>
        /// <returns></returns>
        public string GetString(string stringToTranslate)
        {
            var strings = new ResourceManager("AltinnApp.iOS.CorePlatform.Strings", GetType().Assembly);
            string result = strings.GetString(stringToTranslate, new CultureInfo(AppContext.CurrentCulture));

            return result;
        }
    }
}
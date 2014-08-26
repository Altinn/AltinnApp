using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltinnApp.Core.Models
{
    /// <summary>
    ///     The CookieJar is the container for the cookies Altinn uses
    /// </summary>
    public class CookieJar
    {
        /// <summary>
        /// List of cookies in this jar
        /// </summary>
        private readonly List<Cookie> _cookies;

        public string AspxAuthCookie()
        {
            string ret = string.Empty;
            foreach (Cookie c in _cookies)
            {
                if (c.Name == AppContext.AuthCookieName)
                {
                    ret = c.Name;
                }
            }
            //var c = _cookies.Select(cookie => cookie.Name == AppContext.AuthCookieName) as Cookie;

            return ret;
        }

        /// <summary>
        /// Create a new Cookie Jar
        /// </summary>
        public CookieJar()
        {
            _cookies = new List<Cookie>();
        }

        /// <summary>
        ///     The size is determined to to be the total sum of values of all the cookies in the cookie jar
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return _cookies.Sum(c => c.Size());
        }

        /// <summary>
        ///     Add a new Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public void Add(Cookie cookie)
        {
            _cookies.Add(cookie);
        }

        /// <summary>
        ///     Remove the Cookie with the name cookieName
        /// </summary>
        /// <param name="cookieName"></param>
        public void Remove(string cookieName)
        {
            foreach (Cookie c in _cookies.ToList())
            {
                if (c.Name == cookieName)
                {
                    _cookies.Remove(c);
                }
            }
        }

        /// <summary>
        ///     Add a collection of cookies
        /// </summary>
        /// <param name="cookies"></param>
        public void AddRange(List<Cookie> cookies)
        {
            cookies.AddRange(cookies);
        }

        /// <summary>
        ///     Update the cookie with the new value newValue
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="newValue"></param>
        public void Update(Cookie cookie, string newValue)
        {
            Cookie newCookie = cookie;
            newCookie.Value = newValue;
            Remove(cookie.Name);
            Add(newCookie);
        }

        /// <summary>
        ///     Update the cookie with name cookieName and new value newValue
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="newValue"></param>
        public void Update(string cookieName, string newValue)
        {
            var newCookie = new Cookie
            {
                Name = cookieName,
                Value = newValue
            };
            Remove(cookieName);
            Add(newCookie);
        }

        /// <summary>
        ///     String representation of this cookie jar as a semi-colon separated list on the key=value form
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var str = new StringBuilder();

            foreach (Cookie c in _cookies)
            {
                string thisCookie = c.Name.ToUpper() + "=" + c.Value + ";";
                str.Append(thisCookie);
            }

            return str.ToString();
        }

        /// <summary>
        ///     Get the IDPorten cookie from the cookie jar
        /// </summary>
        /// <returns></returns>
        public string IDPortenCookie()
        {
            string ret = string.Empty;
            // ReSharper disable once SuspiciousTypeConversion.Global
            var c = _cookies.Select(cookie => cookie.Name == AppContext.IDPortenUrl) as Cookie;
            if (c != null)
            {
                ret = c.Name;
            }

            return ret;
        }
    }
}
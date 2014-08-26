namespace AltinnApp.Core.Models
{
    /// <summary>
    ///     Object for the cookie
    /// </summary>
    public class Cookie
    {
        /// <summary>
        /// Create a new Cookie
        /// </summary>
        public Cookie()
        {
            Name = string.Empty;
            Value = string.Empty;
        }

        public Cookie(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the name of the cookie
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the cookie
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Returns the size of this Cookie; the size of the value.
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return Value.Length;
        }
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AltinnApp.Core.Models
{
    public class Profile
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string PreferredLanguage { get; set; }
        public bool ShowClientUnits { get; set; }

        [JsonProperty(PropertyName = "_links")]
        public JObject Links { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class Links
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public object Title { get; set; }
        public object MimeType { get; set; }
        public bool IsTemplated { get; set; }
        public bool Encrypted { get; set; }
    }
}
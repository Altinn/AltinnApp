using System.Collections.Generic;
using System.Linq;

namespace AltinnApp.Core.Models
{
    /// <summary>
    /// The Seesion the current user has
    /// </summary>
    public static class Session
    {
		public static object SentCorres = "Arkivert";

		public static object Sent = "Sendt og arkivert";

        public static List<Organization> Organizations { get; set; }

        public static List<Organization> UnfilteredOrganizations { get; set; }

        public static Organization SelectedOrg { get; set; }

        public static Messagebox mmb { get; set; }

        public static Messagebox mab { get; set; }

        public static void SearchActor(string searchText)
        {
            List<Organization> mess = UnfilteredOrganizations;
            var list =
                mess.Where(
                    m =>
                        m.Name.ToLower().Contains(searchText.ToLower()) ||
                        m.OrganizationNumber.ToLower().Contains(searchText.ToLower()));
            List<Organization> filtered = list.ToList();
            Organizations = filtered;
        }

        public static void Logout()
        {
            //When the user logs out, clear the cookies
            //Session.AuthCookie = null;
            CookieJar = new CookieJar();
            IDPortenCookie = null;
            LoggedIn = false;
            SelectedOrg = null;
            Organizations = null;
            Profile = null;
            UnfilteredOrganizations = null;
            mmb = null;
        }

        /// <summary>
        ///  Gets or sets the container for our cookies
        /// </summary>
        public static CookieJar CookieJar { get; set; }

        /// <summary>
        ///  Gets or sets the IDPorten cookie explicitly needed to open SvarUT Correspondences
        /// </summary>
        public static Cookie IDPortenCookie { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether this user is logged in
        /// </summary>
        public static bool LoggedIn { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether the app tried to log out using external Safari
        /// </summary>
        public static bool DidLogOut { get; set; }

        public static Profile Profile { get; set; }
    }
}
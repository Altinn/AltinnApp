using AltinnApp.Core.SAL;

namespace AltinnApp.Core.Models
{
    /// <summary>
    ///     Class to hold various environment variables
    /// </summary>
    public static class AppContext
    {
        public static bool hasLaunched { get; set; }

        public static string CurrentCulture = "nb-NO";

        #region Constants

        /// <summary>
        ///     Relative url to AppStart
        /// </summary>
        public const string RelativeAppStartUrl = "Pages/temp/AppStart.aspx";

        /// <summary>
        ///     Relative URL to AppStop
        /// </summary>
        public const string RelativeAppStopUrl = "Pages/logout/AppStop.aspx";

        /// <summary>
        ///     The API key registered for this app
        /// </summary>
        public const string ApiKey = "Go to https://www.altinn.no/api/help to request a key";

        /// <summary>
        ///     The URL Schema for this app, must be the same as the one registered together with the API key
        /// </summary>
        public const string AppUrlSchema = "app.altinn.no";

        /// <summary>
        ///     IDPorten URL
        /// </summary>
        public const string IDPortenUrl = "difi.no";

        /// <summary>
        /// Name of the cookie containing the user session
        /// </summary>
        public const string AuthCookieName = ".aspxauth";

        /// <summary>
        /// Name of the cookie containing the ASP.net session
        /// </summary>
        public const string ASPSessionCookieName = "ASP.NET_SessionId";

        /// <summary>
        /// Name of the parameter passing the logged out status
        /// </summary>
        public const string LoggedOutParameter = "loggedOut";

        /// <summary>
        /// Name of the parameter passing the session ID
        /// </summary>
        public const string SessionParameter = "sid";

        /// <summary>
        /// Name of the IDPorten cookie we are intersted in
        /// </summary>
        public const string IDPortenCookieName = "IDPSecurityPortal";

        #endregion

        /// <summary>
        /// The type of environment selected
        /// </summary>
        public static EnvironmentType SelectedEnvironment;

        /// <summary>
        ///     Path for organizations this reportee is able to represent
        /// </summary>
        private static readonly string RelativeRestUrlOrgs = "api/organizations";

        /// <summary>
        ///     Relative REST URL to the message box
        /// </summary>
        private static readonly string RelativeRESTURL = "api/{0}/messages";

        /// <summary>
        ///     Relative path to the message box for a reportee
        /// </summary>
        private static readonly string RelativeRestUrlReportee = "api/my/messages";

        /// <summary>
        ///     Path to trigger IDPorten login
        /// </summary>
        private static readonly string RelativeLoginUrl = "api/my/messages";


        public static string RelativeProfileUrl = "api/my/profile";


        /// <summary>
        ///     Absolute path to AppStop
        /// </summary>
        private static string _appStopUrl;

        /// <summary>
        ///     Absolute paths for organizations this reportee is able to represent
        /// </summary>
        private static string _restrUrlReportee;

        /// <summary>
        ///     Absolute path to the message box
        /// </summary>
        private static string _resturl;

        /// <summary>
        ///     Root to trigger IDPorten login
        /// </summary>
        private static string _loginUrl;

        /// <summary>
        ///     The current language. 1044 = Norwegian Bokmål, 1033 = English. Passed along to the API to get the correct language
        ///     on the messages
        /// </summary>
        private static string _currentLanguage = "1044";

        /// <summary>
        ///     Which login method to use, i.e. internal Safari or external Safari. For BankID to work it needs to be external
        ///     (true)
        /// </summary>
        private static bool _useExternalBrowser = false;

        public static bool FirstTimeStarting;


        /// <summary>
        /// Enum for the environment
        /// </summary>
        public enum EnvironmentType
        {
            /// <summary>
            /// Enum for Production
            /// </summary>
            PROD,

            /// <summary>
            /// Enum for Systems Test
            /// </summary>
            ST2,

            /// <summary>
            /// Enum for Acceptance Test
            /// </summary>
            AT3,
            AT6,

            /// <summary>
            /// Enum for Production Test
            /// </summary>
            TT2
        }

        /// <summary>
        /// Gets or sets the Login method selected
        /// </summary>
        public static int LoginMethod { get; set; }

        /// <summary>
        /// Gets or sets the absolute path to AppStart
        /// </summary>
        public static string AppStartUrl { get; set; }

        /// <summary>
        ///  Gets or sets the domain we are working with, for production this is https://www.altinn.no
        /// </summary>
        public static string Domain { get; set; }

        /// <summary>
        /// Gets or sets the absolute path to the messages for this organization
        /// </summary>
        public static string RESTUrlOrg { get; set; }

        public static string ProfileURL { get; set; }

        /// <summary>
        ///  Gets or sets the absolute path for organizations this reportee is able to represent
        /// </summary>
        public static string RESTUrlOrgs { get; set; }

        /// <summary>
        /// Gets or sets the absolute path to AppStop
        /// </summary>
        public static string AppStopUrl
        {
            get { return _appStopUrl; }
            set { _appStopUrl = value; }
        }

        /// <summary>
        /// Gets or sets the absolute paths for organizations this reportee is able to represent
        /// </summary>
        public static string RESTRUrlReportee
        {
            get { return _restrUrlReportee; }
            set { _restrUrlReportee = value; }
        }

        /// <summary>
        /// Gets or sets the absolute path to the message box
        /// </summary>
        public static string RESTURL
        {
            get { return _resturl; }
            set { _resturl = value; }
        }

        /// <summary>
        /// Gets or sets the root to trigger IDPorten login
        /// </summary>
        public static string LoginUrl
        {
            get { return _loginUrl; }
            set { _loginUrl = value; }
        }

        /// <summary>
        /// Gets or sets the The current language. 1044 = Norwegian Bokmål, 1033 = English. Passed along to the API to get the correct language
        /// on the messages
        /// </summary>
        public static string CurrentLanguage
        {
            get { return _currentLanguage; }
            set { 
			
				_currentLanguage = value; }
        }

        /// <summary>
        ///  Gets or sets a value indicating whether to use internal Safari or external Safari. For BankID to work it needs to be external
        /// </summary>
        public static bool UseExternalBrowser
        {
            get { return _useExternalBrowser; }
            set { _useExternalBrowser = value; }
        }

        /// <summary>
        ///  Set the domain to use, this is only used when testing, once the app goes into production this is always
        ///  EnvironmentType.PROD
        /// </summary>
        /// <param name="environmentType"></param>
        public static void SetDomain(EnvironmentType environmentType)
        {
            if (environmentType == EnvironmentType.PROD)
            {
                Domain = "https://www.altinn.no/";
            }
            else if (environmentType == EnvironmentType.ST2)
            {
                Domain = "https://st02.altinn.basefarm.net/";
            }
            else if (environmentType == EnvironmentType.AT3)
            {
                Domain = "https://at03.altinn.basefarm.net/";
            }
            else if (environmentType == EnvironmentType.AT6)
            {
                Domain = "https://at06.altinn.basefarm.net/";
            }
            else if (environmentType == EnvironmentType.TT2)
            {
                Domain = "https://tt02.altinn.basefarm.net/";
            }

            _restrUrlReportee = Domain + RelativeRestUrlReportee;
            RESTUrlOrg = Domain + "api/{0}/messages";
            _resturl = Domain + RelativeRESTURL;
            RESTUrlOrgs = Domain + RelativeRestUrlOrgs;
            _loginUrl = Domain + RelativeLoginUrl;
            _appStopUrl = Domain + RelativeAppStopUrl;
            AppStartUrl = Domain + RelativeAppStartUrl;

            ProfileURL = Domain + RelativeProfileUrl;
        }
    }
}
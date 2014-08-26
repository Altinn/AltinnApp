namespace AltinnApp.Core.Util
{
    /// <summary>
    ///     Class holding error codes. Can be localized as needed.
    /// </summary>
    public static class ErrorMessages
    {
        /// <summary>
        /// Enums of network conditions that might occur
        /// </summary>
        public enum Network
        {
            /// <summary>
            /// Invalid authentication
            /// </summary>
            InvalidAuthSession,

            /// <summary>
            /// Connection error
            /// </summary>
            ConnectionError
        }

        public const string RemoteMessageError = "RemoteMessageError";

        /// <summary>
        /// Enum of Service conditions that might occur
        /// </summary>
        public enum Service
        {
            /// <summary>
            /// Invalid authentication
            /// </summary>
            InvalidAuthSession,

            /// <summary>
            /// Connection error
            /// </summary>
            ConnectionError,

            /// <summary>
            /// No support for BankID
            /// </summary>
            BankIDNotSupported
        }

        public static string MustOverride = "Must override DownloadData";

        /// <summary>
        ///     General "could not connect" message
        /// </summary>
        public const string ConnectionFailed = "ConnectionFailed";

        /// <summary>
        ///     General "could not connect" message
        /// </summary>
        public const string GeneralError = "GeneralError";

        /// <summary>
        ///     General "could not connect" message
        /// </summary>
        //public const string SerializationError = "En feil oppstod under tolking av data, vennligst prøv igjen";
        /// <summary>
        ///     Failed to show the print view
        /// </summary>
        public const string PrintFailed = "PrintError";

        public const string SerializationError = "SerializationError";

        /// <summary>
        ///     Critical error occured, abort
        /// </summary>
        /// <value>The critical error.</value>
        public const string CriticalError = "CriticalError";

        /// <summary>
        ///     401 gets returned if the user has insufficient authorization level. With "MinID" it is level 3, with "BankID" it is
        ///     level 4
        /// </summary>
        public const string InsufficientAuthorization = "InsufficientAuthorization";

        //"Du har for lavt sikkerhetsnivå for å lese denne meldingen, for å lese den må du logge inn på ny med {0}";

        /// <summary>
        /// Not logged in error
        /// </summary>
        //public static string NotLoggedIn = "Innloggingen har timet ut, logg inn på ny for å fortsette";
        /// <summary>
        /// BankID not supported error
        /// </summary>
        public const string BankIDNotSupported = "BankIDNotSupported";

        /// <summary>
        /// Network not available error
        /// </summary>
        public const string NoNetworkAvailable = "NoNetworkAvailable";
    }
}
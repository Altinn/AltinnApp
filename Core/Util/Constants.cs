using System.Globalization;

namespace AltinnApp.Core.Util
{
    /// <summary>
    /// Container for various constants
    /// </summary>
    public class Constants
    {
        public static CultureInfo NorwegianBokmaal = new CultureInfo("nb-NO");
        public static CultureInfo English = new CultureInfo("nb-NO");
        public static CultureInfo NorwegianNynorsk = new CultureInfo("nn-NO");

        public static string FeedbackMail = "  api@altinn.no";

        public static string ContactMail = "  support@altinn.no";

        public static string ContactSupportTlf = "  75 00 60 00";


        /// <summary>
        /// The code for Norwegian Bokmål
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const string NoBM = "1044";

        /// <summary>
        /// The code for English
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const string En = "1033";

        /// <summary>
        /// The code for Norwegian Nynorsk
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const string NoNN = "1020";

        /// <summary>
        ///     The type of content we want to retrieve from the API
        /// </summary>
        public const string ApiFormat = "application/hal+json";

        /// <summary>
        ///     The type of messagebox we model in the app
        ///     Messagebox is all messages which is NOT archived
        ///     Arhcive is all messages which IS archived
        ///     All is both of the above
        /// </summary>
        public enum MMBType
        {
            /// <summary>
            /// The messagebox type (in inbox)
            /// </summary>
            Messagebox,

            /// <summary>
            /// The archive type
            /// </summary>
            Archive
        }

        /// <summary>
        ///     Language code used to determine the language on the variable fields in the API
        ///     For instance, if noBM is used language=1044 as filter on the API call, then the status code will be in Status-NO
        ///     format
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// The FormTask type (regular form)
            /// </summary>
            FormTask,

            /// <summary>
            /// The Correspondence type (message)
            /// </summary>
            Correspondence
        }

        /// <summary>
        /// The English texts
        /// </summary>
        public static class EN
        {
            /// <summary>
            /// The "Your self" label in English
            /// </summary>
            //public static string Yourself { get; set; }
            /// <summary>
            ///     The status codes on the messages comming from the API in english
            /// </summary>
            public static class Status
            {
                /// <summary>
                /// Message is read
                /// </summary>
                public const string Read = "Read";

                /// <summary>
                /// Message is opened
                /// </summary>
                public const string Opened = "Opened";

                /// <summary>
                /// Message is to be signed
                /// </summary>
                public const string Signing = "Signing";

                /// <summary>
                /// Message is archived
                /// </summary>
                public const string Sent = "Sent and archived";

                /// <summary>
                /// Message is to be filled out
                /// </summary>
                public const string Fillout = "Completion";

                /// <summary>
                /// Message is not read
                /// </summary>
                public const string Unread = "Unread";
            }
        }

        /// <summary>
        /// The Norwegian texts
        /// </summary>
        public static class NO
        {
            /// <summary>
            /// The Norwegian Nynorsk texts
            /// </summary>
            public static class Non
            {
                /// <summary>
                /// YourSelf in Norwegian Nynorsk
                /// </summary>
                //private const string Yourself = "Deg sjølv";
                /// <summary>
                ///     The status codes on the messages comming from the API in english
                /// </summary>
                public static class Status
                {
                    /// <summary>
                    /// Message is read
                    /// </summary>
                    public const string Read = "Lest";

                    /// <summary>
                    /// Message is opened
                    /// </summary>
                    public const string Opened = "Åpnet";

                    /// <summary>
                    /// Message is to be signed
                    /// </summary>
                    public const string Signing = "Signering";

                    /// <summary>
                    /// Message is archived
                    /// </summary>
                    public const string Sent = "Sendt og arkivert";

                    /// <summary>
                    /// Message is to be filled out
                    /// </summary>
                    public const string FillOut = "Utfylling";

                    /// <summary>
                    /// Message is not read
                    /// </summary>
                    public const string Unread = "Ulest";
                }
            }

            /// <summary>
            /// The Norwegian Bokmål texts
            /// </summary>
            public static class Nob
            {
                //public static string CorresArchiveDateText = "Sist endret:";


                //public static string SocialAppNotInstalled = "Du må ha appen installert for å starte den direkte";


                //public static string OnlyAttachmentMessage = "Denne meldingen har bare vedlegg";


                ///// <summary>
                ///// The "archive" label
                ///// </summary>
                //public const string Archive = "  Arkiv";


                ///// <summary>
                ///// The "inbox" label
                ///// </summary>
                //public const string Messagebox = "Innboks";

                ///// <summary>
                ///// "Your self" label
                ///// </summary>
                //public const string YourSelf = "Deg selv";

                ///// <summary>
                ///// "Load more" label
                ///// </summary>
                //public const string LoadMore = "Last flere";

                ///// <summary>
                /////     When attachment is encrypted it cannot be forwarded
                ///// </summary>
                //public const string CanNotForward = "Kan ikke videresendes";

                //public static string SearchFailed = "Søket feilet, prøv igjen";
                //public static string Innbox = "Innboks";
                //public static string NoAttachments = "Ingen vedlegg";
                //public static string NobSearchPlceholder = "Søk";

                /// <summary>
                ///     The status codes on the messages comming from the API in Norwegian Bokmål
                /// </summary>
                public static class Status
                {
                    /// <summary>
                    ///     The status codes on the messages comming from the API in Norwegian Bokmål
                    /// </summary>
                    public const string Read = "Lest",
                        Opened = "Åpnet",
                        Signing = "Signering",
                        Sent = "Sendt og arkivert",
                        SentCorres = "Arkivert",
                        FillOut = "Utfylling",
                        Unread = "Ulest";
                }

                //public static string SelectLanguage = "Språk i appen";
            }
        }
    }
}
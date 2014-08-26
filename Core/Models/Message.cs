using System;

namespace AltinnApp.Core.Models
{
    /// <summary>
    ///     These are the models used to de-serialize the JSON objects comming from the API
    /// </summary>
    public class Rootobject
    {
        /// <summary>
        ///  Gets or sets the array of messages
        /// </summary>
        public Message[] Messages { get; set; }
    }

    /// <summary>
    ///     The Message contains all the fields which are common to the types FormTask and Correspondence, to get the specific
    ///     fields you have to use the "self" link on the message
    /// </summary>
    public class Message
    {
        public string SavedFileName;
        public bool IsDownloaded { get; set; }

        public string AdjustedDate { get; set; }


        /// <summary>
        ///  Gets or sets the ID of this message
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        ///  Gets or sets the subject of this message
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///  Gets or sets the status of this message
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///  Gets or sets the last date of change for this message
        /// </summary>
        public DateTime LastChangedDateTime { get; set; }

        /// <summary>
        ///  Gets or sets the entity who changes this message last
        /// </summary>
        public string LastChangedBy { get; set; }

        /// <summary>
        ///  Gets or sets the Service Owner
        /// </summary>
        public string ServiceOwner { get; set; }

        /// <summary>
        ///  Gets or sets the Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///  Gets or sets the ServiceCode
        /// </summary>
        public string ServiceCode { get; set; }

        /// <summary>
        ///  Gets or sets the ServiceEdition
        /// </summary>
        public int ServiceEdition { get; set; }

        /// <summary>
        ///  Gets or sets the list of links for this message
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "_links")]
        public _Links Links { get; set; }

        /// <summary>
        /// Gets or sets the summary of the Correspondence, specific for Correspondences
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the body of the Correspondence in HTML, specific for Correspondences
        /// </summary>
        public string Body { get; set; }
    }

    /// <summary>
    ///     The REST links related to a message
    /// </summary>
// ReSharper disable once InconsistentNaming
    public class _Links
    {
        /// <summary>
        ///  Gets or sets the Self
        /// </summary>
        public Self Self { get; set; }

        /// <summary>
        ///  Gets or sets the Print
        /// </summary>
        public Print Print { get; set; }

        /// <summary>
        ///  Gets or sets the array of attachments
        /// </summary>
        public Attachment[] Attachment { get; set; }
    }

    /// <summary>
    /// This message
    /// </summary>
    public class Self
    {
        /// <summary>
        ///  Gets or sets the Href
        /// </summary>
        public string Href { get; set; }
    }

    /// <summary>
    ///     The print view of a FormTask, specific for FormTasks
    /// </summary>
    public class Print
    {
        /// <summary>
        /// Gets or sets the URI to the print view
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// Gets or sets the MIME type
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "mime-type")]
        public string Mimetype { get; set; }
    }

    /// <summary>
    ///     The Attachment of a Correspondence, specific for Correspondences
    /// </summary>
    public class Attachment
    {
        public string Mimetype { get; set; }

        /// <summary>
        /// Gets or sets the URI to the Attachment
        /// </summary>
        public string Href { get; set; }

        public string FileEnding { get; set; }

        /// <summary>
        /// Gets or sets the name of the Attachment
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the Attachment is encrypted, if it is encrypted any download of the file will be corrupt
        /// </summary>
        public bool Encrypted { get; set; }
    }
}
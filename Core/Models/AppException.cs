using System;

namespace AltinnApp.Core.Models
{
    /// <summary>
    ///     Class to throw and manage App related exceptions
    /// </summary>
    public class AppException : Exception
    {
        public string ErrorType { get; set; }

        public string Response { get; set; }

        /// <summary>
        /// Gets or sets the statuscode
        /// </summary>
        public string HttpStatusCode { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Create a new AppException to handle later
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="description"></param>
        public AppException(string statusCode, string description)
        {
            Description = description;
            HttpStatusCode = statusCode;
        }

        /// <summary>
        /// Create a new AppException to handle later
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="statusCode"></param>
        /// <param name="description"></param>
        /// <param name="responseUri"></param>
        public AppException(string statusCode, string description, string responseUri)
        {
            Response = responseUri;
            HttpStatusCode = statusCode;
            Description = description;
        }
    }
}
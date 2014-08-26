using System;
using MonoTouch.Foundation;

namespace AltinnApp.iOS.Util
{
    /// <summary>
    /// Implementation of the native URL delegate, we need to override this to allow selv signed certificates and to monitor the HTTP status codes
    /// </summary>
    internal class NativeUrlDelegate : NSUrlConnectionDelegate
    {
        /// <summary>
        /// Success action
        /// </summary>
        private readonly Action<NSMutableData, int> _successCallback;

        /// <summary>
        /// Failure action
        /// </summary>
        private readonly Action<string, int> _failureCallback;

        /// <summary>
        /// The data to return
        /// </summary>
        private readonly NSMutableData _data;

        /// <summary>
        /// HTTP status code
        /// </summary>
        private int _statusCode;

        /// <summary>
        /// Response object
        /// </summary>
        public NSUrlResponse Resp;

        /// <summary>
        /// Use this URL delegate if it is neeed to have more control over the connection
        /// </summary>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public NativeUrlDelegate(Action<NSMutableData, int> success, Action<string, int> failure)
        {
            _successCallback = success;
            _failureCallback = failure;
            _data = new NSMutableData();
        }

        public override void ReceivedData(NSUrlConnection connection, NSData d)
        {
            _data.AppendData(d);
        }

        public override void ReceivedResponse(NSUrlConnection connection, NSUrlResponse response)
        {
            var httpResponse = response as NSHttpUrlResponse;
            Resp = httpResponse;
            if (httpResponse == null)
            {
                _statusCode = -1;
                return;
            }

            _statusCode = httpResponse.StatusCode;
        }

        public override void FailedWithError(NSUrlConnection connection, NSError error)
        {
            if (_failureCallback != null)
                _failureCallback(error.LocalizedDescription, _statusCode);
        }

        public override void FinishedLoading(NSUrlConnection connection)
        {
            if (_statusCode != 200)
            {
                _failureCallback(string.Format("Did not receive a 200 HTTP status code, received '{0}'", _statusCode),
                    _statusCode);
                return;
            }

            _successCallback(_data, _statusCode);
        }

        /// <summary>
        /// We want to allow self signed certificates in testing for development purposes, it is very important to remove this override for a production product
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="protectionSpace"></param>
        /// <returns></returns>
        //public override bool CanAuthenticateAgainstProtectionSpace(NSUrlConnection connection,
        //    NSUrlProtectionSpace protectionSpace)
        //{
        //    return true;
        //}

        /// <summary>
        /// We want to allow self signed certificates in testing for development purposes, it is very important to remove this override for a production product
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="challenge"></param>
        //public override void ReceivedAuthenticationChallenge(NSUrlConnection connection,
        //    NSUrlAuthenticationChallenge challenge)
        //{
        //    if (challenge.ProtectionSpace.AuthenticationMethod == "NSURLAuthenticationMethodServerTrust")
        //        challenge.Sender.UseCredentials(NSUrlCredential.FromTrust(challenge.ProtectionSpace.ServerSecTrust),
        //            challenge);

        //    if (challenge.PreviousFailureCount > 0)
        //    {
        //        challenge.Sender.CancelAuthenticationChallenge(challenge);
        //        Logger.Logg("Authentication failure");
        //        return;
        //    }

        //    if (challenge.ProtectionSpace.AuthenticationMethod == "NSURLAuthenticationMethodServerTrust")
        //        challenge.Sender.UseCredentials(NSUrlCredential.FromTrust(challenge.ProtectionSpace.ServerSecTrust),
        //            challenge);
        //}
    }
}
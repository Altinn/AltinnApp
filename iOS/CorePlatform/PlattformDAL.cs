using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using AltinnApp.Core.DAL;
using AltinnApp.Core.Models;
using AltinnApp.Core.Util;
using AltinnApp.iOS.Util;
using System.Threading.Tasks;

namespace AltinnApp.iOS.CorePlatform
{
    /// <summary>
    /// Inherited class of the Core.DAL to provide the appropiate client transfer mechanism.
    /// This subclass also allows us to override the ICertificatePolicy. It is very important to NOT override this for the production release since it
    /// opens the door to many security threats
    /// </summary>
	internal class PlattformDAL : Data//, ICertificatePolicy
    {
        /// <summary>
        /// Default constructor for the platform specific DAL
        /// </summary>
        public PlattformDAL()
        {
			//ServicePointManager.ServerCertificateValidationCallback = Validator;
        }

        #region RemoveInProductionRelease

        /// <summary>
        /// Override implementation of Validator to accept non-valid certificates to enable self-signed certificates in test environments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        /// 
        public static bool Validator(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Override of CheckValidationResult to accept non-valid certificates to enable self-signed certificates in test environments
        /// </summary>
        /// <param name="srvPoint"></param>
        /// <param name="certificate"></param>
        /// <param name="request"></param>
        /// <param name="certificateProblem"></param>
        /// <returns></returns>
        public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request,
            int certificateProblem)
        {
            return true;
        }

        #endregion

        protected override async Task<byte[]> SetupAndDownloadData(string uri)
        {
            
            //return base.SetupAndDownloadData(uri);
            var ws = new WebClient();

            ws.Headers.Add("Cookie", Session.CookieJar.ToString());
            ws.Headers.Add("ApiKey", AppContext.ApiKey);

            Byte[] res = null;

            if (Reachability.IsHostReachable(uri))
            {
                res = await ws.DownloadDataTaskAsync(uri);
            }
            else
            {
                throw new AppException(ErrorMessages.ConnectionFailed, "0");
            }

            return res;
        }

        protected override async Task<string> SetupAndDownloadString(string uri)
        {
			if (uri.IndexOf ("?") < 0) {
				uri += "?language=" + AppContext.CurrentLanguage;
			} else {
				uri += "&language=" + AppContext.CurrentLanguage;
			}
           
            var ws = new WebClient();

            ws.Headers.Add("Cookie", Session.CookieJar.ToString());
            ws.Headers.Add("ApiKey", AppContext.ApiKey);
            ws.Headers.Add("Accept", Constants.ApiFormat);

            string res = null;

            if (Reachability.IsHostReachable(uri))
            {
                res = await ws.DownloadStringTaskAsync(uri);
            }
            else
            {
                throw new AppException(ErrorMessages.ConnectionFailed, "0");
            }

            return res;
        }
    }
}
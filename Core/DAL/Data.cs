using System;
using System.Net;
using System.Net.Http;
using AltinnApp.Core.Models;
using AltinnApp.Core.Util;
using System.Threading.Tasks;

namespace AltinnApp.Core.DAL
{
    /// <summary>
    ///     This class handles the data transfer of messages.
    ///     Which specific data tranfer client to use us left up to the subclass class and the implementation of "Download",
    ///     often this will be WebClient or HttpClient
    ///     Note that if testing with self signed certificates is needed then the subclass can not be in the PCL project, but
    ///     in the platform specific project
    ///     since we cannot use and override the required classes with PCL.
    /// </summary>
    public class Data
    {
        public async Task<string> SearchActor(string searchValue)
        {
            string url = AppContext.RESTUrlOrgs;


            string param =
                string.Format(
                    "?$filter=substringof('{0}', tolower(Name)) or substringof('{0}', OrganizationNumber)", searchValue);
            string request = url + param;

            string res = await DownloadAndHandle(request);
            return res;
        }

        public async Task<string> Search(string searchString, Constants.MMBType type)
        {
            string url = string.Format(AppContext.RESTURL, Session.SelectedOrg.OrganizationNumber);
            string res = string.Empty;


            if (type == Constants.MMBType.Messagebox)
            {
                string param =
                    string.Format(
                        "?$filter=(Status ne '{0}' and Status ne '{1}') and (substringof('{2}', tolower(Summary)) or substringof('{2}', tolower(Subject)))",
						Session.Sent, Session.SentCorres,
                        searchString.ToLower());

                string request = url + param;

                res = await DownloadAndHandle(request);
            }
            else if (type == Constants.MMBType.Archive)
            {
                string param =
                    string.Format(
                        "?$filter=(Status eq '{0}' or Status eq '{1}') and (substringof('{2}', tolower(Summary)) or substringof('{2}', tolower(Subject)))",
						Session.Sent, Session.SentCorres, searchString);
                string request = url + param;

                res = await DownloadAndHandle(request);
            }

            return res;
        }

        /// <summary>
        /// Gets or sets the Data Access layer object to use with this Service Access Instance
        /// </summary>
        protected Data DAL { get; set; }

        /// <summary>
        /// Gets the messagebox for currently selected reportee
        /// </summary>
        /// <param name="type"></param>
        /// <param name="skipSize"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetMessageBox(Constants.MMBType type, int skipSize, string url)
        {
            return await GetMessageBoxJSON(type, skipSize, url);
        }

        /// <summary>
        ///     Retrieves the JSON for a specific Message.
        ///     This will contain any message specific fields in addition to the common fields.
        /// </summary>
        /// <param name="uri">URI to the message</param>
        /// <returns>Message JSON</returns>
        public async Task<string> GetMessageJSON(string uri)
        {
            var res = await DownloadAndHandle(uri);
            return res;
        }


        /// <summary>
        ///     Get the attachment for a message
        /// </summary>
        /// <param name="uri">uri</param>
        /// <returns>Attachment as byte array</returns>
        public async Task<string> GetAttachmentAsString(string uri)
        {
            string res = null;

            try
            {
                //res = DownloadData(uri + "?language=" + AppContext.CurrentLanguage);
                res = await SetupAndDownloadString(uri);
            }
            catch (AggregateException ex)
            {
                //HttpClient wraps exceptions inside AggregateException
                var inner = ex.InnerException as WebException;
                if (inner != null)
                {
                    var interestingException = inner.Response as HttpWebResponse;
                    if (interestingException != null)
                    {
                        throw new AppException(ErrorMessages.InsufficientAuthorization,
                            interestingException.StatusCode.ToString());
                    }
                }
            }
            catch (WebException w)
            {
                var h = w.Response as HttpWebResponse;
                if (h != null)
                {
                    Logger.Logg("Failed to retrieve the message box due to network error: ", h.StatusCode);
                    throw new AppException(h.StatusCode.ToString(),
                        h.StatusDescription, h.ResponseUri.ToString().ToLower());
                }
            }
            catch (Exception ex)
            {
                Logger.Logg("Unknown network error occured: ", ex.Message);
                throw new AppException("0", ErrorMessages.ConnectionFailed);
            }

            return res;
        }


        /// <summary>
        ///     Get the attachment for a message
        /// </summary>
        /// <param name="uri">uri</param>
        /// <returns>Attachment as byte array</returns>
        public async Task<Byte[]> GetAttachment(string uri)
        {
            Byte[] res = null;

            try
            {
                //res = DownloadData(uri + "?language=" + AppContext.CurrentLanguage);
                res = await SetupAndDownloadData(uri);
            }
            catch (AggregateException ex)
            {
                //HttpClient wraps exceptions inside AggregateException
                var inner = ex.InnerException as WebException;
                if (inner != null)
                {
                    var interestingException = inner.Response as HttpWebResponse;
                    if (interestingException != null)
                    {
                        throw new AppException(ErrorMessages.InsufficientAuthorization,
                            interestingException.StatusCode.ToString());
                    }
                }
            }
            catch (WebException w)
            {
                var h = w.Response as HttpWebResponse;
                if (h != null)
                {
                    Logger.Logg("Failed to retrieve the message box due to network error: ", h.StatusCode);
                    throw new AppException(h.StatusCode.ToString(),
                        h.StatusDescription, h.ResponseUri.ToString().ToLower());
                }
            }
            catch (Exception ex)
            {
                Logger.Logg("Unknown network error occured: ", ex.Message);
                throw new AppException("0", ErrorMessages.ConnectionFailed);
            }

            return res;
        }

        /// <summary>
        ///     Retrieves list of reportees for this user as JSON
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOrganizationsJSON(int skipSize)
        {
            string uri = AppContext.RESTUrlOrgs + "&$skip=" + skipSize;
            var res = await DownloadAndHandle(uri);

            return res;
        }

        /// <summary>
        ///     Retrieves list of reportees for this user as JSON
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOrganizationsJSON()
        {
            string uri = AppContext.RESTUrlOrgs;
            var res = await DownloadAndHandle(uri);

            return res;
        }

        /// <summary>
        ///     Download data
        /// </summary>
        /// <param name="uri">URI to the resource</param>
        /// <returns>Array of bytes</returns>
        protected virtual async Task<byte[]> SetupAndDownloadData(string uri)
        {
            var ws = new HttpClient();
            Byte[] res;
            ws.DefaultRequestHeaders.Accept.Clear();

            if (Session.CookieJar.Size() > 0)
            {
                ws.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", Session.CookieJar.ToString());
                ws.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(Constants.ApiFormat));
                ws.DefaultRequestHeaders.TryAddWithoutValidation("ApiKey", AppContext.ApiKey);

                res = await ws.GetByteArrayAsync(uri);
            }
            else
            {
                //The session can be invalid
                throw new AppException("0", ErrorMessages.InsufficientAuthorization);
            }

            return res;
        }

        protected virtual async Task<string> SetupAndDownloadString(string uri)
        {
            string responseText;
            uri += "&language="+ AppContext.CurrentLanguage;
            if (Session.CookieJar.Size() > 0)
            {
                //ws.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", Session.CookieJar.ToString());
                //ws.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(Constants.ApiFormat));
                //ws.DefaultRequestHeaders.TryAddWithoutValidation("ApiKey", AppContext.ApiKey);

                //res = await ws.GetStringAsync(uri);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, uri);
                request.Headers.Add("ApiKey", AppContext.ApiKey);
                request.Headers.Add("Accept", Constants.ApiFormat);
                request.Headers.Add("Cookie", Session.CookieJar.ToString());

                HttpResponseMessage response = await client.SendAsync(request);
                responseText = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                //The session can be invalid
                throw new AppException("0", ErrorMessages.InsufficientAuthorization);
            }

            return responseText;
        }

        private async Task<string> GetMessageBoxJSON(Constants.MMBType type, int skipSize, string url)
        {
            string res = null;

            if (type == Constants.MMBType.Messagebox)
            {
                //string param = string.Format("?language={0}&$filter=Status ne '{1}' and Status ne '{2}'&$skip={3}",
                // AppContext.CurrentLanguage, Lang.NO.Nob.Status.Sent, Lang.NO.Nob.Status.SentCorres, skipSize);
                //Without language
                string param = string.Format("?$filter=Status ne '{0}' and Status ne '{1}'&$skip={2}",
					Session.Sent, Session.SentCorres, skipSize);

                string request = url + param;

                res = await DownloadAndHandle(request);
            }
            else if (type == Constants.MMBType.Archive)
            {
                string param = string.Format("?$filter=Status eq '{0}' or Status eq '{1}'&$skip={2}",
					Session.Sent,Session.SentCorres, skipSize);
                string request = url + param;

                res = await DownloadAndHandle(request);
            }

            return res;
        }

        private async Task<string> DownloadAndHandle(string uri)
        {
            string res = string.Empty;

            try
            {
                res = await SetupAndDownloadString(uri);
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerException as WebException;

                //HttpClient wraps exceptions inside AggregateException
                if (inner != null)
                {
                    var interestingException = inner.Response as HttpWebResponse;
                    if (interestingException != null)
                    {
                        throw new AppException(ErrorMessages.ConnectionFailed,
                            interestingException.StatusCode.ToString());
                    }
                }
            }
            catch (WebException w)
            {
                var h = w.Response as HttpWebResponse;
                if (h != null)
                {
                    Logger.Logg("Failed to retrieve the message box due to network error: ", h.StatusCode);
                    throw new AppException(h.StatusCode.ToString(),
                        h.StatusDescription, h.ResponseUri.ToString().ToLower());
                }
            }
            catch (Exception ex)
            {
                Logger.Logg("Unknown network error occured: ", ex.Message);
                throw new AppException("0", ErrorMessages.ConnectionFailed);
            }

            return res;
        }

        internal async Task<string> GetProfileJSON()
        {
            string request = AppContext.ProfileURL;

            string res = await DownloadAndHandle(request);
            return res;
        }
    }
}
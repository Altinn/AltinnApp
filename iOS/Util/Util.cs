using System.Collections.Generic;
using System.Globalization;
using AltinnApp.Core.Models;
using AltinnApp.Core.Util;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace AltinnApp.iOS.Util
{
    /// <summary>
    /// Class used for common networking related tasks
    /// </summary>
	public partial class Util
    {
		private static CorePlatform.Translate _trans = new CorePlatform.Translate();

		public static void SetLanguage ()
		{
			Session.Sent = _trans.GetString("Message_Status_Sent");
 			Session.SentCorres = _trans.GetString("Message_Status_SentCorres");
//			if (AppContext.CurrentLanguage == 1044) {
//				Session.Sent = "Sendt";
//				Session.SentCorres = "Arkivert";
//			} else if (ppContext.CurrentLanguage == 1033) {
//				Session.Sent = "Sent";
//				Session.SentCorres = "Archived";
//			}else if (ppContext.CurrentLanguage == 2068){
//
//			};
		}

        public static LoginController Root { get; set; }

        private static readonly CorePlatform.Translate Trans = new CorePlatform.Translate();

        /// <summary>
        /// Show a pop-up alert
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public static void ShowAlert(int status, string message)
        {
            var prompt = new UIAlertView(status.ToString(CultureInfo.InvariantCulture), message, null, "OK")
            {
                AlertViewStyle = UIAlertViewStyle.Default
            };
            prompt.Show();
        }

        /// <summary>
        /// Create a new instance of NativeRequest, add the headers to the request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static NSMutableUrlRequest CreateNativeRequest(string url)
        {
            var request = new NSMutableUrlRequest();

            if (url == null)
            {
                Logger.Logg("Empty URL, could not create NSUrl");
            }
            else
            {
                NSUrl nsurl = NSUrl.FromString(url);
                request = new NSMutableUrlRequest(nsurl);

                if (url.Contains(AppContext.Domain))
                {
                    if (Session.LoggedIn)
                    {
                        request["Cookie"] = Session.CookieJar.ToString();
                        request["ApiKey"] = AppContext.ApiKey;
                    }
                }

                //If we have a IDporten cookie we add that as well (increased performance)
                if (Session.IDPortenCookie != null)
                {
                    request["IDPSecurityPortal"] = Session.IDPortenCookie.Value;
                }
            }

            return request;
        }

        /// <summary>
        /// Logout clears the cookies, we must explicitly delete the cookies in the cookie jar
        /// </summary>
        internal static void Logout()
        {
            Session.Logout();

            //Also need to clear the Cookie storage
            foreach (NSHttpCookie cookie in NSHttpCookieStorage.SharedStorage.Cookies)
            {
                NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);
            }

            NSUrlCache.SharedCache.RemoveAllCachedResponses();
        }

        internal static void ShowAlert(string message)
        {
            if (message == null)
            {
                message = string.Empty;
            }
            var prompt = new UIAlertView(string.Empty, message, null, "OK")
            {
                AlertViewStyle = UIAlertViewStyle.Default
            };
            prompt.Show();
        }

        /// <summary>
        /// Shows an alert with the "message", checks "levels" for required authentication levels and triggers a pop to
        /// root on the sender if "OK" is clicked
        /// </summary>
        /// <param name="message"></param>
        /// <param name="levels"></param>
        /// <param name="sender"></param>
        internal static void ShowAlert(string message, Dictionary<string, char> levels, UIViewController sender)
        {
            string method = string.Empty;

            switch (levels["required"])
            {
                case '2':
                    method = "MinID";
                    levels["required"] = '3';
                    break;
                case '3':
                    method = "MinID";
                    break;
                case '4':
                    method = "BankID";
                    break;
            }
            //Format the message and let the user decide to log in anew to raise the security level or just ignore it
            string choice = string.Format(Trans.GetString("Error_InsufficientAuthorization"), method);
            //var prompt = new UIAlertView("For lavt sikkerhetsnivå", choice, null, "OK", new string[] {"Avbryt"});
            var prompt = new UIAlertView("", choice, null, "OK", new[] {Trans.GetString("Misc_Cancel")});
            prompt.Clicked += (s, b) =>
            {
                //sender.NavigationController.PopViewControllerAnimated(true);
                //label1.Text = "Button " + b.ButtonIndex.ToString() + " clicked";
                //Console.WriteLine("Button " + b.ButtonIndex.ToString() + " clicked");
                if (b.ButtonIndex == 0)
                {
                    //The user clicked "OK" so we log out the session and pop to root
                    //Logout();

                    sender.NavigationController.PopViewControllerAnimated(false);

                    var url =
                        new NSUrl(AppContext.AppStartUrl + "?ApiDomain=" + AppContext.AppUrlSchema + "&ApiKey=" +
                                  AppContext.ApiKey + "&newAuthLevel=" + levels["required"]);
                    UIApplication.SharedApplication.OpenUrl(url);
                    AppContext.LoginMethod = 2; //External browser method
                }
                else
                {
                    //Otherwise we just pop to the parent container since the message now does not contain any usefull data
                    sender.NavigationController.PopViewControllerAnimated(true);
                }
            };
            prompt.Show();
        }

        /// <summary>
        /// Handles an AppException sent from a UIVewController. This method is mainly used to raise the security level of users if
        /// they try to read a message for which they have too low security level
        /// </summary>
        /// <param name="app"></param>
        /// <param name="controller"></param>
        public static void HandleAppException(AppException app, UIViewController controller)
        {
            string mess = string.Empty;

            //A known exception occured, we want to handle that appropiately
            if (app.HttpStatusCode == "Unauthorized")
            {
                if (app.Description != null)
                {
                    Dictionary<string, char> levels = Core.Util.Util.GetAutLevel(app.Description);


                    if (levels.Count > 0)
                    {
                        string localizedAuthMessage = Trans.GetString("Misc_Cancel");
                        mess = string.Format(localizedAuthMessage, levels["current"],
                            levels["required"]);
                        ShowAlert(mess, levels, controller);
                    }
                    else if (app.Description.ToLower().Contains("unable to retrieve pdf for this message"))
                    {
                        mess = Trans.GetString("Error_PrintError");
                        ShowAlert(mess);
                        controller.NavigationController.PopViewControllerAnimated(true);
                    }
                }
                else
                {
                    ShowAlert(Trans.GetString("Error_GeneralError"));
                }
            }
            else if (app.HttpStatusCode == "InternalServerError")
            {
                if (app.Description != null && app.Description.ToLower().Contains("unable to retrieve PDF"))
                {
                    mess = Trans.GetString("Error_PrintError");
                }
                else if (app.Response != null && app.Response.Contains("login"))
                {
                    mess = Trans.GetString("Error_TimedOut");
                    Logout();
                    controller.NavigationController.PopToRootViewController(true);
                }
                else
                {
                    //if (app.ErrorType == ErrorMessages.RemoteMessageError)
                    mess = Trans.GetString("Error_MessageInternalServerError");
                    //controller.NavigationController.PopViewControllerAnimated(true);
                }
                ShowAlert(mess);
            }
            else
            {
                if (app.Description == ErrorMessages.PrintFailed)
                {
                    Logger.Logg("Failed to process the attachment on the client: " + app.Message);
                    mess = Trans.GetString("Error_PrintError");
                    controller.NavigationController.PopViewControllerAnimated(true);
                }
                else if (app.Description == ErrorMessages.ConnectionFailed)
                {
                    Logger.Logg("The connection failed on the client side: " + app.Message);
                    mess = Trans.GetString("Error_GeneralError");
                }
                else
                {
                    Logger.Logg("Something went wrong on the cient side: " + app.Message);
                    mess = Trans.GetString("Error_GeneralError");
                }

                ShowAlert(mess);
            }
        }
    }
}
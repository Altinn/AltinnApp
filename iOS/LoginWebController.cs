using System;
using System.Collections.Generic;
using AltinnApp.Core.Models;
using AltinnApp.Core.Util;
using AltinnApp.iOS.Util;
using BigTed;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SWRevealViewControllerBinding;

namespace AltinnApp.iOS
{
    /// <summary>
    /// Class for handling login with IDPorten, this class gets and stores the cookies needed to connect to the API later on
    /// </summary>
    public partial class LoginWebController : UIViewController
    {
        private CorePlatform.Translate _trans;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="handle"></param>
        public LoginWebController(IntPtr handle)
            : base(handle)
        {
            NSHttpCookieStorage.SharedStorage.AcceptPolicy = NSHttpCookieAcceptPolicy.Always;
        }

        public override void ViewDidLoad()
        {
            _trans = new CorePlatform.Translate();
            base.ViewDidLoad();
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            var cookies = NSHttpCookieStorage.SharedStorage.CookiesForUrl(new NSUrl(new NSUrl(AppContext.Domain).Host));
            foreach (NSHttpCookie cookie in cookies)
            {
                NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);
            }
        }

        private void Localize()
        {
            _headerLabel.Text = _trans.GetString("LoginController_Loginbutton");
        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            //webview.LoadStarted += LoadStartHandler;
            webview.ShouldStartLoad = ShouldStartHandler;

            var cookies = NSHttpCookieStorage.SharedStorage.CookiesForUrl(new NSUrl(new NSUrl(AppContext.Domain).Host));
            foreach (NSHttpCookie cookie in cookies)
            {
                NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);
            }

            //Create the native request
            try
            {
                BTProgressHUD.Show();
                if (Reachability.IsHostReachable(AppContext.Domain))
                {
                    NSMutableUrlRequest theRequest = Util.Util.CreateNativeRequest(AppContext.LoginUrl);

                    int status;
                    NSUrlConnection.FromRequest(theRequest, new NativeUrlDelegate((body, httpStatus) =>
                    {
                        webview.LoadRequest(theRequest);
                        status = httpStatus;
                        // ReSharper disable once ImplicitlyCapturedClosure
                    }, delegate(string reason, int httpStatus)
                    {
                        Logger.Logg("LoginIDPortenWebController:Failed to connect because: ", reason);
                        status = httpStatus;
                        Util.Util.ShowAlert(status, ErrorMessages.ConnectionFailed);
                        BTProgressHUD.Dismiss();
                    }));

                    //View started loading the request
                    // ReSharper disable once ConvertToLambdaExpression
                    webview.LoadStarted += (sender, e) => { BTProgressHUD.Show(); };

                    // ReSharper disable once ConvertToLambdaExpression
                    webview.LoadFinished += (sender, e) => { BTProgressHUD.Dismiss(); };
                }
                else
                {
                    Util.Util.ShowAlert(ErrorMessages.NoNetworkAvailable);
                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception e)
            {
                Logger.Logg("Failed to open the webview: " + e);
                Util.Util.ShowAlert(ErrorMessages.ConnectionFailed);
                BTProgressHUD.Dismiss();
            }
            Localize();
        }

        /// <summary>
        /// Pop to the previous screen, clear any cookies and webviews
        /// </summary>
        /// <param name="sender"></param>
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void GoBack(NSObject sender)
        {
            webview.Dispose();
            webview = new UIWebView();
            NavigationController.PopViewControllerAnimated(true);
        }

        private bool ShouldStartHandler(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navType)
        {
            var cookies1 = NSHttpCookieStorage.SharedStorage.CookiesForUrl(new NSUrl(new NSUrl(AppContext.Domain).Host));
            foreach (NSHttpCookie cookie in cookies1)
            {
                NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);
            }

            //NSHttpCookie aspSessionCookie = null;
            //Get the IDPorten Cookie needed for SvarUT and similar federated sources
            if (request.Url.ToString().ToLower().Contains("bankid"))
            {
                Util.Util.ShowAlert(ErrorMessages.BankIDNotSupported);
                return false;
            }

            if (request.Url.ToString().Contains(AppContext.IDPortenUrl))
            {
                var cookies = NSHttpCookieStorage.SharedStorage.CookiesForUrl(request.Url);

                foreach (NSHttpCookie cookie in cookies)
                {
                    if (cookie.Name == AppContext.IDPortenCookieName)
                    {
                        Session.IDPortenCookie = new Cookie
                        {
                            Name = cookie.Name,
                            Value = cookie.Value
                        };
                    }
                }
            }

            //We know that the cookies are not present if we are stil at IDPorten, but the Altinn Cookies might be present
            if (request.Url.ToString().Contains(AppContext.LoginUrl))
            {
                var cookies = new List<NSHttpCookie>();
                cookies.AddRange(NSHttpCookieStorage.SharedStorage.CookiesForUrl(request.Url));

                //Parse the cookiejar and get the cookies we need
                foreach (NSHttpCookie cookie in cookies)
                {
                    Session.CookieJar.Add(new Cookie
                    {
                        Name = cookie.Name,
                        Value = cookie.Value
                    });

                    //It's very important that the name of the auth cookie is correct, otherwise the webview will hang
                    if (cookie.Name.ToLower() == AppContext.AuthCookieName)
                    {
                        //Set the flag to indicate we are logged in, it does not matter if the user is actually logged in security wise
                        //but we want to be explicit about it
                        Session.LoggedIn = true;
                    }
                }

                //As soon as the cookies are set we can move to the next view and load the data
                if (Session.LoggedIn)
                {
                    //Example of how to store the cookie locally since the cookie is valid for 30 minutes
                    //It is important to properly secure this information otherwise the cookie can be stolen and abused

                    var flyoutController = Storyboard.InstantiateViewController("FlyoutRoot") as SWRevealViewController;
                    if (flyoutController != null)
                    {
                        NavigationController.PushViewController(flyoutController, true);
                    }

                    //                    var messageboxController =
                    //                        Storyboard.InstantiateViewController("MessageboxIdentifier") as MessageboxController;
                    //                    if (messageboxController != null)
                    //                    {
                    //                        messageboxController.Type = Constants.MMBType.Messagebox;
                    //                        NavigationController.PushViewController(messageboxController, true);
                    //                    }
                }
                //                else
                //                {
                //                    //Parse the cookiejar and get the cookies we need
                //                    foreach (NSHttpCookie cookie in cookies)
                //                    {
                //                        NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);
                //                    }
                //                }
            }
            return true;
        }
    }
}
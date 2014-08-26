using System;
using AltinnApp.Core.Models;
using AltinnApp.Core.Util;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SWRevealViewControllerBinding;

namespace AltinnApp.iOS
{
    /// <summary>
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    /// </summary>
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        private static readonly UIStoryboard Storyboard = UIStoryboard.FromName("MainStoryboard_iPhone", null);

        // class-level declarations
        public override UIWindow Window { get; set; }


        /// <summary>ß
        /// The app finished the launching stage
        /// </summary>
        /// <param name="application"></param>
        /// <param name="launcOptions"></param>
        /// <returns></returns>
        public override bool FinishedLaunching(UIApplication application, NSDictionary launcOptions)
        {
            // Override point for customization after application launch.
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
				UIStoryboard.FromName("MainStoryboard_iPhone", null);
            }
            else if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            {
                UIStoryboard.FromName("MainStoryboard_iPhone", null);
            }

            //Load the settings, mainly which environment to use. 
            //This should be turned off for App Store release since users only connect to the production environment
            AppContext.SelectedEnvironment = AppContext.EnvironmentType.PROD;

            Enum.TryParse(NSUserDefaults.StandardUserDefaults.StringForKey("Environment"), true,
                out AppContext.SelectedEnvironment);

            string selection = NSUserDefaults.StandardUserDefaults.StringForKey("LoginUseExternalBrowser");
            AppContext.UseExternalBrowser = selection == "1";

            Session.CookieJar = new CookieJar();
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);


            return true;
        }


        /// <summary>
        /// Activated whenever the app returns to focus from the background
        /// </summary>
        /// <param name="app"></param>
        public override void OnActivated(UIApplication app)
        {
            //try
            //{
            //    //If the user is logged in we continue directly to the HomeScreen
            //    if (Session.LoggedIn)
            //    {
            //        UIViewController con = Window.RootViewController;
            //        var top = con as UINavigationController;

            //        if (top != null)
            //            foreach (UIViewController vc in top.ViewControllers)
            //            {
            //                if (vc is SWRevealViewController)
            //                {
            //                    top.PushViewController(vc, false);
            //                }
            //            }
            //                                    //var flyoutController = Storyboard.InstantiateViewController("FlyoutRoot") as SWRevealViewController;

            //                                    //if (top != null)
            //                                    //{
            //                                    //    top.PushViewController(flyoutController, false);
            //                                    //}
            //                                }
            //    else if (Session.DidLogOut)
            //    {
            //        //else we show the login screen. We must keep track of the "didLogOut" variable to avoid looping back to
            //        //the home screen everytime the app gets focus
            //        Session.DidLogOut = false;
            //        UIViewController con = Window.RootViewController;
            //        var top = con as UINavigationController;

            //        if (top != null)
            //        {
            //            top.PopToRootViewController(false);
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    Logger.Logg("AppDelegate: failed to properly switch to the HomeScreen:" + e);
            //    Util.Util.ShowAlert(ErrorMessages.NOB.CriticalError);
            //}
        }

        /// <summary>
        /// Activated when the app is called by it's URL schema (defined in Info.plist).
        /// We want to retrieve the parameters since it contain the session to use sent from Safari
        /// </summary>
        /// <param name="application"></param>
        /// <param name="url"></param>
        /// <param name="sourceApplication"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            try
            {
                if (url.Query != null && url.Query.Contains(AppContext.SessionParameter))
                {
                    //Get the value of the ?sid=XXX field
                    int startIndex = url.Query.IndexOf("=", StringComparison.Ordinal) + 1;
                    string cookie = url.Query.Substring(startIndex);

                    //Add the auth cookie to the Cookiejar

                    if (!string.IsNullOrEmpty(Session.CookieJar.AspxAuthCookie()))
                    {
                        Session.CookieJar.Update(AppContext.AuthCookieName, cookie);
                        Session.LoggedIn = true;
                    }
                    else
                    {
                        Session.CookieJar.Add(new Cookie {Name = AppContext.AuthCookieName, Value = cookie});
                        Session.LoggedIn = true;
                        if (Session.LoggedIn)
                        {
                            UIViewController con = Window.RootViewController;
                            var top = con as UINavigationController;

                            if (top != null)
                                foreach (UIViewController vc in top.ViewControllers)
                                {
                                    if (vc is SWRevealViewController)
                                    {
                                        top.PushViewController(vc, false);
                                    }
                                    else
                                    {
                                        var flyoutController =
                                            Storyboard.InstantiateViewController("FlyoutRoot") as SWRevealViewController;

                                        top.PushViewController(flyoutController, false);
                                    }
                                }
                        }
                    }
                }
                else if (url.Query != null && url.Query.Contains(AppContext.LoggedOutParameter))
                {
                    Session.DidLogOut = true;
                    Session.LoggedIn = false;

                    if (Session.DidLogOut)
                    {
                        //else we show the login screen. We must keep track of the "didLogOut" variable to avoid looping back to
                        //the home screen everytime the app gets focus
                        Session.DidLogOut = false;
                        UIViewController con = Window.RootViewController;
                        var top = con as UINavigationController;

                        if (top != null)
                        {
                            top.PopToRootViewController(false);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Logg("Failed to open the URL: " + e);
            }

            return true;
        }

        // This method is invoked when the application is about to move from active to inactive state.
        // OpenGL applications should use this method to pause.
        public override void OnResignActivation(UIApplication application)
        {
        }

        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
        }

        // This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground(UIApplication application)
        {
        }

        // This method is called when the application is about to terminate. Save data, if needed. 
        public override void WillTerminate(UIApplication application)
        {
        }
    }
}
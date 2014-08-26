using System;
using AltinnApp.Core.Models;
using BigTed;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SWRevealViewControllerBinding;

namespace AltinnApp.iOS
{
    /// <summary>
    /// Class for displaying the first info screen and decide login
    /// </summary>
    public partial class LoginController : UIViewController
    {
        private LoginWebController _loginIdPortenWebController;
        private CorePlatform.Translate _trans;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="handle"></param>
        public LoginController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            _trans = new CorePlatform.Translate();

            _loginIdPortenWebController =
                Storyboard.InstantiateViewController("LoginWebController") as LoginWebController;


            OpenLanguage.TouchUpInside += (sender, e) =>
            {
                var prompt = new UIAlertView(string.Empty, _trans.GetString("Popup_SelectLanguage"), null,
                    _trans.GetString("Misc_Cancel"),
                    new[]
                    {
                        _trans.GetString("Misc_Lang_Bokmaal"), _trans.GetString("Misc_Lang_Nynorsk"),
                        _trans.GetString("Misc_Lang_English")
                    })
                {
                    AlertViewStyle = UIAlertViewStyle.Default
                };


                prompt.Clicked += (s, b) =>
                {
                    Localize();
                    if (b.ButtonIndex == 0)
                    {
                    }
                    else if (b.ButtonIndex == 1)
                    {
                        AppContext.CurrentCulture = "nb-NO";
                        AppContext.CurrentLanguage = "1044";
                    }
                    else if (b.ButtonIndex == 2)
                    {
                        AppContext.CurrentCulture = "nn-NO";
                        AppContext.CurrentLanguage = "2068";
                    }
                    else if (b.ButtonIndex == 3)
                    {
                        AppContext.CurrentCulture = "en";
                        AppContext.CurrentLanguage = "1033";
                    }
					Util.Util.SetLanguage();
                    Localize();
                };
                prompt.Show();
            };

            //Add this as an observer of the Settings
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("NSUserDefaultsDidChangeNotification"),
                // ReSharper disable once RedundantLambdaParameterType
                (NSNotification obj) =>
                {
                    NSUserDefaults.StandardUserDefaults.Synchronize();
                    LoadSettings();
                });

            Util.Util.Root = this;

            AppContext.hasLaunched = NSUserDefaults.StandardUserDefaults.BoolForKey("hasLaunched");

            if (!AppContext.hasLaunched)
            {
                NSUserDefaults.StandardUserDefaults.SetBool(true, "hasLaunched");
            }
        }

        private void Localize()
        {
            _IntroText.Text = _trans.GetString("LoginController_Infobox");
            _LoginButton.SetTitle(_trans.GetString("LoginController_Loginbutton"), UIControlState.Normal);
            _LoginButton.SetTitle(_trans.GetString("LoginController_Loginbutton"), UIControlState.Highlighted);
            OpenLanguage.SetTitle("  " + _trans.GetString("LoginController_CurrentLanguage"), UIControlState.Normal);
            OpenLanguage.SetTitle("  " + _trans.GetString("LoginController_CurrentLanguage"), UIControlState.Highlighted);
        }

        /// <summary>
        /// The ViewWillAppear runs everytime the view refreshes (ViewDidLoad runs once)
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            LoadSettings();
            Localize();
            BTProgressHUD.Dismiss();
        }

        //Loads the settings
        private void LoadSettings()
        {
            //Load the settings, mainly which environment to use, by default Production
            AppContext.SelectedEnvironment = AppContext.EnvironmentType.PROD;

            //Try to get the selected environment from the settings
            Enum.TryParse(NSUserDefaults.StandardUserDefaults.StringForKey("Environment"), true,
                out AppContext.SelectedEnvironment);

            AppContext.SetDomain(AppContext.SelectedEnvironment);
            string selection = NSUserDefaults.StandardUserDefaults.StringForKey("LoginUseExternalBrowser");
            AppContext.UseExternalBrowser = selection == "1";
        }

        /// <summary>
        /// Process the click of the "login" button
        /// </summary>
        /// <param name="sender">Sender.</param>
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void Login(NSObject sender)
        {
            //Check if the user is logged in
            if (Session.LoggedIn)
                //if (true)
            {
                //if (true)
                var flyoutController = Storyboard.InstantiateViewController("FlyoutRoot") as SWRevealViewController;
                if (flyoutController != null)
                {
                    NavigationController.PushViewController(flyoutController, true);
                }
            }
            else
            {
                //Otherwise show the loginscreen
                //The environments where the new BankID solution is not deployed we use with integrated webview
                //if (!AppContext.UseExternalBrowser)
                if (false)
                {
                    //As long as AppStart is in a environments we use it, at least until BankID 2.0 i live
                    BTProgressHUD.Show();
                    NavigationController.PushViewController(_loginIdPortenWebController, true);
                    AppContext.LoginMethod = 1; //Integrated browser method
                }
                    //else if (AppContext.UseExternalBrowser)
                else if (true)
                {
                    //AppContext.FirstTimeStarting = true;

                    if (!AppContext.hasLaunched)
                    {
                        //The first time we start we show the infoscreen
                        var infoScreenController =
                            Storyboard.InstantiateViewController("InfoscreenController") as UIViewController;
                        if (infoScreenController != null)
                        {
                            NavigationController.PresentViewController(infoScreenController, true, null);
                        }
                    }
                    else
                    {
                        //For the environments with the new BankID solution we use the new BankID solution
                        //which is to use AppStart.aspx as a proxy for the login, we need to send along this app's URL schema and the APIKey
                        var url =
                            new NSUrl(AppContext.AppStartUrl + "?ApiDomain=" + AppContext.AppUrlSchema + "&ApiKey=" +
                                      AppContext.ApiKey);
                        UIApplication.SharedApplication.OpenUrl(url);
                        AppContext.LoginMethod = 2; //External browser method
                        // BTProgressHUD.Show();
                    }
                }
            }
        }
    }
}
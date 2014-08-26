using System;
using BigTed;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SWRevealViewControllerBinding;
using AltinnApp.Core.Models;
using AltinnApp.Core.Util;

namespace AltinnApp.iOS
{
    public partial class FlyoutController : UIViewController
    {
        private CorePlatform.Translate _trans;

        public FlyoutController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //var tableView = _tableView;
            if (_tableView != null)
            {
                _tableView.Source = new FlyoutTablesource();
				_tableView.BackgroundColor = UIColor.FromRGB (108, 108, 108);
				this.View.BackgroundColor = UIColor.FromRGB (108, 108, 108);
            }

            if (Session.Organizations != null && Session.Organizations.Count == 1)
            {
                _actorLabel.Hidden = true;
                _actorForwardIcon.Hidden = true;
            }
            _trans = new CorePlatform.Translate();
        }

        private void Localize()
        {
            //If we have English, add some more spaces to the label
            if (AppContext.CurrentLanguage == "1033")
            {
                _actorLabel.SetTitle(_trans.GetString("FlyoutController_Actor"), UIControlState.Normal);
                _actorLabel.SetTitle(_trans.GetString("FlyoutController_Actor"), UIControlState.Selected);
            }
            else
            {
                _actorLabel.SetTitle(_trans.GetString("FlyoutController_Actor"), UIControlState.Normal);
                _actorLabel.SetTitle(_trans.GetString("FlyoutController_Actor"), UIControlState.Selected);
            }


            _logoutButton.SetTitle(_trans.GetString("FlyoutController_LogoutButton"), UIControlState.Normal);
            _logoutButton.SetTitle(_trans.GetString("FlyoutController_LogoutButton"), UIControlState.Selected);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            //_tableView.UpdateConstraints ();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void WillMoveToParentViewController(UIViewController parent)
        {
            base.WillMoveToParentViewController(parent);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            BTProgressHUD.Dismiss();
            if (Session.Organizations != null && Session.Organizations.Count == 1)
            {
                _actorLabel.Hidden = true;
                _actorForwardIcon.Hidden = true;
            }

            if (Session.SelectedOrg != null && Session.Profile != null)
            {
                if (Session.SelectedOrg.Name == Session.Profile.Name)
                {
                    _nameLabel.Text = Session.Profile.FirstName;
                    _lastNameLabel.Text = Session.Profile.LastName;
                }
                else
                {
                    _nameLabel.Text = Session.SelectedOrg.Name;
                    _lastNameLabel.Text = Session.SelectedOrg.OrganizationNumber;
                }
            }


            Localize();
            _tableView.ReloadData();
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void _logout(NSObject sender)
        {
            //Clear all the cookies and user data
            Util.Util.Logout();

            //Logout method 2 with external browser (required for BankID)
            if (AppContext.LoginMethod == 2)
            {
                var url =
                    new NSUrl(AppContext.AppStopUrl + "?logout=true&appUrl=" + AppContext.AppUrlSchema + "&appKey=" +
                              AppContext.ApiKey);
                UIApplication.SharedApplication.OpenUrl(url);
            }
            else if (AppContext.LoginMethod != 1)
            {
                //Logout method 1 with the internal browser
                //this.RevealViewController ().PerformSegue("Logout", this);
                DismissViewController(true, null);
                //this.RevealViewController ().NavigationController.PopToViewController(Util.Util.Root, true);
            }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            var segueReveal = segue as SWRevealViewControllerSegue;
            if (segueReveal == null)
            {
                return;
            }
            var destinationController = segueReveal.DestinationViewController;
            if (destinationController != null && sender is UITableViewCell)
            {
                var senderCell = sender as UITableViewCell;
                if (senderCell.ReuseIdentifier == "Inbox")
                {
                    //destinationController.LabelContent = "2";
                    //destinationController.ViewColor = UIColor.Green;
                    ((MessageboxController) destinationController).MMBType = Constants.MMBType.Messagebox;
                }
                else if (senderCell.ReuseIdentifier == "Archive")
                {
                    ((MessageboxController) destinationController).MMBType = Constants.MMBType.Archive;
                }
                else if (senderCell.ReuseIdentifier == "Settings")
                {
                }
                else if (senderCell.ReuseIdentifier == "Contact")
                {
                }
                else if (senderCell.ReuseIdentifier == "Feedback")
                {
                }
            }
            //this.RevealViewController().SetFrontViewController(segueReveal.DestinationViewController, true);
//			this.RevealViewController().SetFrontViewController(segueReveal.DestinationViewController, false);
//			this.RevealViewController().SetFrontViewPosition(FrontViewPosition.LeftSide, true);
            this.RevealViewController().SetFrontViewController(segueReveal.DestinationViewController, false);
            this.RevealViewController().SetFrontViewPosition(FrontViewPosition.LeftSideMostRemoved, true);
        }
    }
}
using System;
using AltinnApp.Core.Models;
using AltinnApp.Core.Util;
using AltinnApp.iOS.CorePlatform;
using BigTed;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SWRevealViewControllerBinding;

namespace AltinnApp.iOS
{
    /// <summary>
    /// This class controls the display and selection of reportees
    /// </summary>
    public partial class ReporteeController : UIViewController
    {
        private Translate _trans;
        private bool _once = true;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="handle"></param>
        public ReporteeController(IntPtr handle)
            : base(handle)
        {
            //_service = new PlattformService();
        }

        /// <summary>
        /// Creates a new ReporteeController
        /// </summary>
        public ReporteeController()
        {
        }


        private void Localize()
        {
            _header.Text = _trans.GetString("ReporteeController_Header");
            _innboxLabel.SetTitle(_trans.GetString("MessageboxController_InboxHeader"), UIControlState.Normal);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            tableView.WeakDataSource = tableView.WeakDelegate = this;

            _goBack.TouchUpInside += (sender, e) => this.RevealViewController().RevealToggleAnimated(true);

            _innboxLabel.TouchUpInside += (sender, e) =>
            {
                try
                {
                    var controller =
                        Storyboard.InstantiateViewController("MessageboxIdentifier") as MessageboxController;
                    if (controller != null)
                    {
                        controller.MMBType = Constants.MMBType.Messagebox;
                        this.RevealViewController().SetFrontViewController(controller, true);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Logg(ex);
                }
            };

            _trans = new Translate();
            View.AddGestureRecognizer(this.RevealViewController().PanGestureRecognizer);
            //Make sure the fields are blanked out
            _searchField.Placeholder = _trans.GetString("Misc_SearchboxPlaceholder");
            _searchField.TextChanged += SearchTextChanged;
            _searchField.OnEditingStopped += SearchStopped;
            _searchField.OnEditingStarted += SearchStarted;
            _searchField.CancelButtonClicked += CancelButtonClicked;
            _searchField.SearchButtonClicked += SearchButtonClicked;

            Localize();
        }


        /// <summary>
        /// Triggered each time the text in the search filed changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextChanged(object sender, UISearchBarTextChangedEventArgs e)
        {
            Session.SearchActor(e.SearchText);
            tableView.ReloadData();
        }


        private async void SearchButtonClicked(object sender, EventArgs e)
        {
            string searchValue = _searchField.Text;
            _searchField.ResignFirstResponder();

            BTProgressHUD.Show();
            Session.Organizations = await PlattformService.Service.SearchActorRemote(searchValue);

            tableView.ReloadData();
            BTProgressHUD.Dismiss();
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            _searchField.Text = string.Empty;
            Session.Organizations = Session.UnfilteredOrganizations;
        }

        private void SearchStarted(object sender, EventArgs e)
        {
        }

        private void SearchStopped(object sender, EventArgs e)
        {
        }

        [Export("scrollViewDidScroll:")]
        public async void Scrolled2(UIScrollView scrollView)
        {
            try
            {
                if (Session.Organizations.Count >= 50 && _once)
                {
                    if ((tableView.ContentOffset.Y) >= (tableView.ContentSize.Height - tableView.Bounds.Size.Height + 40))
                    {
                        _once = false;
                        if ((Session.Organizations.Count % 50) == 0)
                        {
                            //If we have a messagebox size modulo 50 = 0 then we know we reached the max size for one call and have to check
                            //if there are any more messages with a skip starting at the last element in the messagebox
                            var mess = await PlattformService.Service.PopulateOrganizations(Session.Organizations.Count);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Logg("Failed to retrieve more than 50 organizations");
            }
        }

        [Export("tableView:numberOfRowsInSection:")]
        public int RowsInSection(UITableView tableview, int section)
        {
            if (Session.Organizations != null)
            {
                return Session.Organizations.Count;
            }
            return 0;
        }

        [Export("tableView:cellForRowAtIndexPath:")]
        // ReSharper disable once ParameterHidesMember
        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            const string cellName = "aCell";
            var cell = tableView.DequeueReusableCell(cellName) ??
                       new UITableViewCell(UITableViewCellStyle.Subtitle, cellName);
            if (Session.Organizations != null)
            {
                Organization org = Session.Organizations[indexPath.Row];
                if (org.OrganizationNumber == "my")
                {
                    cell.TextLabel.Text = org.Name;
                }
                else
                {
                    cell.TextLabel.Text = org.OrganizationNumber + " - " + org.Name;
                }


                if (org.OrganizationNumber == Session.SelectedOrg.OrganizationNumber)
                {
                    try
                    {
                        cell.AccessoryView = new UIImageView(UIImage.FromBundle("Infoscreen-Grey_checked.png"));
                    }
                    catch (Exception ex)
                    {
                        Logger.Logg("Failed to set cell background color" + ex);
                    }
                }
                else
                {
                    cell.AccessoryView = null;
                }
            }

            var backgroundView = new UIView(cell.Frame) {BackgroundColor = UIColor.FromRGB(229, 225, 156)};
            cell.SelectedBackgroundView = backgroundView;

            return cell;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        // ReSharper disable once ParameterHidesMember
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //When a reportee is selected, make it so in the AppContext
            //null out the filter when we change reportee
            if (Session.Organizations != null)
            {
                var selected = Session.Organizations[indexPath.Row];

                Session.SelectedOrg = selected;
            }
            tableView.ReloadData();
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void GoBack(NSObject sender)
        {
            //NavigationController.PopViewControllerAnimated(true);
        }
    }
}
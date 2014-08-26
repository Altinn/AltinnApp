using System;
using System.Collections.Generic;
using System.Globalization;
using AltinnApp.Core.Models;
using AltinnApp.Core.Util;
using AltinnApp.iOS.CorePlatform;
using BigTed;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SWRevealViewControllerBinding;
using System.Threading.Tasks;

namespace AltinnApp.iOS
{
    /// <summary>
    /// This class holds the model of the message box, the messagebox (innbox) or the archive
    /// Nothing is stored on the client side in this reference implementation, it is held in memory and will be lost when the app exits
    /// Wheter or not to store data locally for caching is a design consideration, if anything is stored it must be protected with for example encryption
    /// </summary>
    public partial class MessageboxController : UIViewController
    {
        public readonly MessageboxController Controller;
        private Constants.MMBType _type;
        private const string CorrespondenceUnreadCell = "CorrespondenceUnread";
        private const string CorrespondenceReadCell = "CorrespondenceRead";
        //At the time no difference in looks between "read" and "unread" forms, but that might change so we want the plumbing in place
        private const string FormUnread = "FormUnread";
        private const string FormRead = "FormRead";
        private const string MessageCell = "MessageCell";
        private Translate _trans;
        private bool _once = true;
        private string _headerString;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="handle"></param>
        public MessageboxController(IntPtr handle)
            : base(handle)
        {
            Controller = this;

            _type = Constants.MMBType.Messagebox;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageboxController()
        {
            Controller = this;
        }

        public Constants.MMBType MMBType
        {
            get { return _type; }
            set { _type = value; }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            //BTProgressHUD.Dismiss();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _headerLabel.Text =
                _trans.GetString(_type == Constants.MMBType.Archive
                    ? "MessageboxController_ArchiveHeader"
                    : "MessageboxController_InboxHeader");
            _headerString = _headerLabel.Text;
            Localize();
        }

//        public override bool ShouldAutorotate()
//        {
//            return false;
//        }
//
//        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
//        {
//            return UIInterfaceOrientationMask.Portrait;
//        }

        private void Localize()
        {
            //Inbox or Archive label set in VewWillAppear
            _SearchField.Placeholder = _trans.GetString("Misc_SearchboxPlaceholder");
        }

        /// <summary>
        /// The ViewDidLoad triggers every first time the view gets started (not in the view stack), if it's
        /// already on the view stack it uses that one and goes straigh to ViewWillAppear and ViewDidAppear
        /// </summary>
        public override async void ViewDidLoad()
        {
            BTProgressHUD.Show();

            _trans = new Translate();
            //Make sure the fields are blanked out

            _SearchField.TextChanged += SearchTextChanged;
            _SearchField.OnEditingStopped += SearchStopped;
            _SearchField.OnEditingStarted += SearchStarted;
            _SearchField.CancelButtonClicked += CancelButtonClicked;
            _SearchField.SearchButtonClicked += SearchButtonClicked;
            //tableView.Scrolled += DidScroll;

            tableView.WeakDataSource = tableView.WeakDelegate = this;

            //Reset the messagebox to load new inbox or archive
            Session.mmb = new Messagebox();

            if (this.RevealViewController() != null)
            {
                _goBack.TouchUpInside += (sender, e) => this.RevealViewController().RevealToggleAnimated(true);
                this.RevealViewController().RearViewRevealWidth =
                    (float) (UIScreen.MainScreen.Bounds.Width - UIScreen.MainScreen.Bounds.Width*0.15);
                View.AddGestureRecognizer(this.RevealViewController().PanGestureRecognizer);
            }

            Localize();

            try
            {
                var mess = await LoadMessagebox(0);
            }
            catch (AppException app)
            {
                Util.Util.HandleAppException(app, this);
                Logger.Logg("Connection failed when loading messagebox: " + app.Message);
            }
            catch (Exception ex)
            {
                Logger.Logg("Failed to load the messagebox: " + ex);
                Util.Util.ShowAlert(ErrorMessages.GeneralError);
            }
        }

//        private void DidScroll(object sender, EventArgs e)
//        {
//        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            //Check which segue we shall use and set the data in the next viewscreen
            if (segue.Identifier == "GoToCorrespondence")
            {
                var view = (CorrespondenceViewController) segue.DestinationViewController;
                view.CurrentMessage = Session.mmb.CurrentMessage;
                view.Type = _type;
            }
            else if (segue.Identifier == "GoToForm")
            {
                var view = (FormViewController) segue.DestinationViewController;
                view.CurrentMessage = Session.mmb.CurrentMessage;
                view.Type = _type;
            }
        }

        [Export("tableView:numberOfRowsInSection:")]
        public int RowsInSection(UITableView tableview, int section)
        {
            if (Session.mmb != null)
            {
                return Session.mmb.Messages.Count;
            }
            return 0;
        }

        [Export("scrollViewDidScroll:")]
        public async void Scrolled2(UIScrollView scrollView)
        {
            if (Session.mmb.Messages.Count >= 50 && _once)
            {
                if ((tableView.ContentOffset.Y) >= (tableView.ContentSize.Height - tableView.Bounds.Size.Height + 40))
                {
                    _once = false;
                    if ((Session.mmb.Messages.Count%50) == 0)
                    {
                        //If we have a messagebox size modulo 50 = 0 then we know we reached the max size for one call and have to check
                        //if there are any more messages with a skip starting at the last element in the messagebox
                        var mess = await LoadMessagebox(Session.mmb.Messages.Count);
                    }
                }
            }
        }

        [Export("tableView:cellForRowAtIndexPath:")]
        // ReSharper disable once ParameterHidesMember
        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // in a Storyboard, Dequeue will ALWAYS return a cell, 
            Message mess = Session.mmb.Messages[indexPath.Row];
            string cellIdentifier = MessageCell;

            if (mess.Type == "Correspondence")
            {
                if (mess.Status == Constants.NO.Nob.Status.Unread || mess.Status == Constants.EN.Status.Unread)
                {
                    cellIdentifier = CorrespondenceUnreadCell;
                }
                else
                {
                    cellIdentifier = CorrespondenceReadCell;
                }
            }
            else if (mess.Type == "FormTask")
            {
                if (mess.Status == "Unread" || mess.Status == "Ulest")
                {
                    cellIdentifier = FormUnread;
                }
                else
                {
                    cellIdentifier = FormRead;
                }
            }
            var cell = tableView.DequeueReusableCell(cellIdentifier) as MessageCell ?? new MessageCell(cellIdentifier);
            // now set the properties as normal
            cell.UpdateCell(mess.ServiceOwner, mess.Subject,
                mess.LastChangedDateTime.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));
            try
            {
                var backgroundView = new UIView(cell.Frame) {BackgroundColor = UIColor.FromRGB(229, 225, 156)};
                cell.SelectedBackgroundView = backgroundView;
            }
            catch (Exception ex)
            {
                Logger.Logg("Failed to set cell background color" + ex);
            }
            return cell;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        // ReSharper disable once ParameterHidesMember
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _SearchField.ResignFirstResponder();
            Session.mmb.CurrentMessage = Session.mmb.Messages[indexPath.Row];

            if (Session.mmb.CurrentMessage.Type == Constants.MessageType.Correspondence.ToString())
            {
                Controller.PerformSegue("GoToCorrespondence", Controller);
            }
            else if (Session.mmb.CurrentMessage.Type == Constants.MessageType.FormTask.ToString())
            {
                Controller.PerformSegue("GoToForm", Controller);
            }
            tableView.DeselectRow(indexPath, false);
        }

        /// <summary>
        /// Triggered each time the text in the search filed changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextChanged(object sender, UISearchBarTextChangedEventArgs e)
        {
            Session.mmb.Search(e.SearchText);
            tableView.ReloadData();
        }

        private async void SearchButtonClicked(object sender, EventArgs e)
        {
            string searchValue = _SearchField.Text;
			//_SearchField.ResignFirstResponder();

            BTProgressHUD.Show();
            try
            {
                Session.mmb.Messages = await PlattformService.Service.Search(searchValue, MMBType);
            }
            catch (AppException app)
            {
                Util.Util.HandleAppException(app, this);
                Logger.Logg(app);
            }
            catch (Exception ex)
            {
                Util.Util.ShowAlert(_trans.GetString("Error_GeneralError"));
                Logger.Logg(ex);
            }

            tableView.ReloadData();
            BTProgressHUD.Dismiss();
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            _SearchField.Text = string.Empty;
        }

        private void SearchStarted(object sender, EventArgs e)
        {
        }

        private void SearchStopped(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Load the messagebox for this reportee starting at skipsize
        /// </summary>
        /// <param name="skipsize"></param>
        /// <returns></returns>
        private async Task<List<Message>> LoadMessagebox(int skipsize)
        {
            BTProgressHUD.Show();
            PlattformService.Service.UseOneInbox = true;
            List<Message> mess = await PlattformService.Service.PopulateMessagebox(_type, skipsize);

            if (Session.mmb.Unread > 0)
            {
                _headerLabel.Text = _headerString + " (" + Session.mmb.Unread + ")";
            }
            tableView.ReloadData();
            _once = true;

            BTProgressHUD.Dismiss();
            return mess;
        }
    }
}
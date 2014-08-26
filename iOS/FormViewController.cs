using System;
using System.Globalization;
using AltinnApp.Core.Models;
using AltinnApp.Core.Util;
using AltinnApp.iOS.CorePlatform;
using BigTed;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace AltinnApp.iOS
{
    /// <summary>
    /// The class for controlling the view of formTasks
    /// </summary>
    public partial class FormViewController : UIViewController
    {
        //private SI _si;
        private Translate _trans;
        private Constants.MMBType _type;

        public Constants.MMBType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="handle"></param>
        public FormViewController(IntPtr handle)
            : base(handle)
        {
        }

        public Message CurrentMessage { get; set; }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            var view = (AttachmentController) segue.DestinationViewController;
            view.SelectedMessage = CurrentMessage;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            BTProgressHUD.Dismiss();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            BTProgressHUD.Show();

            ConfigureView();
            _trans = new Translate();

            Localize();
        }

        private void Localize()
        {
            if (_type == Constants.MMBType.Archive)
            {
                _goToInbox.Text = _trans.GetString("MessageboxController_ArchiveHeader");
                if (CurrentMessage.MessageId.ToLower().StartsWith("c"))
                {
                    _printLabel.Text = _trans.GetString("Message_ArchivedAltinn1");
                    _printIcon.Enabled = false;
					_printLabel.TextColor = UIColor.FromRGB (181, 181, 181);
                }
                else
                {
                    _printLabel.Text = _trans.GetString("Message_Archived");
                }
            }
            else if (_type == Constants.MMBType.Messagebox)
            {
                _goToInbox.Text = _trans.GetString("MessageboxController_InboxHeader");
                _printLabel.Text = _trans.GetString("Message_Print");
            }
            else
            {
                _goToInbox.Text = string.Empty;
            }


            _header.Text = _trans.GetString("FormController_Header");
            _recipientLabel.Text = _trans.GetString("Message_Recipient");
            _lastChangedDateLabel.Text = _trans.GetString("Message_LastChanged");
            _infoBox.Text = _trans.GetString("Message_Infobox");
            _lastChangedByLabel.Text = _trans.GetString("Message_LastChangedBy");
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void GoBack(NSObject sender)
        {
            NavigationController.PopViewControllerAnimated(true);
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void HandleSwipeRight(NSObject sender)
        {
            GoBack(null);
        }

        public override bool ShouldAutorotate()
        {
            return false;
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void OpenPrint(NSObject sender)
        {
        }

        private async void ConfigureView()
        {
            try
            {
                //var si = new PlattformService();
                if (IsViewLoaded && CurrentMessage != null)
                {
                    //Retrieve the full message

                    AddPadding();

                    if (!CurrentMessage.IsDownloaded)
                    {
                        CurrentMessage = await PlattformService.Service.GetMessage(CurrentMessage);
                    }

                    _subject.Text = CurrentMessage.Subject;
                    _lastChangedDate.Text += CurrentMessage.LastChangedDateTime.ToString("dd.MM.yyyy hh:mm",
                        CultureInfo.InvariantCulture);
                    _lastChangedBy.Text += CurrentMessage.LastChangedBy;
                    //we can either hide the controls or set then enabled = false and use a separate icon for that state (can be set in the Storyboard)

                    //The rest depend on if we have a print view of the FormTask
                    if (CurrentMessage.Links != null && CurrentMessage.Links.Print != null &&
                        CurrentMessage.Links.Print.Href != null)
                    {
                        //If no print view exist hide the controls
                        _printIcon.Hidden = false;
                        _printLabel.Hidden = false;
                    }
                    else
                    {
                        _printIcon.Hidden = true;
                        _printLabel.Hidden = true;
                    }


                    _recipient.Text = CurrentMessage.ServiceOwner ?? CurrentMessage.ServiceOwner;
                }
            }
            catch (AppException app)
            {
                Util.Util.HandleAppException(app, this);
            }
            catch (Exception ex)
            {
                Logger.Logg("General error occured when getting the message: " + ex);
                Util.Util.ShowAlert(ErrorMessages.GeneralError);
            }
        }

        private void AddPadding()
        {
            //If we have English, add some more spaces to the label
            _lastChangedDate.Text = string.Empty;

            _lastChangedBy.Text = string.Empty;
			if (AppContext.CurrentLanguage == "1033") {
				_lastChangedDate.Text = "     ";

				_lastChangedBy.Text = "     ";
			} else if (AppContext.CurrentLanguage == "2068") {


			} else if (AppContext.CurrentLanguage == "1044") {
				_lastChangedDate.Text = " ";
			}
        }
    }
}
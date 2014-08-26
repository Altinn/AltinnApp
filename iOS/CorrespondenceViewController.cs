using System;
using System.Globalization;
using AltinnApp.Core.Models;
using AltinnApp.Core.Util;
using AltinnApp.iOS.CorePlatform;
using BigTed;
using MonoTouch.Foundation;
using MonoTouch.MessageUI;
using MonoTouch.UIKit;
using System.Drawing;

namespace AltinnApp.iOS
{
    /// <summary>
    /// The class for controlling the view of correspondences
    /// </summary>
    public partial class CorrespondenceViewController : UIViewController
    {
        //private PlattformService _si;
        //private NSUrl FileURL;
        private Translate _trans;
        private Constants.MMBType _type;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="handle"></param>
        public CorrespondenceViewController(IntPtr handle)
            : base(handle)
        {
        }

        public Constants.MMBType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public Message CurrentMessage { get; set; }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            var view = (AttachmentController) segue.DestinationViewController;
            view.SelectedMessage = CurrentMessage;
        }

        public override bool ShouldAutorotate()
        {
            return false;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            ConfigureView();

            BTProgressHUD.Dismiss();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            BTProgressHUD.Show();

            webview.ShouldStartLoad = ShouldStartHandler;

            _trans = new Translate();
            Localize();
        }

        private void Localize()
        {
            if (_type == Constants.MMBType.Archive)
            {
                GoToInbox.Text = _trans.GetString("MessageboxController_ArchiveHeader");
            }
            else if (_type == Constants.MMBType.Messagebox)
            {
                GoToInbox.Text = _trans.GetString("MessageboxController_InboxHeader");
            }
            else
            {
                GoToInbox.Text = string.Empty;
            }
            _header.Text = _trans.GetString("CorrespondenceController_Header");
            _recipient.Text = _trans.GetString("Message_Recipient");
            _DateReceivedLabel.Text = _trans.GetString("Message_LastChanged");
            _SenderLabel.Text = _trans.GetString("Message_Sender");
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void GoBack(NSObject sender)
        {
            NavigationController.PopViewControllerAnimated(true);
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        async partial void SendToMail(NSObject sender)
        {
            try
            {
                BTProgressHUD.Show();
                var mailController = new MFMailComposeViewController();

                mailController.SetToRecipients(new[]
                {
                    string.Empty
                });
                mailController.SetSubject(CurrentMessage.Subject);
                mailController.SetMessageBody(CurrentMessage.Body, true);

                byte[] data = await PlattformService.Service.GetAttachment(CurrentMessage.Links.Attachment[0].Href);
                NSData attachment = NSData.FromArray(data);

                if (attachment != null && CurrentMessage.Links.Attachment[0].Mimetype != null)
                {
                    mailController.AddAttachmentData(attachment, CurrentMessage.Links.Attachment[0].Mimetype,
                        CurrentMessage.Links.Attachment[0].Name);
                }

                mailController.Finished += (s, args) =>
                {
                    Logger.Logg(args.Result.ToString());
                    args.Controller.DismissViewController(true, null);
                };
                BTProgressHUD.Dismiss();
                PresentViewController(mailController, true, null);
            }
            catch (Exception ex)
            {
                Logger.Logg("failed to create mail: " + ex);
            }
        }

        private async void ConfigureView()
        {
            try
            {
                AddPadding();


                //_si = new PlattformService();
                // Update the user interface for the detail item
                if (IsViewLoaded && CurrentMessage != null)
                {
                    //Retrieve the full message

                    if (!CurrentMessage.IsDownloaded)
                    {
                        if (CurrentMessage.Body == null || CurrentMessage.Summary == null)
                        {
                            CurrentMessage = await PlattformService.Service.GetMessage(CurrentMessage);
                        }
                    }
                    _subject.Text = CurrentMessage.Subject;

                    _createdDate.Text += CurrentMessage.LastChangedDateTime.ToString("dd.MM.yyyy hh:mm",
                        CultureInfo.InvariantCulture);

                    _serviceOwner.Text = Session.SelectedOrg.Name;
                    _sender.Text += CurrentMessage.ServiceOwner;
                    _sendToMailLabel.Text = _trans.GetString("CorrespondenceController_SendToMail");
                    //If there is no attachment hide the buttons and labels


                    if (CurrentMessage.Links.Attachment == null)
                    {
                        _attachmentIcon.Hidden = true;
                        _attachmentLabel.Hidden = true;
                        _attachmentLabel.Text = _trans.GetString("CorrespondenceController_NoAttachments");
                        _separatorImageTop.Hidden = true;
                        _separatorImageBottom.Hidden = true;
                        _sendToMailIcon.Hidden = true;
                        _sendToMailLabel.Hidden = true;

                        RectangleF frame = webview.Frame;
                        frame.Height += 100;
                        webview = new UIWebView(frame);
                        Add(webview);
                        //If we hide the attachment actions at the bottom then we habe space for 50px more
                    }
                    else if (
                        !(CurrentMessage.Links.Attachment[0].FileEnding == "pdf" ||
                          CurrentMessage.Links.Attachment[0].FileEnding == "html"))
                    {
                        _attachmentIcon.Enabled = false;
                        _attachmentLabel.Text = CurrentMessage.Links.Attachment[0].Name;
                    }
                    else
                    {
                        //Otherwise show them
                        //There could be multiple attachments, but only the first is processed at this time
                        if (CurrentMessage.Links.Attachment != null &&
                            CurrentMessage.Links.Attachment[0].Encrypted)
                        {
                            _attachmentIcon.Enabled = false;
                            _attachmentLabel.Text = _trans.GetString("CorrespondenceController_CannotForward");
                        }

                        //If there are any restrictions on what attachments the user is allowed to send to mail then this is the place to do that
                        _attachmentIcon.Enabled = true;
                        _attachmentLabel.Enabled = true;
                        _attachmentLabel.Text = _trans.GetString("CorrespondenceController_Open") +
                                                CurrentMessage.Links.Attachment[0].Name;
                        _sendToMailIcon.Enabled = true;
                        _sendToMailLabel.Enabled = true;
                    }


                    webview.ShouldStartLoad = ShouldStartHandler;

                    if (!string.IsNullOrEmpty(CurrentMessage.Summary) && !string.IsNullOrEmpty(CurrentMessage.Body))
                    {
                        var n = new NSUrl(CurrentMessage.Links.Self.Href);
                        string both = CurrentMessage.Summary + CurrentMessage.Body;
                        webview.LoadHtmlString(both, n);
                        _summaryLabel.Hidden = true;
                    }
                    else if (!string.IsNullOrEmpty(CurrentMessage.Body))
                    {
                        var n = new NSUrl(CurrentMessage.Links.Self.Href);
                        webview.LoadHtmlString(CurrentMessage.Body, n);
                        _summaryLabel.Hidden = true;
                    }
                    else if (!string.IsNullOrEmpty(CurrentMessage.Summary))
                    {
                        if (isHtml(CurrentMessage.Summary))
                        {
                            var n = new NSUrl(CurrentMessage.Links.Self.Href);
                            webview.LoadHtmlString(CurrentMessage.Summary, n);
                            _summaryLabel.Hidden = true;
                        }
                        else
                        {
                            webview.Hidden = true;
                            _summaryLabel.Hidden = false;
                            _summaryLabel.Text = CurrentMessage.Summary;
                            _summaryLabel.TextAlignment = UITextAlignment.Left;
                            _summaryLabel.Lines = 0;
                            _summaryLabel.SizeToFit();
                        }
                    }
                    else
                    {
                        webview.Hidden = true;
                        _summaryLabel.Hidden = false;
                        _summaryLabel.Text = _trans.GetString("Message_OnlyAttachmentMessage");
                    }
                }

                if (CurrentMessage != null && CurrentMessage.Status == "Unread")
                {
                    Session.mmb.CurrentMessage.Status = "Read";
                }
                else if (CurrentMessage != null && CurrentMessage.Status == "Ulest")
                {
                    Session.mmb.CurrentMessage.Status = "Lest";
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
            _sender.Text = string.Empty;
            _createdDate.Text = string.Empty;

			if (AppContext.CurrentLanguage == "1044")
            {
                _sender.Text = "    ";
				_createdDate.Text = " ";
            }
            else if (AppContext.CurrentLanguage == "1033")
            {
				_createdDate.Text = "     ";
			}else if(AppContext.CurrentLanguage == "2068"){
				_sender.Text = "    ";

			}
        }

        private bool isHtml(string summary)
        {
            if (summary.Contains("<p>") || summary.Contains("<html>") || summary.Contains("<body>") ||
                summary.Contains("<br>") || summary.Contains("</a>"))
            {
                return true;
            }
            return false;
        }

        private bool ShouldStartHandler(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navType)
        {
            //If we use external browser

            //and if the user clicked a link in the view
            if (navType == UIWebViewNavigationType.LinkClicked)
            {
                UIApplication.SharedApplication.OpenUrl(request.Url);
                //To open
                return false;
            }

            return true;
        }
    }
}
using System;
using System.IO;
using AltinnApp.Core.Models;
using AltinnApp.Core.Util;
using AltinnApp.iOS.CorePlatform;
using AltinnApp.iOS.Util;
using BigTed;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace AltinnApp.iOS
{
    /// <summary>
    /// Class responsible for handling the opening of print view and attachments
    /// </summary>
    public partial class AttachmentController : UIViewController
    {
        //private NSMutableUrlRequest _theRequest;

        internal string AttachmentFilename;
        //private readonly PlattformService _service;
        private Translate _trans;

        /// <summary>
        /// Default constructor for Attachent
        /// </summary>
        /// <param name="handle"></param>
        public AttachmentController(IntPtr handle)
            : base(handle)
        {
            //_service = new PlattformService();
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        async partial void OpenIn(NSObject sender)
        {
            try
            {
                if (SelectedMessage.SavedFileName == null)
                {
                    BTProgressHUD.Show();
                    string filename;

                    if (SelectedMessage.Type == Constants.MessageType.FormTask.ToString())
                    {
                        if (SelectedMessage.Links.Print != null && SelectedMessage.Links.Print.Mimetype == "text/html")
                        {
                            //opening html forms has some isues

                            string data =
                                await PlattformService.Service.GetAttachmentAsString(SelectedMessage.Links.Print.Href);
                            string attachment = data;
                            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            filename = Path.Combine(documents,
                                SelectedMessage.MessageId + ".html");
                            if (attachment != null)
                            {
                                File.WriteAllText(filename, attachment);
                                SelectedMessage.SavedFileName = filename;
                            }
                        }
                        else
                        {
                            if (SelectedMessage.Links.Print != null)
                            {
                                byte[] data =
                                    await PlattformService.Service.GetAttachment(SelectedMessage.Links.Print.Href);
                                byte[] attachment = data;
                                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                                filename = Path.Combine(documents,
                                    SelectedMessage.MessageId + ".pdf");
                                if (attachment != null)
                                {
                                    File.WriteAllBytes(filename, attachment);
                                    SelectedMessage.SavedFileName = filename;
                                }
                            }
                        }
                    }
                    else if (SelectedMessage.Type == Constants.MessageType.Correspondence.ToString())
                    {
                        if (SelectedMessage.Links.Attachment != null)
                        {
                            byte[] data =
                                await PlattformService.Service.GetAttachment(SelectedMessage.Links.Attachment[0].Href);
                            byte[] attachment = data;
                            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            filename = Path.Combine(documents,
                                SelectedMessage.Links.Attachment[0].Name);
                            if (attachment != null)
                            {
                                File.WriteAllBytes(filename, attachment);

                                SelectedMessage.SavedFileName = filename;
                            }
                        }
                    }

                    BTProgressHUD.Dismiss();
                }
                if (SelectedMessage.SavedFileName != null)
                {
                    NSUrl fileUrl = NSUrl.FromFilename(SelectedMessage.SavedFileName);
                    var viewer = UIDocumentInteractionController.FromUrl(fileUrl);
                    viewer.ViewControllerForPreview = c => this;
                    viewer.PresentOptionsMenu(View.Bounds, View, true);
                }
            }
            catch (Exception ex)
            {
                Logger.Logg("Failed to openWith attachment: " + ex);
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary>
        /// Gets or sets the Selected Message
        /// </summary>
        public Message SelectedMessage { get; set; }

        public int Status { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            webview.AutosizesSubviews = true;
            BTProgressHUD.Show();
            View.AutosizesSubviews = true;
            _trans = new Translate();
        }

        public override bool ShouldAutorotate()
        {
            return true;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.All;
        }

        public override async void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            BTProgressHUD.Show();

            try
            {
                if (Reachability.IsHostReachable(AppContext.LoginUrl))
                {
                    //Check that all the necessary information is in place
                    if (SelectedMessage != null &&
                        (SelectedMessage.Links.Print != null || SelectedMessage.Links.Attachment != null))
                    {
                        //Distinguish between FormTask and Correspondence
                        if (SelectedMessage.Type == Constants.MessageType.Correspondence.ToString())
                        {
                            if (SelectedMessage.Links.Attachment != null)
                            {
                                if (SelectedMessage.Links.Attachment != null)
                                {
                                    byte[] data =
                                        await
                                            PlattformService.Service.GetAttachment(
                                                SelectedMessage.Links.Attachment[0].Href);


                                    webview.LoadData(NSData.FromArray(data),
                                        SelectedMessage.Links.Attachment[0].Mimetype, "UTF-8",
                                        new NSUrl(SelectedMessage.Links.Self.Href));
                                }
                            }
                        }
                        else if (SelectedMessage.Type == Constants.MessageType.FormTask.ToString())
                        {
                            if (SelectedMessage.Links.Print != null)
                            {
                                if (SelectedMessage.Links.Print.Mimetype != null)
                                {
                                    string mimeType = SelectedMessage.Links.Print.Mimetype;
                                    if (!(mimeType == "application/pdf" || mimeType == "text/html"))
                                    {
                                        throw new AppException("0", ErrorMessages.PrintFailed);
                                    }


                                    if (mimeType == "text/html")
                                    {
                                        string data =
                                            await
                                                PlattformService.Service.GetAttachmentAsString(
                                                    SelectedMessage.Links.Print.Href);

//										webview.LoadHtmlString(NSData.FromString(attachment),
//                                            SelectedMessage.Links.Print.Mimetype, "UTF-8",
//                                            new NSUrl(SelectedMessage.Links.Print.Href));
                                        webview.LoadHtmlString(data, new NSUrl(SelectedMessage.Links.Print.Href));
                                    }
                                    else
                                    {
                                        byte[] data =
                                            await
                                                PlattformService.Service.GetAttachment(SelectedMessage.Links.Print.Href);

                                        webview.LoadData(NSData.FromArray(data),
                                            SelectedMessage.Links.Print.Mimetype, "UTF-8",
                                            new NSUrl(SelectedMessage.Links.Print.Href));
                                    }
                                }
                            }
                        }

                        BTProgressHUD.Dismiss();
                    }
                }
                else
                {
                    Util.Util.ShowAlert(ErrorMessages.NoNetworkAvailable);
                }
            }
            catch (AppException app)
            {
                Util.Util.HandleAppException(app, this);
            }
            catch (Exception ex)
            {
                Logger.Logg("Failed to load the data for the webview" + ex);
                Util.Util.ShowAlert(ErrorMessages.GeneralError);
            }
            BTProgressHUD.Dismiss();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (SelectedMessage != null)
            {
                if (SelectedMessage.Type == Constants.MessageType.Correspondence.ToString())
                {
                    _backLabel.Text = _backLabel.Text = _trans.GetString("CorrespondenceController_Header");

                    _openIn.Hidden = false;

                    if (SelectedMessage.Links.Attachment != null)
                    {
                        _headerLabel.Text = SelectedMessage.Links.Attachment[0].Name;
                    }
                }
                else if (SelectedMessage.Type == Constants.MessageType.FormTask.ToString())
                {
                    _backLabel.Text = _backLabel.Text = _trans.GetString("FormController_Header");

                    _openIn.Hidden = false;

                    if (SelectedMessage.Links.Print != null)
                    {
                        _headerLabel.Text = SelectedMessage.Subject;
                    }
                }
            }
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void GoBack(NSObject sender)
        {
            BTProgressHUD.Dismiss();
            NavigationController.PopViewControllerAnimated(true);
        }
    }
}
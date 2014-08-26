using System;
using AltinnApp.Core.Util;
using MonoTouch.Foundation;
using MonoTouch.MessageUI;
using MonoTouch.UIKit;
using SWRevealViewControllerBinding;

namespace AltinnApp.iOS
{
    public partial class ContactController : UIViewController
    {
        private CorePlatform.Translate _trans;

        public ContactController(IntPtr handle)
            : base(handle)
        {
        }

        private void Localize()
        {
            _header.Text = _trans.GetString("ContactController_Header");
            _bodyTopHeader.Text = _trans.GetString("ContactController_BodyTopHeader");
            _bodyTop.Text = _trans.GetString("ContactController_BodyTop");
            _bodyBottomHeader.Text = _trans.GetString("ContactController_BodyBottomHeader");
            _bodyBottom.Text = _trans.GetString("ContactController_BodyBottom");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            if (this.RevealViewController() != null)
            {
                _gobackButton.TouchUpInside += (sender, e) => this.RevealViewController().RevealToggleAnimated(true);
                View.AddGestureRecognizer(this.RevealViewController().PanGestureRecognizer);
            }

			//_contactTlf.TitleLabel.Text = Constants.ContactSupportTlf;
			//_contactMail.TitleLabel.Text = Constants.ContactSupportTlf;
            _trans = new CorePlatform.Translate();
            Localize();
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void OpenFacebook(NSObject sender)
        {
            var nsurl = new NSUrl("fb://profile/214112901961006");
            if (UIApplication.SharedApplication.CanOpenUrl(nsurl))
            {
                UIApplication.SharedApplication.OpenUrl(nsurl);
            }
            else
            {
                Util.Util.ShowAlert(_trans.GetString("Error_MissingFBApp"));
            }
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void OpenTwitter(NSObject sender)
        {
            var nsurl = new NSUrl("twitter://user?screen_name=Altinn");
            if (UIApplication.SharedApplication.CanOpenUrl(nsurl))
            {
                UIApplication.SharedApplication.OpenUrl(nsurl);
            }
            else
            {
                Util.Util.ShowAlert(_trans.GetString("Error_MissingFBApp"));
            }
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void Call(NSObject sender)
        {
            string tlf = "tel://" + Constants.ContactSupportTlf.Trim().Replace(" ", "");
            var url = new NSUrl(tlf);
            UIApplication.SharedApplication.OpenUrl(url);
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void SendMail(NSObject sender)
        {
            var mailController = new MFMailComposeViewController();

            mailController.SetToRecipients(new[]
            {
                "support@altinn.no"
            });
            mailController.SetSubject(_trans.GetString("ContactController_MailTitle"));


            mailController.Finished += (s, args) =>
            {
                
                args.Controller.DismissViewController(true, null);
            };

            PresentViewController(mailController, true, null);
        }
    }
}
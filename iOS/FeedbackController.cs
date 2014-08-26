using System;
using AltinnApp.Core.Util;
using MonoTouch.Foundation;
using MonoTouch.MessageUI;
using MonoTouch.UIKit;
using SWRevealViewControllerBinding;
using AltinnApp.Core.Models;

namespace AltinnApp.iOS
{
    public partial class FeedbackController : UIViewController
    {
        private CorePlatform.Translate _trans;

        public FeedbackController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            if (this.RevealViewController() != null)
            {
                _gobackButton.TouchUpInside += (sender, e) => this.RevealViewController().RevealToggleAnimated(true);
                View.AddGestureRecognizer(this.RevealViewController().PanGestureRecognizer);

                //Popout from Flyout
                //				mailController.ModalInPopover = true;
                //				mailController.SetToRecipients(new[] { "app@altinn.no" });
                //				mailController.SetSubject("Altinn hjelp");
                //				mailController.SetMessageBody("Ansvarsfraskrivelse av Altinn!", true);
                //
                //				mailController.Finished += (s, args) =>
                //				{
                //					Logger.Logg(args.Result.ToString());
                //
                //					this.RevealViewController().RevealToggleAnimated(true);
                //					args.Controller.DismissViewController(false, null);
                //					//this.RevealViewController().SetFrontViewController(this.RevealViewController().RearViewController, true);
                //				};
                //
                //				//this.RevealViewController ().SetFrontViewController (mailController, true);
                //
                //				mailController.ModalInPopover = true;
                //				//					this.RevealViewController().SetFrontViewController(mailController, false);
                //				this.RevealViewController().PresentModalViewController(mailController, false);
                _trans = new CorePlatform.Translate();

                Localize();
            }
        }

        /// <summary>
        /// Localize the strings in this controller
        /// </summary>
        private void Localize()
        {
            _bodyHeader.Text = _trans.GetString("FeedbackController_BodyHeader");
            _body.Text = _trans.GetString("FeedbackController_Body");
            _header.Text = _trans.GetString("FeedbackController_Header");
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void SendMail(NSObject sender)
        {
            var mailController = new MFMailComposeViewController();

            mailController.SetToRecipients(new[]
            {
                Constants.FeedbackMail
            });
            mailController.SetSubject(_trans.GetString("FeedbackController_MailTitle"));


            mailController.Finished += (s, args) =>
            {
                Logger.Logg(args.Result.ToString());
                args.Controller.DismissViewController(true, null);
            };

            PresentViewController(mailController, true, null);
        }
    }
}
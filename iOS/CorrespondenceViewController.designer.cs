// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace AltinnApp.iOS
{
	[Register ("CorrespondenceViewController")]
	partial class CorrespondenceViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton _attachmentIcon { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _attachmentLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _createdDate { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _DateReceivedLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _header { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _recipient { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _sender { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _SenderLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _sendToMailIcon { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _sendToMailLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView _separatorImageBottom { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView _separatorImageTop { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _serviceOwner { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _subject { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _summaryLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel GoToInbox { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton[] OpenAttachment { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIWebView webview { get; set; }

		[Action ("GoBack:")]
		partial void GoBack (MonoTouch.Foundation.NSObject sender);

		[Action ("HandleSwipeRight:")]
		partial void HandleSwipeRight (MonoTouch.Foundation.NSObject sender);

		[Action ("SendToMail:")]
		partial void SendToMail (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_header != null) {
				_header.Dispose ();
				_header = null;
			}

			if (_DateReceivedLabel != null) {
				_DateReceivedLabel.Dispose ();
				_DateReceivedLabel = null;
			}

			if (_SenderLabel != null) {
				_SenderLabel.Dispose ();
				_SenderLabel = null;
			}

			if (_attachmentIcon != null) {
				_attachmentIcon.Dispose ();
				_attachmentIcon = null;
			}

			if (_attachmentLabel != null) {
				_attachmentLabel.Dispose ();
				_attachmentLabel = null;
			}

			if (_createdDate != null) {
				_createdDate.Dispose ();
				_createdDate = null;
			}

			if (_recipient != null) {
				_recipient.Dispose ();
				_recipient = null;
			}

			if (_sender != null) {
				_sender.Dispose ();
				_sender = null;
			}

			if (_sendToMailIcon != null) {
				_sendToMailIcon.Dispose ();
				_sendToMailIcon = null;
			}

			if (_sendToMailLabel != null) {
				_sendToMailLabel.Dispose ();
				_sendToMailLabel = null;
			}

			if (_separatorImageBottom != null) {
				_separatorImageBottom.Dispose ();
				_separatorImageBottom = null;
			}

			if (_separatorImageTop != null) {
				_separatorImageTop.Dispose ();
				_separatorImageTop = null;
			}

			if (_serviceOwner != null) {
				_serviceOwner.Dispose ();
				_serviceOwner = null;
			}

			if (_subject != null) {
				_subject.Dispose ();
				_subject = null;
			}

			if (_summaryLabel != null) {
				_summaryLabel.Dispose ();
				_summaryLabel = null;
			}

			if (GoToInbox != null) {
				GoToInbox.Dispose ();
				GoToInbox = null;
			}

			if (webview != null) {
				webview.Dispose ();
				webview = null;
			}
		}
	}
}

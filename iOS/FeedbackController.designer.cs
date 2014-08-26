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
	[Register ("FeedbackController")]
	partial class FeedbackController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel _body { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _bodyHeader { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _gobackButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _header { get; set; }

		[Action ("SendMail:")]
		partial void SendMail (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_body != null) {
				_body.Dispose ();
				_body = null;
			}

			if (_bodyHeader != null) {
				_bodyHeader.Dispose ();
				_bodyHeader = null;
			}

			if (_gobackButton != null) {
				_gobackButton.Dispose ();
				_gobackButton = null;
			}

			if (_header != null) {
				_header.Dispose ();
				_header = null;
			}
		}
	}
}

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
	[Register ("ContactController")]
	partial class ContactController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel _bodyBottom { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _bodyBottomHeader { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _bodyTop { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _bodyTopHeader { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _contactMail { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _contactTlf { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _facebookButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _gobackButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _header { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _twitterButton { get; set; }

		[Action ("Call:")]
		partial void Call (MonoTouch.Foundation.NSObject sender);

		[Action ("OpenFacebook:")]
		partial void OpenFacebook (MonoTouch.Foundation.NSObject sender);

		[Action ("OpenTwitter:")]
		partial void OpenTwitter (MonoTouch.Foundation.NSObject sender);

		[Action ("SendMail:")]
		partial void SendMail (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_bodyBottom != null) {
				_bodyBottom.Dispose ();
				_bodyBottom = null;
			}

			if (_bodyBottomHeader != null) {
				_bodyBottomHeader.Dispose ();
				_bodyBottomHeader = null;
			}

			if (_bodyTop != null) {
				_bodyTop.Dispose ();
				_bodyTop = null;
			}

			if (_bodyTopHeader != null) {
				_bodyTopHeader.Dispose ();
				_bodyTopHeader = null;
			}

			if (_contactMail != null) {
				_contactMail.Dispose ();
				_contactMail = null;
			}

			if (_contactTlf != null) {
				_contactTlf.Dispose ();
				_contactTlf = null;
			}

			if (_facebookButton != null) {
				_facebookButton.Dispose ();
				_facebookButton = null;
			}

			if (_gobackButton != null) {
				_gobackButton.Dispose ();
				_gobackButton = null;
			}

			if (_header != null) {
				_header.Dispose ();
				_header = null;
			}

			if (_twitterButton != null) {
				_twitterButton.Dispose ();
				_twitterButton = null;
			}
		}
	}
}

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
	[Register ("LoginController")]
	partial class LoginController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel _IntroText { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _LoginButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton OpenLanguage { get; set; }

		[Action ("Login:")]
		partial void Login (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_IntroText != null) {
				_IntroText.Dispose ();
				_IntroText = null;
			}

			if (_LoginButton != null) {
				_LoginButton.Dispose ();
				_LoginButton = null;
			}

			if (OpenLanguage != null) {
				OpenLanguage.Dispose ();
				OpenLanguage = null;
			}
		}
	}
}

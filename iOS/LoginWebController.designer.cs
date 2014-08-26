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
	[Register ("LoginWebController")]
	partial class LoginWebController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel _headerLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIWebView webview { get; set; }

		[Action ("GoBack:")]
		partial void GoBack (MonoTouch.Foundation.NSObject sender);

		[Action ("HandleSwipe:")]
		partial void HandleSwipe (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (webview != null) {
				webview.Dispose ();
				webview = null;
			}

			if (_headerLabel != null) {
				_headerLabel.Dispose ();
				_headerLabel = null;
			}
		}
	}
}

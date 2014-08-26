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
	[Register ("AttachmentController")]
	partial class AttachmentController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel _backLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _headerLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _openIn { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIWebView webview { get; set; }

		[Action ("GoBack:")]
		partial void GoBack (MonoTouch.Foundation.NSObject sender);

		[Action ("OpenIn:")]
		partial void OpenIn (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_backLabel != null) {
				_backLabel.Dispose ();
				_backLabel = null;
			}

			if (_headerLabel != null) {
				_headerLabel.Dispose ();
				_headerLabel = null;
			}

			if (webview != null) {
				webview.Dispose ();
				webview = null;
			}

			if (_openIn != null) {
				_openIn.Dispose ();
				_openIn = null;
			}
		}
	}
}

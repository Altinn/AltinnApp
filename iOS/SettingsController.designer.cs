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
	[Register ("SettingsController")]
	partial class SettingsController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton _gobackButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _header { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView tableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_gobackButton != null) {
				_gobackButton.Dispose ();
				_gobackButton = null;
			}

			if (_header != null) {
				_header.Dispose ();
				_header = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}
		}
	}
}

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
	[Register ("FlyoutController")]
	partial class FlyoutController
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView _actorForwardIcon { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _actorLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _lastNameLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _logoutButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView _nameIcon { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _nameLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView _tableView { get; set; }

		[Action ("_logout:")]
		partial void _logout (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_actorForwardIcon != null) {
				_actorForwardIcon.Dispose ();
				_actorForwardIcon = null;
			}

			if (_actorLabel != null) {
				_actorLabel.Dispose ();
				_actorLabel = null;
			}

			if (_lastNameLabel != null) {
				_lastNameLabel.Dispose ();
				_lastNameLabel = null;
			}

			if (_logoutButton != null) {
				_logoutButton.Dispose ();
				_logoutButton = null;
			}

			if (_nameIcon != null) {
				_nameIcon.Dispose ();
				_nameIcon = null;
			}

			if (_nameLabel != null) {
				_nameLabel.Dispose ();
				_nameLabel = null;
			}

			if (_tableView != null) {
				_tableView.Dispose ();
				_tableView = null;
			}
		}
	}
}

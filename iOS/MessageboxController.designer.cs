// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using MonoTouch.Foundation;

namespace AltinnApp.iOS
{
	[Register ("MessageboxController")]
	partial class MessageboxController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton _goBack { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _headerLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISearchBar _SearchField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView tableView { get; set; }

		[Action ("GoBack:")]
		partial void GoBack (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_goBack != null) {
				_goBack.Dispose ();
				_goBack = null;
			}

			if (_headerLabel != null) {
				_headerLabel.Dispose ();
				_headerLabel = null;
			}

			if (_SearchField != null) {
				_SearchField.Dispose ();
				_SearchField = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}
		}
	}
}

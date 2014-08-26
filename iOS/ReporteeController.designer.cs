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
	[Register ("ReporteeController")]
	partial class ReporteeController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton _goBack { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _header { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _innboxLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISearchBar _searchField { get; set; }

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

			if (_header != null) {
				_header.Dispose ();
				_header = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (_innboxLabel != null) {
				_innboxLabel.Dispose ();
				_innboxLabel = null;
			}

			if (_searchField != null) {
				_searchField.Dispose ();
				_searchField = null;
			}
		}
	}
}

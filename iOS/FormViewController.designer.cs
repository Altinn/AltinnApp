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
	[Register ("FormViewController")]
	partial class FormViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel _goToInbox { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _header { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _infoBox { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _lastChangedBy { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _lastChangedByLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _lastChangedDate { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _lastChangedDateLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _printIcon { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _printLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _recipient { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _recipientLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _subject { get; set; }

		[Action ("GoBack:")]
		partial void GoBack (MonoTouch.Foundation.NSObject sender);

		[Action ("HandleSwipeRight:")]
		partial void HandleSwipeRight (MonoTouch.Foundation.NSObject sender);

		[Action ("OpenPrint:")]
		partial void OpenPrint (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_subject != null) {
				_subject.Dispose ();
				_subject = null;
			}

			if (_lastChangedBy != null) {
				_lastChangedBy.Dispose ();
				_lastChangedBy = null;
			}

			if (_recipientLabel != null) {
				_recipientLabel.Dispose ();
				_recipientLabel = null;
			}

			if (_lastChangedDate != null) {
				_lastChangedDate.Dispose ();
				_lastChangedDate = null;
			}

			if (_recipient != null) {
				_recipient.Dispose ();
				_recipient = null;
			}

			if (_lastChangedByLabel != null) {
				_lastChangedByLabel.Dispose ();
				_lastChangedByLabel = null;
			}

			if (_lastChangedDateLabel != null) {
				_lastChangedDateLabel.Dispose ();
				_lastChangedDateLabel = null;
			}

			if (_goToInbox != null) {
				_goToInbox.Dispose ();
				_goToInbox = null;
			}

			if (_header != null) {
				_header.Dispose ();
				_header = null;
			}

			if (_infoBox != null) {
				_infoBox.Dispose ();
				_infoBox = null;
			}

			if (_printIcon != null) {
				_printIcon.Dispose ();
				_printIcon = null;
			}

			if (_printLabel != null) {
				_printLabel.Dispose ();
				_printLabel = null;
			}
		}
	}
}

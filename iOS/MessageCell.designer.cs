// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using MonoTouch.Foundation;

namespace AltinnApp.iOS
{
	[Register ("MessageCell")]
	partial class MessageCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel _date { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _from { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _subject { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_from != null) {
				_from.Dispose ();
				_from = null;
			}

			if (_subject != null) {
				_subject.Dispose ();
				_subject = null;
			}

			if (_date != null) {
				_date.Dispose ();
				_date = null;
			}
		}
	}
}

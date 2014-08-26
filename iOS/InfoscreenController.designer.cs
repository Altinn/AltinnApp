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
	[Register ("InfoscreenController")]
	partial class InfoscreenController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel _body1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _body2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _body3 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _body4 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _body5 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _cancel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _continue { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _headerBottom { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _headerTop { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _intro { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _outtro { get; set; }

		[Action ("Cancel:")]
		partial void Cancel (MonoTouch.Foundation.NSObject sender);

		[Action ("Continue:")]
		partial void Continue (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_cancel != null) {
				_cancel.Dispose ();
				_cancel = null;
			}

			if (_continue != null) {
				_continue.Dispose ();
				_continue = null;
			}

			if (_intro != null) {
				_intro.Dispose ();
				_intro = null;
			}

			if (_headerTop != null) {
				_headerTop.Dispose ();
				_headerTop = null;
			}

			if (_body1 != null) {
				_body1.Dispose ();
				_body1 = null;
			}

			if (_body2 != null) {
				_body2.Dispose ();
				_body2 = null;
			}

			if (_body3 != null) {
				_body3.Dispose ();
				_body3 = null;
			}

			if (_headerBottom != null) {
				_headerBottom.Dispose ();
				_headerBottom = null;
			}

			if (_body4 != null) {
				_body4.Dispose ();
				_body4 = null;
			}

			if (_body5 != null) {
				_body5.Dispose ();
				_body5 = null;
			}

			if (_outtro != null) {
				_outtro.Dispose ();
				_outtro = null;
			}
		}
	}
}

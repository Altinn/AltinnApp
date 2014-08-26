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
	[Register ("SettingsCell")]
	partial class SettingsCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel _languageLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_languageLabel != null) {
				_languageLabel.Dispose ();
				_languageLabel = null;
			}
		}
	}
}

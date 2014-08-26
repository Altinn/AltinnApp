using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace AltinnApp.iOS
{
    public partial class Language : UIViewController
    {
        public Language(IntPtr handle)
            : base(handle)
        {
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        partial void _goBack(NSObject sender)
        {
            NavigationController.PopViewControllerAnimated(true);
        }
    }
}
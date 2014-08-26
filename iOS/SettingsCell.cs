using System;
using MonoTouch.UIKit;

namespace AltinnApp.iOS
{
    public partial class SettingsCell : UITableViewCell
    {
        private readonly CorePlatform.Translate _trans;

        public SettingsCell(IntPtr handle) : base(handle)
        {
            _trans = new CorePlatform.Translate();
        }

        public SettingsCell()
        {
        }

        public void UpdateCell()
        {
            var backgroundView = new UIView(Frame) {BackgroundColor = UIColor.FromRGB(73, 73, 73)};
            SelectedBackgroundView = backgroundView;
            _languageLabel.Text = _trans.GetString("SettingsController_LanguageSelection");
        }
    }
}
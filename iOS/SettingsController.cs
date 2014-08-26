using System;
using MonoTouch.UIKit;
using SWRevealViewControllerBinding;
using MonoTouch.Foundation;
using AltinnApp.Core.Models;

namespace AltinnApp.iOS
{
    public partial class SettingsController : UIViewController
    {
        private CorePlatform.Translate _trans;

        public SettingsController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            tableView.WeakDataSource = tableView.WeakDelegate = this;
            if (this.RevealViewController() != null)
            {
                _gobackButton.TouchUpInside += (sender, e) => this.RevealViewController().RevealToggleAnimated(true);
                View.AddGestureRecognizer(this.RevealViewController().PanGestureRecognizer);
            }
            _trans = new CorePlatform.Translate();
            Localize();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            Localize();

            tableView.ReloadData();
        }

        [Export("tableView:numberOfRowsInSection:")]
        public int RowsInSection(UITableView tableview, int section)
        {
            return 1;
        }

        [Export("tableView:cellForRowAtIndexPath:")]
        // ReSharper disable once ParameterHidesMember
        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            const string cellName = "LanguageCell";
            var cell = tableView.DequeueReusableCell(cellName) as SettingsCell ?? new SettingsCell();
            cell.UpdateCell();
            return cell;
        }

        /// <summary>
        /// Localize the strings in this controller
        /// </summary>
        private void Localize()
        {
            _header.Text = _trans.GetString("SettingsController_Header");
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        // ReSharper disable once ParameterHidesMember
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row == 0)
            {
                var prompt = new UIAlertView(string.Empty, _trans.GetString("Popup_SelectLanguage"), null,
                    _trans.GetString("Misc_Cancel"),
                    new[]
                    {
                        _trans.GetString("Misc_Lang_Bokmaal"), _trans.GetString("Misc_Lang_Nynorsk"),
                        _trans.GetString("Misc_Lang_English")
                    })
                {
                    AlertViewStyle = UIAlertViewStyle.Default
                };


                prompt.Clicked += (s, b) =>
                {
                    if (b.ButtonIndex == 0)
                    {
                    }
                    else if (b.ButtonIndex == 1)
                    {
                        AppContext.CurrentCulture = "nb-NO";
                        AppContext.CurrentLanguage = "1044";
                    }
                    else if (b.ButtonIndex == 2)
                    {
                        AppContext.CurrentCulture = "nn-NO";
                        AppContext.CurrentLanguage = "2068";
                    }
                    else if (b.ButtonIndex == 3)
                    {
                        AppContext.CurrentCulture = "en";
                        AppContext.CurrentLanguage = "1033";
                    }
					Util.Util.SetLanguage();
                    Localize();
                    tableView.ReloadData();
                };
                prompt.Show();
            }
        }
    }
}
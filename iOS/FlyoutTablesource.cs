using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace AltinnApp.iOS
{
    public class FlyoutTablesource : UITableViewSource
    {
        private readonly CorePlatform.Translate _trans;

        public FlyoutTablesource()
        {
            _trans = new CorePlatform.Translate();
        }


        public override int RowsInSection(UITableView tableview, int section)
        {
            return 5;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            string cellIdentifier = "Cell";
            string cellLabel = string.Empty;


            switch (indexPath.Row)
            {
                case 0:

                    cellIdentifier = "Inbox";
                    cellLabel = _trans.GetString("MessageboxController_InboxHeader");
                    break;

                case 1:
                    cellIdentifier = "Archive";
                    cellLabel = _trans.GetString("MessageboxController_ArchiveHeader");
                    break;
                case 2:
                    cellIdentifier = "Settings";
                    cellLabel = _trans.GetString("SettingsController_Header");
                    break;
                case 3:
                    cellIdentifier = "Contact";
                    cellLabel = _trans.GetString("ContactController_Header");
                    break;
                case 4:
                    cellIdentifier = "Feedback";
                    cellLabel = _trans.GetString("FeedbackController_Header");
                    break;
            }

            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            var selectedBackgroundView = new UIView(cell.Frame) { BackgroundColor = UIColor.FromRGB(73, 73, 73) };
            cell.SelectedBackgroundView = selectedBackgroundView;

			cell.BackgroundColor = UIColor.FromRGB (108, 108, 108);
			var backgroundView = new UIView(cell.Frame) { BackgroundColor = UIColor.FromRGB(108, 108, 108) };
            cell.BackgroundView = backgroundView;


			cell.ContentView.BackgroundColor = UIColor.FromRGB(108, 108, 108);


            cell.TextLabel.Text = cellLabel;

            return cell;
        }
    }
}
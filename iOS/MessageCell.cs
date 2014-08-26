using System;
using MonoTouch.UIKit;

namespace AltinnApp.iOS
{
    public partial class MessageCell : UITableViewCell
    {
        public MessageCell(IntPtr handle) : base(handle)
        {
        }

        public MessageCell(string cellId) : base(UITableViewCellStyle.Default, cellId)
        {
        }

        public void UpdateCell(string from, string subject, string date)
        {
            _from.Text = from;
            _subject.Text = subject;
            _date.Text = date;
        }
    }
}
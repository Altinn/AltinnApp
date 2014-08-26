using System.Collections.Generic;
using System.Linq;
using AltinnApp.Core.Util;

namespace AltinnApp.Core.Models
{
    /// <summary>
    /// The model holding the latest retrieved state of the message box
    /// </summary>
    public class Messagebox
    {
        /// <summary>
        /// Default constructor for the messagebox
        /// </summary>
        public Messagebox()
        {
            Messages = new List<Message>();
            UnfilteredMessages = new List<Message>();
        }

        /// <summary>
        /// Gets or sets the list of messages for the current user
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Gets or sets the currently selected message
        /// </summary>
        public Message CurrentMessage { get; set; }

        public List<Message> UnfilteredMessages { get; set; }

        /// <summary>
        /// Search through the current local message box in a "type-ahead" manner, filters and sets the current message box to the search result
        /// </summary>
        /// <param name="p"></param>
        public void Search(string p)
        {
            List<Message> mess = UnfilteredMessages;
            var list =
                mess.Where(
                    m =>
                        m.Subject.ToLower().Contains(p.ToLower()) ||
                        m.ServiceOwner.ToLower().Contains(p.ToLower()));
            List<Message> filtered = list.ToList();
            Messages = filtered;
        }

        public int Unread { get; set; }
        public Constants.MMBType Type { get; set; }
    }
}
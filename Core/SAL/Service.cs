using System;
using System.Collections.Generic;
using System.Linq;
using AltinnApp.Core.DAL;
using AltinnApp.Core.Models;
using AltinnApp.Core.Util;
using System.Threading.Tasks;

namespace AltinnApp.Core.SAL
{
    /// <summary>
    ///     The Service class acts as a single point of entry to retrieve data
    /// </summary>
    public class Service
    {
        public Service()
        {
            _dal = new Data();
        }

        /// <summary>
        ///     The Data Access Layer to use
        /// </summary>
        private Data _dal;

        public bool UseOneInbox = false;

        /// <summary>
        /// Gets or sets the Data DAL to use
        /// </summary>
        protected internal Data DAL
        {
            get { return _dal; }
            set { _dal = value; }
        }

        public async Task<Profile> GetProfile()
        {
            if (Session.Profile == null)
            {
                string profileJSON = await _dal.GetProfileJSON();

                Session.Profile = Serializer.DeserializeProfile(profileJSON);

                var parts = new List<string>(Session.Profile.Name.Split(' '));

                if (parts.Count >= 2)
                {
                    Session.Profile.LastName = parts[0];

                    Session.Profile.FirstName = parts[1];

                    if (parts.Count >= 3)
                    {
                        Session.Profile.FirstName += " " + parts[2];
                    }
                }
            }
            return Session.Profile;
        }

        /// <summary>
        ///     Takes the common message format gets the specific fields
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public async Task<Message> GetMessage(Message m)
        {
            //Only the URI is required to retrieve the message
            var theMessage = new Message();
            string message = await _dal.GetMessageJSON(m.Links.Self.Href);
            if (!string.IsNullOrEmpty(message))
            {
                theMessage = Serializer.DeserializeMessage(message);
                theMessage = AdjustMessage(theMessage);
            }
            return theMessage;
        }

        public async Task<Message> GetMessage(string messageid)
        {
            //Only the URI is required to retrieve the message
            var theMessage = new Message();
            string requestUrl = String.Format(AppContext.RESTURL, Session.SelectedOrg.OrganizationNumber) + "/" +
                                messageid;
            string message = await _dal.GetMessageJSON(requestUrl);
            if (!string.IsNullOrEmpty(message))
            {
                theMessage = Serializer.DeserializeMessage(message);
                theMessage = AdjustMessage(theMessage);
            }
            return theMessage;
        }

        private Message CorrectCorresAttachment(Message theMessage)
        {
            try
            {
                //Some attachments is missing the file ending
                if (theMessage.Links.Attachment != null)
                {
                    if (!string.IsNullOrEmpty(theMessage.Links.Attachment[0].Name))
                    {
                        string name = theMessage.Links.Attachment[0].Name;

                        if (name.IndexOf('.') > 0)
                        {
                            //string fileEnding = name.Substring (name.IndexOf ('.') + 1);
                            string fileending = name.Substring(name.IndexOf('.') + 1);
                            theMessage.Links.Attachment[0].FileEnding = fileending;

                            if (fileending == "pdf")
                            {
                                theMessage.Links.Attachment[0].Mimetype = "application/pdf";
                            }
                            else if (fileending == "html")
                            {
                                theMessage.Links.Attachment[0].Mimetype = "text/html";
                            }
                            else if (fileending == "xml")
                            {
                                theMessage.Links.Attachment[0].Mimetype = "text/xml";
                            }
                            else
                            {
                                theMessage.Links.Attachment[0].Mimetype = "";
                            }
                        }
                        else if (name.IndexOf('.') < 0)
                        {
                            //If the file ending is missing we attempt to add "pdf" since in most cases it is. Some tax return attachments are missing the file ending
                            theMessage.Links.Attachment[0].Name += ".pdf";
                            theMessage.Links.Attachment[0].FileEnding = "pdf";
                            theMessage.Links.Attachment[0].Mimetype = "application/pdf";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logg("Failed to adjust corres mime-type: " + ex);
            }
            return theMessage;
        }

        public Message AdjustMessage(Message theMessage)
        {
            //Set the downloaded flag so we know that it is done
            theMessage.IsDownloaded = true;

            //Correct the date from the API per client request
            if (theMessage.Type == Constants.MessageType.Correspondence.ToString())
            {
                theMessage = CorrectCorresAttachment(theMessage);
            }
            //theMessage = CorrectDate (theMessage);


            return theMessage;
        }

        public async Task<List<Organization>> SearchActorRemote(string searchValue)
        {
            var mess = new List<Organization>();

            try
            {
                string messageJSON = await _dal.SearchActor(searchValue);
                if (!string.IsNullOrEmpty(messageJSON))
                {
                    mess = Serializer.DeserializeOrganizations(messageJSON);
                }
            }
            catch (Exception e)
            {
                Logger.Logg("Failed to de-serialize the messagebox:" + e);
                throw new AppException("0", ErrorMessages.SerializationError);
            }

            return mess;
        }

        /// <summary>
        /// Search through the message box on the server, filters and sets the current message box to the search result
        /// </summary>
        /// <param name="p"></param>
        /// <param name="type"></param>
        public async Task<List<Message>> Search(string p, Constants.MMBType type)
        {
            var mess = new List<Message>();

            try
            {
                string messageJSON = await _dal.Search(p, type);
                if (!string.IsNullOrEmpty(messageJSON))
                {
                    mess = Serializer.DeserializeMessagebox(messageJSON);
                }
            }
            catch (Exception e)
            {
                Logger.Logg("Failed to de-serialize the messagebox:" + e);
                throw new AppException("0", ErrorMessages.SerializationError);
            }

            return mess;
        }

        /// <summary>
        ///     Gets the attachment at the specified URI
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<byte[]> GetAttachment(string uri)
        {
            return await _dal.GetAttachment(uri);
        }

        /// <summary>
        ///     Gets the attachment at the specified URI
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<string> GetAttachmentAsString(string uri)
        {
            return await _dal.GetAttachmentAsString(uri);
        }

        /// <summary>
        ///     Retrieves the contents of the message box based on the user credentials of the currently logged in user
        ///     and populates the local model of the message box
        /// </summary>
        /// <param name="type">The type of messages we want, i.e Messagebox or Archive</param>
        /// <param name="skipSize">Max result per API call is 50, use skip to get the next 50</param>
        /// <returns></returns>
        public async Task<List<Message>> PopulateMessagebox(Constants.MMBType type, int skipSize)
        {
            if (AppContext.Domain == null)
            {
            }

            //Setup the Profile
            Session.Profile = await GetProfile();

            if (Session.Profile == null)
            {
                throw new AppException("0", "Failed to get profile");
            }

            //Setup organizations
            Session.Organizations = await PopulateOrganizations();

            if (Session.Organizations == null)
                throw new AppException("0", "Failed to get Organizations");

            string url = string.Format(AppContext.RESTURL, Session.SelectedOrg.OrganizationNumber);

            //Setup messagebox

            string messagesJson = await _dal.GetMessageBox(type, skipSize, url);
            var mess = new List<Message>();

            if (messagesJson == null)
                throw new AppException("0", "Failed to get Organizations");

            try
            {
                if (!string.IsNullOrEmpty(messagesJson))
                {
                    mess = Serializer.DeserializeMessagebox(messagesJson);
                }
            }
            catch (Exception e)
            {
                Logger.Logg("Failed to de-serialize the messagebox:" + e);
                throw new AppException("0", ErrorMessages.SerializationError);
            }

            if (!UseOneInbox)
            {
                switch (type)
                {
                    case Constants.MMBType.Messagebox:
                        if (Session.mmb == null)
                        {
                            Session.mmb = new Messagebox {Messages = new List<Message>()};
                        }
                        Session.mmb.Messages.AddRange(mess);
                        Session.mmb.UnfilteredMessages = Session.mmb.Messages.ToList();
                        Session.mmb.Unread =
                            Session.mmb.Messages.Count(m => m.Status == Constants.NO.Nob.Status.Unread);
                        break;

                    case Constants.MMBType.Archive:
                        if (Session.mab == null)
                        {
                            Session.mab = new Messagebox {Messages = new List<Message>()};
                        }
                        Session.mab.Messages.AddRange(mess);
                        Session.mab.UnfilteredMessages = Session.mab.Messages.ToList();
                        Session.mab.Unread =
                            Session.mab.Messages.Count(m => m.Status == Constants.NO.Nob.Status.Unread);
                        break;
                }
            }
            else
            {
                if (Session.mmb == null)
                {
                    Session.mmb = new Messagebox {Messages = new List<Message>()};
                }
                Session.mmb.Messages.AddRange(mess);
                Session.mmb.UnfilteredMessages = Session.mmb.Messages.ToList();
                Session.mmb.Unread =
                    Session.mmb.Messages.Count(m => m.Status == Constants.NO.Nob.Status.Unread);
            }

            return mess;
        }

        public async Task<List<Organization>> PopulateOrganizations(int skipSize)
        {
            string organizationsJson = await _dal.GetOrganizationsJSON(skipSize);
            var orgs = new List<Organization>();

            if (!string.IsNullOrEmpty(organizationsJson))
            {
                //Have to de-serialize the JSON
                orgs = Serializer.DeserializeOrganizations(organizationsJson);
            }

            Session.Organizations.AddRange(orgs);
            Session.UnfilteredOrganizations = Session.Organizations;

            return Session.Organizations;
        }

        /// <summary>
        ///     Gets a list of all possible reportees for this user
        /// </summary>
        /// <returns> List of reportees</returns>
        public async Task<List<Organization>> PopulateOrganizations()
        {
            if (Session.Organizations == null)
            {
                string organizationsJson = await _dal.GetOrganizationsJSON();
                var orgs = new List<Organization>();

                if (!string.IsNullOrEmpty(organizationsJson))
                {
                    //Have to de-serialize the JSON
                    orgs = Serializer.DeserializeOrganizations(organizationsJson);
                }


                //Add yourself as well to the reportee list
                if (Session.Profile != null)
                {
                    var yourSelf = new Organization
                    {
                        Name = Session.Profile.Name,
                        OrganizationNumber = "my"
                    };
                    orgs.Add(yourSelf);
                    Session.SelectedOrg = yourSelf;
                }
                Session.Organizations = orgs;
                Session.UnfilteredOrganizations = Session.Organizations;
            }
            return Session.Organizations;
        }
    }
}
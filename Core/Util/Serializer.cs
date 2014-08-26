using System;
using System.Collections.Generic;
using System.Linq;
using AltinnApp.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AltinnApp.Core.Util
{
    /// <summary>
    ///     Class for de-serializing JSON data
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        ///     De-serialize the HAL-JSON returned from the API to an object
        /// </summary>
        /// <param name="jsonMessage"></param>
        /// <returns></returns>
        public static Message DeserializeMessage(string jsonMessage)
        {
            Message newMessage = null;

            try
            {
                newMessage = JsonConvert.DeserializeObject<Message>(jsonMessage);
            }
            catch (Exception e)
            {
                Logger.Logg("Failed to de-serialize the message: ", e);
            }

            return newMessage;
        }

        /// <summary>
        ///     De-serialize the messagebox from JSON
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<Message> DeserializeMessagebox(string json)
        {
            var mess = new List<Message>();
            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    var outer = JsonConvert.DeserializeObject<OuterJson>(json);
                    JObject innerObjectJson = outer._embedded;
                    JToken messages = innerObjectJson["messages"];
                    mess = messages.ToObject<IList<Message>>().ToList();
                }
            }
            catch (Exception e)
            {
                Logger.Logg("Failed to deserialize Json: ", e);
            }

            return mess;
        }

        /// <summary>
        ///     De-serialize the list of reportees from JSON
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<Organization> DeserializeOrganizations(string json)
        {
            var orgs = new List<Organization>();
            try
            {
                var outerOrg = JsonConvert.DeserializeObject<OuterJson>(json);
                JObject innerObjectJsonOrg = outerOrg._embedded;
                JToken orgMessages = innerObjectJsonOrg["organizations"];
                orgs = orgMessages.ToObject<IList<Organization>>().ToList();
            }
            catch (Exception e)
            {
                Logger.Logg("Failed to deserialize Json: ", e);
            }

            return orgs;
        }

        public static Profile DeserializeProfile(string profileJSON)
        {
            var prof = new Profile();
            try
            {
                prof = JsonConvert.DeserializeObject<Profile>(profileJSON);
                //JObject innerObjectJsonOrg = outerOrg._embedded;
                //JToken orgMessages = innerObjectJsonOrg["organizations"];
                //prof = orgMessages.ToObject<Profile>();
            }
            catch (Exception e)
            {
                Logger.Logg("Failed to deserialize Json: ", e);
            }
            return prof;
        }
    }
}
using System.Collections.Generic;

namespace AltinnApp.Core.Models
{
    /// <summary>
    /// Model for an organization
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// Gets or sets the unique organization number
        /// </summary>
        public string OrganizationNumber { get; set; }

        /// <summary>
        /// Gets or sets the mostly unique organization name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of organization
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///  Gets or sets the links related to the organization
        /// </summary>
        public List<string> Links { get; set; }

        /// <summary>
        ///     Check if this Organization has the same name as o.
        ///     If a check for strict uniqueness is required then a comparison on OrganizationNumber is more appropiate
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(Organization o)
        {
            if (o != null && Name == o.Name)
            {
                return true;
            }

            return false;
        }
    }
}
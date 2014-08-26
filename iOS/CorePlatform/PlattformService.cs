using AltinnApp.Core.DAL;
using AltinnApp.Core.SAL;

namespace AltinnApp.iOS.CorePlatform
{
    /// <summary>
    /// The platform specific subclass of the Service class
    /// </summary>
    public partial class PlattformService : Core.SAL.Service
    {
        private static Service _service;

        /// <summary>
        /// Subclass of the Serivce to override the DAL used
        /// </summary>
        public PlattformService()
        {
            DAL = new PlattformDAL();
        }

        public static Service Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new PlattformService();
                }
                return _service;
            }
            //set { _service = value; }
        }
    }
}
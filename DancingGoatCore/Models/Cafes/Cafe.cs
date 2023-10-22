using DancingGoat.Models;

namespace CMS.DocumentEngine.Types.DancingGoatCore
{
    /// <summary>
    /// Specification of Cafe members and IContact interface relationship.
    /// </summary>
    public partial class Cafe : IContact
    {
        public new string Name
        {
            get
            {
                return null;
            }
        }

        public string Phone
        {
            get
            {
                return null;
            }
        }

        public string Email
        {
            get
            {
                return "";
            }
        }

        public string ZIP
        {
            get
            {
                return null;
            }
        }

        public string Street
        {
            get
            {
                return null;
            }
        }

        public string City
        {
            get
            {
                return null;
            }
        }

        public string Country
        {
            get
            {
                return null;
            }
        }
    }
}
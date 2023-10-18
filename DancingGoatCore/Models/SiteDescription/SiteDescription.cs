using DancingGoat.Models.SiteDescription;

namespace CMS.DocumentEngine.Types.DancingGoatCore
{
    public partial class SiteDescription : ISiteDescription
    {
        public new string LongSiteContent
        {
            get
            {
                return Fields.LongSiteContent;
            }
        }
    }
}
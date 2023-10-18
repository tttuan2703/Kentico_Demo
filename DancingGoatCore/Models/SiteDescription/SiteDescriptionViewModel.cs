using Microsoft.Extensions.Localization;

namespace DancingGoat.Models.SiteDescription
{
    public class SiteDescriptionViewModel
    {
        public string LongContent { get; set; }

        public static SiteDescriptionViewModel GetViewModel(ISiteDescription siteDescription)
        {
            var model = new SiteDescriptionViewModel();
            model.LongContent = siteDescription.LongSiteContent;

            return model;
        }
    }
}

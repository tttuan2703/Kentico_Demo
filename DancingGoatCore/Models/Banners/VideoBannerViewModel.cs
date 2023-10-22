using DancingGoat.Models.SiteDescription;
using Microsoft.Extensions.Localization;

namespace DancingGoat.Models.Banners
{
    public class VideoBannerViewModel
    {
        public string VideoPath { get; set; }

        public static VideoBannerViewModel GetViewModel(IVideoBannerDescription siteDescription)
        {
            var model = new VideoBannerViewModel();
            model.VideoPath = siteDescription._VideoPath;

            return model;
        }
    }
}

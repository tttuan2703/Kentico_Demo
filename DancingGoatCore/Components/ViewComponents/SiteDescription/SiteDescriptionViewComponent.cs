using CMS.DocumentEngine.Types.DancingGoatCore;
using DancingGoat.Models.SiteDescription;
using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Components.ViewComponents.SiteDescription
{
    public class SiteDescriptionViewComponent : ViewComponent
    {
        private readonly SiteDescriptionRepository siteDescriptionRepository;

        public SiteDescriptionViewComponent(SiteDescriptionRepository siteDescriptionRepository)
        {
            this.siteDescriptionRepository = siteDescriptionRepository;
        }

        public IViewComponentResult Invoke()
        {
            var contact = siteDescriptionRepository.GetSiteDescriptionContact();
            var model = SiteDescriptionViewModel.GetViewModel(contact);

            return View("~/Components/ViewComponents/SiteDescription/Default.cshtml", model);
        }
    }
}
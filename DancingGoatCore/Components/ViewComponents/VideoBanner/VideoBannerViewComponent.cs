using CMS.DocumentEngine.Types.DancingGoatCore;
using DancingGoat.Models;
using DancingGoat.Models.Banners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DancingGoat.ViewComponents.VideoBanner
{
    public class VideoBannerViewComponent : ViewComponent
    {
        private readonly VideoBannerRepository contactRepository;


        public VideoBannerViewComponent(VideoBannerRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }


        public IViewComponentResult Invoke()
        {
            var contact = contactRepository.GetVideoBannerContact();
            var model = VideoBannerViewModel.GetViewModel(contact);
            
            return View("~/Components/ViewComponents/VideoBanner/Default.cshtml", model);
        }
    }
}

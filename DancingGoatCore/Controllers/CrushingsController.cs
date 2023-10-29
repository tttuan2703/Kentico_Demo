using CMS.DocumentEngine.Types.DancingGoatCore;
using DancingGoat;
using DancingGoat.Controllers;
using DancingGoat.Helpers;
using DancingGoat.Models;
using DancingGoat.Models.Crushings;
using DancingGoat.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;

[assembly: RegisterPageRoute(ProductSection.CLASS_NAME, typeof(CrushingsController), Path = ContentItemIdentifiers.CRUSHINGS)]
namespace DancingGoat.Controllers
{
    public class CrushingsController : Controller
    {
        private readonly CrushingRepository crushingRepository;
        private readonly ICalculationService calculationService;
        private readonly IPageUrlRetriever pageUrlRetriever;

        public CrushingsController(CrushingRepository crushingRepository, ICalculationService calculationService, IPageUrlRetriever pageUrlRetriever)
        {
            this.crushingRepository = crushingRepository;
            this.calculationService = calculationService;
            this.pageUrlRetriever = pageUrlRetriever;
        }

        // GET: Crushings
        public ActionResult Index([FromServices] IStringLocalizer<SharedResources> localizer, int? page = 0)
        {
            var pageSize = 6;
            var items = GetFilteredCrushings(null);

            // paging items
            var pageCount = (double)items.Count() / pageSize;
            pageCount = pageCount % 2 == 0 ? (int)pageCount : (int)pageCount + 1;

            var pageIndex = page == 0 ? page : (page - 1) * pageSize;
            items = items.Skip((int)pageIndex).Take(pageSize);

            var filter = new CrushingFilterViewModel();
            filter.Load(localizer);

            return View(new ProductListViewModel
            {
                Filter = filter,
                Items = items,
                PageSelected = (int)page,
                Paginations = (int)pageCount
            });
        }

        // POST: Filter
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Crushings/Filter")]
        public ActionResult Filter(CrushingFilterViewModel filter)
        {
            if (!Request.IsAjaxRequest())
            {
                return NotFound();
            }

            var items = GetFilteredCrushings(filter);

            return PartialView("CrushingList", items);
        }

        private IEnumerable<ProductListItemViewModel> GetFilteredCrushings(IRepositoryFilter filter)
        {
            var crushings = crushingRepository.GetCrushings(filter);

            var items = crushings.Select(
                crushing => new ProductListItemViewModel(
                    crushing,
                    calculationService.CalculatePrice(crushing.SKU),
                    crushing.Product.PublicStatus?.PublicStatusDisplayName,
                    pageUrlRetriever,
                    crushing.Horsepower,
                    crushing.OperatingWeight,
                    crushing.BladeCapacity)
                );
            return items;
        }
    }
}

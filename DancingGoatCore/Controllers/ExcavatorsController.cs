using CMS.DocumentEngine.Types.DancingGoatCore;
using DancingGoat;
using DancingGoat.Controllers;
using DancingGoat.Helpers;
using DancingGoat.Models;
using DancingGoat.Models.Crushings;
using DancingGoat.Models.Excavators;
using DancingGoat.Services;
using DocumentFormat.OpenXml.Wordprocessing;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;

[assembly: RegisterPageRoute(ProductSection.CLASS_NAME, typeof(ExcavatorsController), Path = ContentItemIdentifiers.EXCAVATORS)]

namespace DancingGoat.Controllers
{
    public class ExcavatorsController : Controller
    {
        private readonly ExcavatorRepository excavatorRepository;
        private readonly ICalculationService calculationService;
        private readonly IPageUrlRetriever pageUrlRetriever;

        public ExcavatorsController(ExcavatorRepository excavatorRepository, ICalculationService calculationService, IPageUrlRetriever pageUrlRetriever)
        {
            this.excavatorRepository = excavatorRepository;
            this.calculationService = calculationService;
            this.pageUrlRetriever = pageUrlRetriever;
        }

        // GET: Excavators
        public ActionResult Index([FromServices] IStringLocalizer<SharedResources> localizer, int? page = 1)
        {
            var pageSize = 6;
            var items = GetFilteredExcavators(null);

            // paging items
            var pageCount = (double)items.Count() / pageSize;
            pageCount = pageCount % 2 == 0 ? (int)pageCount : (int)pageCount + 1;

            var pageIndex = page ?? 1;
            pageIndex = pageIndex == 1 ? pageIndex : (pageIndex - 1) * pageSize;

            items = items.Skip(pageIndex).Take(pageSize);

            var filter = new ExcavatorFilterViewModel();
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
        [Route("Excavators/Filter")]
        public ActionResult Filter(ExcavatorFilterViewModel filter)
        {
            if (!Request.IsAjaxRequest())
            {
                return NotFound();
            }

            var items = GetFilteredExcavators(filter);

            return PartialView("ExcavatorList", items);
        }

        private IEnumerable<ProductListItemViewModel> GetFilteredExcavators(IRepositoryFilter filter)
        {
            var excavators = excavatorRepository.GetExcavators(filter);

            var items = excavators.Select(
                excavator => new ProductListItemViewModel(
                    excavator,
                    calculationService.CalculatePrice(excavator.SKU),
                    excavator.Product.PublicStatus?.PublicStatusDisplayName,
                    pageUrlRetriever,
                    excavator.Horsepower,
                    excavator.OperatingWeight,
                    excavator.BladeCapacity)
                );
            return items;
        }
    }
}
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;

using DancingGoat;
using DancingGoat.Controllers;
using DancingGoat.Helpers;
using DancingGoat.Models;
using DancingGoat.Models.Dozers;
using DancingGoat.Services;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[assembly: RegisterPageRoute(ProductSection.CLASS_NAME, typeof(DozersController), Path = ContentItemIdentifiers.DOZERS)]
namespace DancingGoat.Controllers
{
    public class DozersController : Controller
    {
        private readonly DozerRepository dozerRepository;
        private readonly ICalculationService calculationService;
        private readonly IPageUrlRetriever pageUrlRetriever;


        public DozersController(DozerRepository dozerRepository, ICalculationService calculationService, IPageUrlRetriever pageUrlRetriever)
        {
            this.dozerRepository = dozerRepository;
            this.calculationService = calculationService;
            this.pageUrlRetriever = pageUrlRetriever;
        }


        // GET: Dozers
        public ActionResult Index([FromServices] IStringLocalizer<SharedResources> localizer, int? page = 0)
        {
            var pageSize = 6;
            var items = GetFilteredDozers(null);

            // paging items
            var pageCount = (double)items.Count() / pageSize;
            pageCount = pageCount % 2 == 0 ? (int)pageCount : (int)pageCount + 1;

            var pageIndex = page == 0 ? page : (page - 1) * pageSize;
            items = items.Skip((int)pageIndex).Take(pageSize);

            var filter = new DozerFilterViewModel();
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
        [Route("Dozers/Filter")]
        public ActionResult Filter(DozerFilterViewModel filter)
        {
            if (!Request.IsAjaxRequest())
            {
                return NotFound();
            }

            var items = GetFilteredDozers(filter);

            return PartialView("DozerList", items);
        }


        private IEnumerable<ProductListItemViewModel> GetFilteredDozers(IRepositoryFilter filter)
        {
            var dozers = dozerRepository.GetDozers(filter);

            var items = dozers.Select(
                dozer => new ProductListItemViewModel(
                    dozer,
                    calculationService.CalculatePrice(dozer.SKU),
                    dozer.Product.PublicStatus?.PublicStatusDisplayName,
                    pageUrlRetriever,
                    dozer.Horsepower,
                    dozer.OperatingWeight,
                    dozer.BladeCapacity)
                );
            return items;
        }
    }
}
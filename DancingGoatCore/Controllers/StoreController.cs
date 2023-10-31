using CMS.DocumentEngine.Types.DancingGoatCore;
using DancingGoat.Controllers;
using DancingGoat.Models;
using DancingGoat.Models.Crushings;
using DancingGoat.Models.Dozers;
using DancingGoat.Models.Excavators;
using DancingGoat.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[assembly: RegisterPageRoute(StoreSection.CLASS_NAME, typeof(StoreController))]

namespace DancingGoat.Controllers
{
    public class StoreController : Controller
    {
        private readonly IPageDataContextRetriever dataRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly ICalculationService calculationService;

        private readonly DozerRepository dozerRepository;
        private readonly ExcavatorRepository excavatorRepository;
        private readonly CrushingRepository crushingRepository;

        public StoreController(IPageDataContextRetriever dataRetriever,
            ICalculationService calculationService,
            IPageUrlRetriever pageUrlRetriever,
            DozerRepository dozerRepository,
            ExcavatorRepository excavatorRepository,
            CrushingRepository crushingRepository)
        {
            this.dataRetriever = dataRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
            this.calculationService = calculationService;
            this.dozerRepository = dozerRepository;
            this.excavatorRepository = excavatorRepository;
            this.crushingRepository = crushingRepository;
        }

        public ActionResult Index()
        {
            var dozerProducts = GetFilteredDozers(null);
            var excavatorProducts = GetFilteredExcavators(null);
            var crushingProducts = GetFilteredCrushings(null);

            var model = new StoreViewModel
            {
                DozerProducts = dozerProducts,
                ExcavatorProducts = excavatorProducts,
                CrushingProducts = crushingProducts,
            };

            return View(model);
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
                ).Take(4);
            return items;
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
                ).Take(4);
            return items;
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
                ).Take(4);
            return items;
        }
    }
}
using CMS.DocumentEngine;
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
        private readonly ProductRepository productRepository;
        private readonly PublicStatusRepository publicStatusRepository;
        private readonly ManufacturerRepository manufacturerRepository;
        private readonly HotTipsRepository hotTipsRepository;

        private readonly DozerRepository dozerRepository;
        private readonly ExcavatorRepository excavatorRepository;
        private readonly CrushingRepository crushingRepository;

        public StoreController(IPageDataContextRetriever dataRetriever,
            ICalculationService calculationService,
            ProductRepository productRepository,
            PublicStatusRepository publicStatusRepository,
            ManufacturerRepository manufacturerRepository,
            HotTipsRepository hotTipsRepository,
            IPageUrlRetriever pageUrlRetriever,
            DozerRepository dozerRepository,
            ExcavatorRepository excavatorRepository,
            CrushingRepository crushingRepository)
        {
            this.dataRetriever = dataRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
            this.calculationService = calculationService;
            this.productRepository = productRepository;
            this.publicStatusRepository = publicStatusRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.hotTipsRepository = hotTipsRepository;
            this.dozerRepository = dozerRepository;
            this.excavatorRepository = excavatorRepository;
            this.crushingRepository = crushingRepository;
        }

        public ActionResult Index()
        {
            var dozerProducts = GetFilteredDozers(null);
            var excavatorProducts = GetFilteredExcavators(null);
            var crushingProducts = GetFilteredCrushings(null);
            var featuredProducts = GetBestsellers();
            var manufacturers = GetManufacturers();
            var hotTipProducts = GetHotTipProducts();

            var model = new StoreViewModel
            {
                DozerProducts = dozerProducts,
                ExcavatorProducts = excavatorProducts,
                CrushingProducts = crushingProducts,
                FeaturedProducts = featuredProducts,
                Manufacturers = manufacturers,
                HotTipProducts = hotTipProducts
            };

            return View(model);
        }

        private IEnumerable<ProductListItemViewModel> GetBestsellers()
        {
            const string bestsellerCodename = "DancingGoatCore.Bestseller";
            var status = publicStatusRepository.GetByName(bestsellerCodename);

            if (status == null)
            {
                return Enumerable.Empty<ProductListItemViewModel>();
            }

            var products = productRepository.GetProductsByStatus(status.PublicStatusID);

            return products.Select(
                product => new ProductListItemViewModel(
                    product,
                    calculationService.CalculatePrice(product.SKU),
                    status.PublicStatusDisplayName,
                    pageUrlRetriever
                )
            );
        }

        private IEnumerable<ManufactureListItemViewModel> GetManufacturers()
        {
            var manufacturers = manufacturerRepository.GetManufacturers(ContentItemIdentifiers.MANUFACTURERS);

            return manufacturers.Select(manufacturer => new ManufactureListItemViewModel(manufacturer, pageUrlRetriever));
        }

        private IEnumerable<ProductListItemViewModel> GetHotTipProducts()
        {
            var hotTips = hotTipsRepository.GetHotTipProducts(dataRetriever.Retrieve<TreeNode>().Page.NodeAliasPath);

            return hotTips.Select(
                product => new ProductListItemViewModel(
                    product,
                    calculationService.CalculatePrice(product.SKU),
                    publicStatusRepository.GetById(product.SKU.SKUPublicStatusID)?.PublicStatusDisplayName,
                    pageUrlRetriever
                )
            );
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
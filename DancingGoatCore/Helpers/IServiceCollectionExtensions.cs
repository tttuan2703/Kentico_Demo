using CMS.DocumentEngine.Types.DancingGoatCore;
using DancingGoat.Infrastructure;
using DancingGoat.Models;
using DancingGoat.Models.Crushings;
using DancingGoat.Models.Dozers;
using DancingGoat.Models.Excavators;
using DancingGoat.PageTemplates;
using DancingGoatCore.SchedulerTasks;
using DancingGoat.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DancingGoat
{
    public static class IServiceCollectionExtensions
    {
        public static void AddDancingGoatServices(this IServiceCollection services)
        {
            AddViewComponentServices(services);

            AddRepositories(services);

            services.AddSingleton<TypedProductViewModelFactory>();
            services.AddSingleton<TypedSearchItemViewModelFactory>();
            services.AddSingleton<ICalculationService, CalculationService>();
            services.AddSingleton<ICheckoutService, CheckoutService>();
            services.AddSingleton<ICartShoppingService, CartShoppingService>();
            services.AddSingleton<RepositoryCacheHelper>();
            services.AddSingleton<UpdateUserStatusTask>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddSingleton<ArticleRepository>();
            services.AddSingleton<ContactRepository>();
            services.AddSingleton<CountryRepository>();
            services.AddSingleton<NavigationRepository>();
            services.AddSingleton<SocialLinkRepository>();
            services.AddSingleton<DozerRepository>();
            services.AddSingleton<CrushingRepository>();
            services.AddSingleton<ExcavatorRepository>();
            services.AddSingleton<ProductRepository>();
            services.AddSingleton<VariantRepository>();
            services.AddSingleton<HotTipsRepository>();
            services.AddSingleton<CustomerAddressRepository>();
            services.AddSingleton<ShippingOptionRepository>();
            services.AddSingleton<PaymentMethodRepository>();
            services.AddSingleton<MediaFileRepository>();
            services.AddSingleton<ReferenceRepository>();
            services.AddSingleton<HomeRepository>();
            services.AddSingleton<OrderRepository>();
            services.AddSingleton<VideoBannerRepository>();
            services.AddSingleton<SiteDescriptionRepository>();
        }

        private static void AddViewComponentServices(IServiceCollection services)
        {
            services.AddSingleton<ArticleWithSidebarPageTemplateService>();
            services.AddSingleton<ArticlePageTemplateService>();
        }
    }
}
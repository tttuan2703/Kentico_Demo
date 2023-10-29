using System.Linq;
using System.Net;

using CMS.Ecommerce;
using CMS.Globalization;

using DancingGoat.Models;

using Kentico.Content.Web.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderRepository orderRepository;
        private readonly IShoppingService shoppingService;
        private readonly ICurrencyInfoProvider currencyInfoProvider;
        private readonly IOrderStatusInfoProvider orderStatusInfoProvider;
        private readonly ICountryInfoProvider countryInfoProvider;
        private readonly IStateInfoProvider stateInfoProvider;


        private CustomerInfo CurrentCustomer => shoppingService.GetCurrentCustomer();


        public OrdersController(OrderRepository orderRepository, IShoppingService shoppingService, ICurrencyInfoProvider currencyInfoProvider, IOrderStatusInfoProvider orderStatusInfoProvider,
            ICountryInfoProvider countryInfoProvider, IStateInfoProvider stateInfoProvider)
        {
            this.orderRepository = orderRepository;
            this.shoppingService = shoppingService;
            this.currencyInfoProvider = currencyInfoProvider;
            this.orderStatusInfoProvider = orderStatusInfoProvider;
            this.countryInfoProvider = countryInfoProvider;
            this.stateInfoProvider = stateInfoProvider;
        }


        // GET: Orders
        [Authorize]
        public ActionResult Index()
        {
            var currentCustomer = shoppingService.GetCurrentCustomer();
            var orders = currentCustomer != null
                ? orderRepository.GetByCustomerId(currentCustomer.CustomerID).Select(order => new OrdersListViewModel(order, currencyInfoProvider, orderStatusInfoProvider))
                : Enumerable.Empty<OrdersListViewModel>();

            return View(orders);
        }


        // GET: Orders/OrderDetail
        [Authorize]
        public ActionResult OrderDetail(int? orderID)
        {
            if (orderID == null)
            {
                return RedirectToAction("Index");
            }

            // Validate other repository
            var order = orderRepository.GetById(orderID.Value);
            if ((order == null) || (order.OrderCustomerID != CurrentCustomer?.CustomerID))
            {
                return RedirectToAction("Error", "HttpErrors", new { code = (int)HttpStatusCode.NotFound });
            }

            // prepare order detail
            var currency = currencyInfoProvider.Get(order.OrderCurrencyID);
            var result = new OrderDetailViewModel(currency.CurrencyFormatString
                , order, orderStatusInfoProvider, currencyInfoProvider, countryInfoProvider, stateInfoProvider);

            return View(result);
        }
    }
}
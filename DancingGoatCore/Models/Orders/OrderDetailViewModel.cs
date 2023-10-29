using CMS.Ecommerce;
using CMS.Globalization;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DancingGoat.Models
{
    public class OrderDetailViewModel
    {
        private readonly string currencyFormatString;


        public string InvoiceNumber { get; set; }

        public decimal TotalPrice { get; set; }

        public string StatusName { get; set; }

        public OrderAddressViewModel OrderAddress { get; set; }

        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }


        public OrderDetailViewModel(string currencyFormatString, OrderInfo order,
            IOrderStatusInfoProvider orderStatusInfoProvider,
            ICurrencyInfoProvider currencyInfoProvider,
            ICountryInfoProvider countryInfoProvider,
            IStateInfoProvider stateInfoProvider)
        {
            if (string.IsNullOrEmpty(currencyFormatString))
            {
                throw new ArgumentException($"{nameof(currencyFormatString)} is not defined.");
            }

            this.currencyFormatString = currencyFormatString;

            var currency = currencyInfoProvider.Get(order.OrderCurrencyID);

            InvoiceNumber = order.OrderInvoiceNumber;
            TotalPrice = order.OrderTotalPrice;
            StatusName = orderStatusInfoProvider.Get(order.OrderStatusID)?.StatusDisplayName;
            OrderAddress = new OrderAddressViewModel(order.OrderBillingAddress, countryInfoProvider, stateInfoProvider);
            OrderItems = OrderItemInfoProvider.GetOrderItems(order.OrderID).Select(orderItem =>
            {
                return new OrderItemViewModel
                {
                    SKUID = orderItem.OrderItemSKUID,
                    SKUName = orderItem.OrderItemSKUName,
                    SKUImagePath = string.IsNullOrEmpty(orderItem.OrderItemSKU.SKUImagePath) ? null : new FileUrl(orderItem.OrderItemSKU.SKUImagePath, true).WithSizeConstraint(SizeConstraint.MaxWidthOrHeight(70)).RelativePath,
                    TotalPriceInMainCurrency = orderItem.OrderItemTotalPriceInMainCurrency,
                    UnitCount = orderItem.OrderItemUnitCount,
                    UnitPrice = orderItem.OrderItemUnitPrice
                };
            });
        }

        public string FormatPrice(decimal price)
        {
            return string.Format(currencyFormatString, price);
        }
    }
}
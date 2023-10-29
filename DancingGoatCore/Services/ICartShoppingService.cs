using CMS.Ecommerce;
using CMS.Globalization;
using DancingGoat.Models;
using System.Collections.Generic;

namespace DancingGoat.Services
{
    public interface ICartShoppingService
    {
        CartViewModel PrepareCartShoppingViewModel();

        DeliveryDetailsViewModel PrepareDeliveryDetailsViewModel(CustomerViewModel customer = null, BillingAddressViewModel billingAddress = null, ShippingOptionViewModel shippingOption = null);

        IEnumerable<StateInfo> GetCountryStates(int countryId);

        bool IsShippingOptionValid(int shippingOptionId);

        bool IsCountryValid(int countryId);

        bool IsStateValid(int countryId, int? stateId);

        AddressInfo GetAddress(int addressID);

        PreviewViewModel PreparePreviewViewModel(PaymentMethodViewModel paymentMethod = null);

        bool IsPaymentMethodValid(int paymentMethodId);
    }
}
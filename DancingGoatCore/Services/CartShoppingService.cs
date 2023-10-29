using CMS.Ecommerce;
using CMS.Globalization;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DancingGoat.Services
{
    public class CartShoppingService : ICartShoppingService
    {
        private readonly CMS.Ecommerce.IShoppingService mShoppingService;
        private readonly PaymentMethodRepository mPaymentMethodRepository;
        private readonly ShippingOptionRepository mShippingOptionRepository;
        private readonly CountryRepository mCountryRepository;
        private readonly CustomerAddressRepository mAddressRepository;

        public CartShoppingService(CMS.Ecommerce.IShoppingService shoppingService,
                                CustomerAddressRepository addressRepository,
                                PaymentMethodRepository paymentMethodRepository,
                                ShippingOptionRepository shippingOptionRepository,
                                CountryRepository countryRepository)
        {
            mShoppingService = shoppingService;
            mPaymentMethodRepository = paymentMethodRepository;
            mShippingOptionRepository = shippingOptionRepository;
            mCountryRepository = countryRepository;
            mAddressRepository = addressRepository;
        }

        public CartViewModel PrepareCartShoppingViewModel()
        {
            var cart = mShoppingService.GetCurrentShoppingCart();
            var appliedCouponCodes = cart.CouponCodes?.AllAppliedCodes?.Select(x => x.Code); // Get selected coupons
            var cartViewModel = new CartViewModel(cart);

            return cartViewModel;
        }

        public DeliveryDetailsViewModel PrepareDeliveryDetailsViewModel(CustomerViewModel customer = null, BillingAddressViewModel billingAddress = null, ShippingOptionViewModel shippingOption = null)
        {
            var cart = mShoppingService.GetCurrentShoppingCart();
            var countries = CreateCountryList();
            var shippingOptions = CreateShippingOptionList();

            customer = customer ?? new CustomerViewModel(cart.Customer);

            var addresses = (cart.Customer != null)
                ? mAddressRepository.GetByCustomerId(cart.Customer.CustomerID)
                : Enumerable.Empty<AddressInfo>();

            var billingAddresses = new SelectList(addresses, nameof(AddressInfo.AddressID), nameof(AddressInfo.AddressName));

            billingAddress = billingAddress ?? new BillingAddressViewModel(mShoppingService.GetBillingAddress(), countries, mCountryRepository, billingAddresses);
            shippingOption = shippingOption ?? new ShippingOptionViewModel(cart.ShippingOption, shippingOptions, cart.IsShippingNeeded);

            billingAddress.BillingAddressCountryStateSelector.Countries = billingAddress.BillingAddressCountryStateSelector.Countries ?? countries;
            billingAddress.BillingAddressSelector = billingAddress.BillingAddressSelector ?? new AddressSelectorViewModel { Addresses = billingAddresses };
            shippingOption.ShippingOptions = shippingOptions;

            var viewModel = new DeliveryDetailsViewModel
            {
                Customer = customer,
                BillingAddress = billingAddress,
                ShippingOption = shippingOption
            };

            return viewModel;
        }

        public IEnumerable<StateInfo> GetCountryStates(int countryId)
        {
            return mCountryRepository.GetCountryStates(countryId).ToList();
        }

        public bool IsShippingOptionValid(int shippingOptionId)
        {
            var shippingOptions = mShippingOptionRepository.GetAllEnabled().ToList();

            return shippingOptions.Exists(s => s.ShippingOptionID == shippingOptionId);
        }

        public bool IsCountryValid(int countryId)
        {
            return mCountryRepository.GetCountry(countryId) != null;
        }

        public bool IsStateValid(int countryId, int? stateId)
        {
            var states = mCountryRepository.GetCountryStates(countryId).ToList();

            return (states.Count < 1) || states.Exists(s => s.StateID == stateId);
        }

        public AddressInfo GetAddress(int addressID)
        {
            return mAddressRepository.GetById(addressID);
        }

        public PreviewViewModel PreparePreviewViewModel(PaymentMethodViewModel paymentMethod = null)
        {
            var cart = mShoppingService.GetCurrentShoppingCart();
            var billingAddress = mShoppingService.GetBillingAddress();
            var shippingOption = cart.ShippingOption;
            var paymentMethods = CreatePaymentMethodList(cart);

            paymentMethod = paymentMethod ?? new PaymentMethodViewModel(cart.PaymentOption, paymentMethods);

            // PaymentMethods are excluded from automatic binding and must be recreated manually after post action
            paymentMethod.PaymentMethods = paymentMethod.PaymentMethods ?? paymentMethods;

            var deliveryDetailsModel = new DeliveryDetailsViewModel
            {
                Customer = new CustomerViewModel(cart.Customer),
                BillingAddress = new BillingAddressViewModel(billingAddress, null, mCountryRepository),
                ShippingOption = new ShippingOptionViewModel(shippingOption, null, cart.IsShippingNeeded)
            };

            var cartModel = new CartViewModel(cart);

            var viewModel = new PreviewViewModel
            {
                CartModel = cartModel,
                DeliveryDetails = deliveryDetailsModel,
                ShippingName = shippingOption?.ShippingOptionDisplayName ?? "",
                PaymentMethod = paymentMethod
            };

            return viewModel;
        }

        public bool IsPaymentMethodValid(int paymentMethodId)
        {
            var cart = mShoppingService.GetCurrentShoppingCart();
            var paymentMethods = GetApplicablePaymentMethods(cart).ToList();

            return paymentMethods.Exists(p => p.PaymentOptionID == paymentMethodId);
        }

        #region Private Methods

        private SelectList CreateCountryList()
        {
            var allCountries = mCountryRepository.GetAllCountries();
            return new SelectList(allCountries, "CountryID", "CountryDisplayName");
        }

        private SelectList CreateShippingOptionList()
        {
            var shippingOptions = mShippingOptionRepository.GetAllEnabled();
            var cart = mShoppingService.GetCurrentShoppingCart();

            var selectList = shippingOptions.Select(s =>
            {
                var shippingPrice = mShoppingService.CalculateShippingOptionPrice(s);
                var currency = cart.Currency;

                return new SelectListItem
                {
                    Value = s.ShippingOptionID.ToString(),
                    Text = $"{s.ShippingOptionDisplayName} ({String.Format(currency.CurrencyFormatString, shippingPrice)})"
                };
            });

            return new SelectList(selectList, "Value", "Text");
        }

        private SelectList CreatePaymentMethodList(ShoppingCartInfo cart)
        {
            var paymentMethods = GetApplicablePaymentMethods(cart);

            return new SelectList(paymentMethods, "PaymentOptionID", "PaymentOptionDisplayName");
        }

        private IEnumerable<PaymentOptionInfo> GetApplicablePaymentMethods(ShoppingCartInfo cart)
        {
            return mPaymentMethodRepository.GetAll().Where(paymentMethod => PaymentOptionInfoProvider.IsPaymentOptionApplicable(cart, paymentMethod));
        }

        #endregion Private Methods
    }
}
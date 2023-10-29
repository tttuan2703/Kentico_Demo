using CMS.Base;
using CMS.Core;
using CMS.Ecommerce;
using CMS.Globalization;
using CMS.Helpers;
using DancingGoat.Constants;
using DancingGoat.Models;
using DancingGoat.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DancingGoat.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly CMS.Ecommerce.IShoppingService shoppingService;
        private readonly IEventLogService eventLogService;
        private readonly ICartShoppingService cartShoppingService;
        private readonly ContactRepository contactRepository;
        private readonly ProductRepository productRepository;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly ISKUInfoProvider skuInfoProvider;
        private readonly ICountryInfoProvider countryInfoProvider;
        private readonly IStateInfoProvider stateInfoProvider;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IStringLocalizer<SharedResources> localizer;

        public ShoppingController(CMS.Ecommerce.IShoppingService shoppingService,
                                    ICartShoppingService cartShoppingService,
                                    ContactRepository contactRepository,
                                    ProductRepository productRepository,
                                    IPageUrlRetriever pageUrlRetriever,
                                    ISKUInfoProvider skuInfoProvider,
                                    ICountryInfoProvider countryInfoProvider,
                                    IStateInfoProvider stateInfoProvider,
                                    SignInManager<ApplicationUser> signInManager,
                                    IStringLocalizer<SharedResources> localizer,
                                    IEventLogService eventLogService)
        {
            this.cartShoppingService = cartShoppingService;
            this.shoppingService = shoppingService;
            this.contactRepository = contactRepository;
            this.productRepository = productRepository;
            this.pageUrlRetriever = pageUrlRetriever;
            this.skuInfoProvider = skuInfoProvider;
            this.countryInfoProvider = countryInfoProvider;
            this.stateInfoProvider = stateInfoProvider;
            this.signInManager = signInManager;
            this.localizer = localizer;
            this.eventLogService = eventLogService;
        }

        public ActionResult ShoppingCart()
        {
            var viewModel = cartShoppingService.PrepareCartShoppingViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(CartItemUpdateModel item)
        {
            // if no login
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect(ContentItemIdentifiers.LOGIN);
            }

            if (ModelState.IsValid)
            {
                shoppingService.AddItemToCart(item.SKUID, item.Units);
            }

            return RedirectToAction("ShoppingCart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShoppingCart(CartItemUpdateModel item, string cartAction)
        {
            switch (cartAction)
            {
                case CartActionConstant.UPDATE:
                    return UpdateItem(item);

                case CartActionConstant.DELETE:
                    return RemoveItem(item);

                case CartActionConstant.DELETE_ALL:
                    return RemoveAllItems();

                default: break;
            }

            var viewModel = cartShoppingService.PrepareCartShoppingViewModel();

            return View(viewModel);
        }

        public ActionResult ItemDetail(int skuId)
        {
            var product = productRepository.GetProductForSKU(skuId);

            if (product == null)
            {
                return NotFound();
            }

            var path = pageUrlRetriever.Retrieve(product).RelativePath;

            return Redirect(path);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShoppingCartCheckout()
        {
            try
            {
                var cart = shoppingService.GetCurrentShoppingCart();
                var validationErrors = ShoppingCartInfoProvider.ValidateShoppingCart(cart);

                cart.Evaluate();

                if (!validationErrors.Any())
                {
                    var customer = GetCustomerFromAuthenticatedUser();
                    if (customer == null)
                    {
                        return Redirect(ContentItemIdentifiers.LOGIN);
                    }

                    shoppingService.SetCustomer(customer);
                    shoppingService.SaveCart();
                    return RedirectToAction("DeliveryDetails");
                }

                // Fill model state with errors from the check result
                ProcessCheckResult(validationErrors);

                var viewModel = cartShoppingService.PrepareCartShoppingViewModel();
                return View("ShoppingCart", viewModel);
            }
            catch (Exception ex)
            {
                eventLogService.LogException("ShoppingController", "ShoppingCartCheckout", ex);
                return NotFound();
            }
        }

        public ActionResult DeliveryDetails()
        {
            var cart = shoppingService.GetCurrentShoppingCart();
            if (cart.IsEmpty)
            {
                return RedirectToAction("ShoppingCart");
            }

            var viewModel = cartShoppingService.PrepareDeliveryDetailsViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeliveryDetails(DeliveryDetailsViewModel model, [FromServices] IStringLocalizer<SharedResources> localizer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var viewModel = cartShoppingService.PrepareDeliveryDetailsViewModel
                        (model.Customer, model.BillingAddress, model.ShippingOption);
                    return View(viewModel);
                }

                // Update model state
                ValidateDeliveryDetailsViewModel(model);

                // Handle customer
                var customer = GetCustomerFromAuthenticatedUser() ?? new CustomerInfo();
                bool emailCanBeChanged = !User.Identity.IsAuthenticated || string.IsNullOrWhiteSpace(customer.CustomerEmail);
                model.Customer.ApplyToCustomer(customer, emailCanBeChanged);
                shoppingService.SetCustomer(customer);

                // Handle bill
                var modelAddressID = model.BillingAddress.BillingAddressSelector?.AddressID ?? 0;
                var billingAddress = cartShoppingService.GetAddress(modelAddressID) ?? new AddressInfo();

                model.BillingAddress.ApplyTo(billingAddress);
                billingAddress.AddressPersonalName = $"{customer.CustomerFirstName} {customer.CustomerLastName}";

                // Save
                shoppingService.SetBillingAddress(billingAddress);
                shoppingService.SetShippingOption(model.ShippingOption?.ShippingOptionID ?? 0);

                return RedirectToAction("PreviewAndPay");
            }
            catch (Exception ex)
            {
                eventLogService.LogException("ShoppingController", "DeliveryDetails", ex);
                return NotFound();
            }
        }

        public ActionResult PreviewAndPay()
        {
            if (shoppingService.GetCurrentShoppingCart().IsEmpty)
            {
                return RedirectToAction("ShoppingCart");
            }

            var viewModel = cartShoppingService.PreparePreviewViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PreviewAndPay(PreviewViewModel model, [FromServices] IStringLocalizer<SharedResources> localizer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var viewModel = cartShoppingService.PreparePreviewViewModel(model.PaymentMethod);
                    return View("PreviewAndPay", viewModel);
                }

                // Validate current cart again
                var cart = shoppingService.GetCurrentShoppingCart();
                if (cart.IsEmpty)
                {
                    ModelState.AddModelError("cart.empty", localizer["Please add some item to your shopping cart."]);

                    var viewModel = cartShoppingService.PreparePreviewViewModel(model.PaymentMethod);
                    return View("PreviewAndPay", viewModel);
                }

                // Validate cart payment valid
                if (!cartShoppingService.IsPaymentMethodValid(model.PaymentMethod.PaymentMethodID))
                {
                    ModelState.AddModelError("PaymentMethod.PaymentMethodID", localizer["Select payment method"]);
                }
                else
                {
                    shoppingService.SetPaymentOption(model.PaymentMethod.PaymentMethodID);
                }

                // Validate other valid
                var validator = new CreateOrderValidator(cart, skuInfoProvider, countryInfoProvider, stateInfoProvider);
                if (!validator.Validate())
                {
                    ProcessCheckResult(validator.Errors);
                }

                // Successful: Save other
                shoppingService.CreateOrder();
                return RedirectToAction("ThankYou");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("cart.createordererror", ex.Message);
                var viewModel = cartShoppingService.PreparePreviewViewModel(model.PaymentMethod);
                return View("PreviewAndPay", viewModel);
            }
        }

        public ActionResult ThankYou()
        {
            var companyContact = contactRepository.GetCompanyContact();

            var viewModel = new ThankYouViewModel
            {
                Phone = companyContact.Phone
            };

            return View(viewModel);
        }

        #region Private Methods

        private ActionResult UpdateItem(CartItemUpdateModel item)
        {
            if (ModelState.IsValid)
            {
                shoppingService.UpdateItemQuantity(item.ID, item.Units);
            }

            var cartViewModel = cartShoppingService.PrepareCartShoppingViewModel();
            return View("ShoppingCart", cartViewModel);
        }

        private ActionResult RemoveItem(CartItemUpdateModel item)
        {
            shoppingService.RemoveItemFromCart(item.ID);

            var cartViewModel = cartShoppingService.PrepareCartShoppingViewModel();
            return View("ShoppingCart", cartViewModel);
        }

        private ActionResult RemoveAllItems()
        {
            shoppingService.RemoveAllItemsFromCart();

            var cartViewModel = cartShoppingService.PrepareCartShoppingViewModel();
            return View("ShoppingCart", cartViewModel);
        }

        private void ProcessCheckResult(IEnumerable<IValidationError> validationErrors)
        {
            var itemErrors = validationErrors
                .OfType<ShoppingCartItemValidationError>()
                .GroupBy(g => g.SKUId);

            foreach (var errorGroup in itemErrors)
            {
                var errors = errorGroup
                    .Select(e => e.GetMessage())
                    .Join(" ");

                ModelState.AddModelError(errorGroup.Key.ToString(), errors);
            }
        }

        private CustomerInfo GetCustomerFromAuthenticatedUser()
        {
            var cart = shoppingService.GetCurrentShoppingCart();
            var customer = cart.Customer;

            if (customer != null)
            {
                return customer;
            }

            var user = cart.User;

            return user != null ? CustomerHelper.MapToCustomer(user) : null;
        }

        private ModelStateDictionary ValidateDeliveryDetailsViewModel(DeliveryDetailsViewModel model)
        {
            // Check the selected shipping option
            bool isShippingNeeded = shoppingService.GetCurrentShoppingCart().IsShippingNeeded;
            if (isShippingNeeded && !cartShoppingService.IsShippingOptionValid(model.ShippingOption.ShippingOptionID))
            {
                ModelState.AddModelError("ShippingOption.ShippingOptionID", localizer["Please select a delivery method"]);
            }

            // Check if the billing address's country and state are valid
            var countryStateViewModel = model.BillingAddress.BillingAddressCountryStateSelector;
            if (!cartShoppingService.IsCountryValid(countryStateViewModel.CountryID))
            {
                countryStateViewModel.CountryID = 0;
                ModelState.AddModelError("BillingAddress.BillingAddressCountryStateSelector.CountryID", localizer["The Country field is required"]);
            }
            else if (!cartShoppingService.IsStateValid(countryStateViewModel.CountryID, countryStateViewModel.StateID))
            {
                countryStateViewModel.StateID = 0;
                ModelState.AddModelError("BillingAddress.BillingAddressCountryStateSelector.StateID", localizer["The State field is required"]);
            }

            return ModelState;
        }

        #endregion Private Methods
    }
}

using CMS.Ecommerce;

using DancingGoat.Models;
using DancingGoat.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DancingGoat.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly CMS.Ecommerce.IShoppingService shoppingService;
        private readonly ICheckoutService checkoutService;


        public CouponController(CMS.Ecommerce.IShoppingService shoppingService, ICheckoutService checkoutService)
        {
            this.shoppingService = shoppingService;
            this.checkoutService = checkoutService;
        }


        // GET: Coupon/Show
        public ActionResult Show()
        {
            var viewModel = checkoutService.PrepareCartViewModel();

            return PartialView("_CouponCodeEdit", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCouponCode(CouponCodesUpdateModel model, [FromServices] IStringLocalizer<SharedResources> localizer)
        {
            string couponCode = model.NewCouponCode?.Trim();
            if (string.IsNullOrEmpty(couponCode) || !shoppingService.AddCouponCode(couponCode))
            {
                return Json(new { couponCodeInvalidMessage = localizer["Discount coupon is not valid"].Value });
            }

            return new EmptyResult();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveCouponCode(CouponCodesUpdateModel model)
        {
            shoppingService.RemoveCouponCode(model.RemoveCouponCode);

            return new EmptyResult();
        }
    }
}
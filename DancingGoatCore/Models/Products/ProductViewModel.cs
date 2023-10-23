using System;
using System.Collections.Generic;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Ecommerce;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class ProductViewModel
    {
        public ITypedProductViewModel TypedProduct { get; }


        public ProductCatalogPrices PriceDetail { get; }


        public IEnumerable<ProductOptionCategoryViewModel> ProductOptionCategories { get; }


        public int SelectedVariantID { get; set; }


        public bool IsInStock { get; }


        public bool AllowSale { get; }


        public string Name { get; }


        public string Description { get; }


        public string ShortDescription { get; }


        public int SKUID { get; }


        public string ImagePath { get; }


        public string Horsepower { get; set; }


        public string OperatingWeight { get; set; }


        public string BladeCapacity { get; set; }


        public ProductViewModel(SKUTreeNode productPage, ProductCatalogPrices priceDetail,
            ITypedProductViewModel typedProductViewModel = null)
        {
            // Set page information
            Name = productPage.DocumentName;
            Description = productPage.DocumentSKUDescription;
            ShortDescription = productPage.DocumentSKUShortDescription;

            // Set SKU information
            var sku = productPage.SKU;
            SKUID = sku.SKUID;
            ImagePath = string.IsNullOrEmpty(sku.SKUImagePath) ? null : new FileUrl(sku.SKUImagePath, true).WithSizeConstraint(SizeConstraint.MaxWidthOrHeight(400)).RelativePath;
            IsInStock = sku.SKUTrackInventory == TrackInventoryTypeEnum.Disabled ||
                        sku.SKUAvailableItems > 0;
            AllowSale = IsInStock || !sku.SKUSellOnlyAvailable;

            // Set additional info
            TypedProduct = typedProductViewModel;
            PriceDetail = priceDetail;
        }


        public ProductViewModel(SKUTreeNode productPage, ProductCatalogPrices price,
            ITypedProductViewModel typedProductViewModel, ProductVariant defaultVariant,
            IEnumerable<ProductOptionCategoryViewModel> categories, string horsepower = null,
            string operatingWeight = null, string bladeCapacity = null)
            : this(productPage, price, typedProductViewModel)
        {
            if (defaultVariant == null)
            {
                throw new ArgumentNullException(nameof(defaultVariant));
            }

            var variant = defaultVariant.Variant;

            IsInStock = ((variant.SKUTrackInventory == TrackInventoryTypeEnum.Disabled) || (variant.SKUAvailableItems > 0));
            AllowSale = IsInStock || !variant.SKUSellOnlyAvailable;

            SelectedVariantID = variant.SKUID;

            // Variant categories which will be rendered
            ProductOptionCategories = categories;
        }


        public ProductViewModel(Dozer dozer, ProductCatalogPrices priceDetail,
            ITypedProductViewModel typedProductViewModel = null)
        {
            // Set page information
            Name = dozer.DocumentName;
            Description = dozer.DocumentSKUDescription;
            ShortDescription = dozer.DocumentSKUShortDescription;

            // Set SKU information
            var sku = dozer.SKU;
            SKUID = sku.SKUID;
            ImagePath = string.IsNullOrEmpty(sku.SKUImagePath) ? null : new FileUrl(sku.SKUImagePath, true).WithSizeConstraint(SizeConstraint.MaxWidthOrHeight(400)).RelativePath;
            IsInStock = sku.SKUTrackInventory == TrackInventoryTypeEnum.Disabled ||
                        sku.SKUAvailableItems > 0;
            AllowSale = IsInStock || !sku.SKUSellOnlyAvailable;

            // Set additional info
            TypedProduct = typedProductViewModel;
            PriceDetail = priceDetail;

            Horsepower = dozer.Horsepower;
            OperatingWeight = dozer.OperatingWeight;
            BladeCapacity = dozer.BladeCapacity;
        }


        public ProductViewModel(Dozer dozer, ProductCatalogPrices price,
            ITypedProductViewModel typedProductViewModel, ProductVariant defaultVariant,
            IEnumerable<ProductOptionCategoryViewModel> categories)
            : this(dozer, price, typedProductViewModel)
        {
            if (defaultVariant == null)
            {
                throw new ArgumentNullException(nameof(defaultVariant));
            }

            var variant = defaultVariant.Variant;

            IsInStock = ((variant.SKUTrackInventory == TrackInventoryTypeEnum.Disabled) || (variant.SKUAvailableItems > 0));
            AllowSale = IsInStock || !variant.SKUSellOnlyAvailable;

            SelectedVariantID = variant.SKUID;

            // Variant categories which will be rendered
            ProductOptionCategories = categories;
        }


        public ProductViewModel(Excavator excavator, ProductCatalogPrices priceDetail,
            ITypedProductViewModel typedProductViewModel = null)
        {
            // Set page information
            Name = excavator.DocumentName;
            Description = excavator.DocumentSKUDescription;
            ShortDescription = excavator.DocumentSKUShortDescription;

            // Set SKU information
            var sku = excavator.SKU;
            SKUID = sku.SKUID;
            ImagePath = string.IsNullOrEmpty(sku.SKUImagePath) ? null : new FileUrl(sku.SKUImagePath, true).WithSizeConstraint(SizeConstraint.MaxWidthOrHeight(400)).RelativePath;
            IsInStock = sku.SKUTrackInventory == TrackInventoryTypeEnum.Disabled ||
                        sku.SKUAvailableItems > 0;
            AllowSale = IsInStock || !sku.SKUSellOnlyAvailable;

            // Set additional info
            TypedProduct = typedProductViewModel;
            PriceDetail = priceDetail;

            Horsepower = excavator.Horsepower;
            OperatingWeight = excavator.OperatingWeight;
            BladeCapacity = excavator.BladeCapacity;
        }


        public ProductViewModel(Excavator excavator, ProductCatalogPrices price,
            ITypedProductViewModel typedProductViewModel, ProductVariant defaultVariant,
            IEnumerable<ProductOptionCategoryViewModel> categories)
            : this(excavator, price, typedProductViewModel)
        {
            if (defaultVariant == null)
            {
                throw new ArgumentNullException(nameof(defaultVariant));
            }

            var variant = defaultVariant.Variant;

            IsInStock = ((variant.SKUTrackInventory == TrackInventoryTypeEnum.Disabled) || (variant.SKUAvailableItems > 0));
            AllowSale = IsInStock || !variant.SKUSellOnlyAvailable;

            SelectedVariantID = variant.SKUID;

            // Variant categories which will be rendered
            ProductOptionCategories = categories;
        }


        public ProductViewModel(Crushing crushing, ProductCatalogPrices priceDetail,
            ITypedProductViewModel typedProductViewModel = null)
        {
            // Set page information
            Name = crushing.DocumentName;
            Description = crushing.DocumentSKUDescription;
            ShortDescription = crushing.DocumentSKUShortDescription;

            // Set SKU information
            var sku = crushing.SKU;
            SKUID = sku.SKUID;
            ImagePath = string.IsNullOrEmpty(sku.SKUImagePath) ? null : new FileUrl(sku.SKUImagePath, true).WithSizeConstraint(SizeConstraint.MaxWidthOrHeight(400)).RelativePath;
            IsInStock = sku.SKUTrackInventory == TrackInventoryTypeEnum.Disabled ||
                        sku.SKUAvailableItems > 0;
            AllowSale = IsInStock || !sku.SKUSellOnlyAvailable;

            // Set additional info
            TypedProduct = typedProductViewModel;
            PriceDetail = priceDetail;

            Horsepower = crushing.Horsepower;
            OperatingWeight = crushing.OperatingWeight;
            BladeCapacity = crushing.BladeCapacity;
        }


        public ProductViewModel(Crushing crushing, ProductCatalogPrices price,
            ITypedProductViewModel typedProductViewModel, ProductVariant defaultVariant,
            IEnumerable<ProductOptionCategoryViewModel> categories)
            : this(crushing, price, typedProductViewModel)
        {
            if (defaultVariant == null)
            {
                throw new ArgumentNullException(nameof(defaultVariant));
            }

            var variant = defaultVariant.Variant;

            IsInStock = ((variant.SKUTrackInventory == TrackInventoryTypeEnum.Disabled) || (variant.SKUAvailableItems > 0));
            AllowSale = IsInStock || !variant.SKUSellOnlyAvailable;

            SelectedVariantID = variant.SKUID;

            // Variant categories which will be rendered
            ProductOptionCategories = categories;
        }
    }
}
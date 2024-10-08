//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at https://docs.xperience.io/.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CMS;
using CMS.Base;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.DocumentEngine.Types.DancingGoatCore;

[assembly: RegisterDocumentType(Construction.CLASS_NAME, typeof(Construction))]

namespace CMS.DocumentEngine.Types.DancingGoatCore
{
	/// <summary>
	/// Represents a content item of type Construction.
	/// </summary>
	public partial class Construction : SKUTreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "DancingGoatCore.Construction";


		/// <summary>
		/// The instance of the class that provides extended API for working with Construction fields.
		/// </summary>
		private readonly ConstructionFields mFields;


		/// <summary>
		/// The instance of the class that provides extended API for working with SKU fields.
		/// </summary>
		private readonly ProductFields mProduct;

		#endregion


		#region "Properties"

		/// <summary>
		/// CoffeeID.
		/// </summary>
		[DatabaseIDField]
		public int ConstructionID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("ConstructionID"), 0);
			}
			set
			{
				SetValue("ConstructionID", value);
			}
		}


		/// <summary>
		/// Horsepower.
		/// </summary>
		[DatabaseField]
		public string Horsepower
		{
			get
			{
				return ValidationHelper.GetString(GetValue("Horsepower"), @"");
			}
			set
			{
				SetValue("Horsepower", value);
			}
		}


		/// <summary>
		/// Operating Weight.
		/// </summary>
		[DatabaseField]
		public string OperatingWeight
		{
			get
			{
				return ValidationHelper.GetString(GetValue("OperatingWeight"), @"");
			}
			set
			{
				SetValue("OperatingWeight", value);
			}
		}


		/// <summary>
		/// Blade Capacity.
		/// </summary>
		[DatabaseField]
		public string BladeCapacity
		{
			get
			{
				return ValidationHelper.GetString(GetValue("BladeCapacity"), @"");
			}
			set
			{
				SetValue("BladeCapacity", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with Construction fields.
		/// </summary>
		[RegisterProperty]
		public ConstructionFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with SKU fields.
		/// </summary>
        [RegisterProperty]
		public ProductFields Product
		{
			get
			{
				return mProduct;
			}
		}


		/// <summary>
		/// Provides extended API for working with Construction fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class ConstructionFields : AbstractHierarchicalObject<ConstructionFields>
		{
			/// <summary>
			/// The content item of type Construction that is a target of the extended API.
			/// </summary>
			private readonly Construction mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="ConstructionFields" /> class with the specified content item of type Construction.
			/// </summary>
			/// <param name="instance">The content item of type Construction that is a target of the extended API.</param>
			public ConstructionFields(Construction instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// CoffeeID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.ConstructionID;
				}
				set
				{
					mInstance.ConstructionID = value;
				}
			}


			/// <summary>
			/// Horsepower.
			/// </summary>
			public string Horsepower
			{
				get
				{
					return mInstance.Horsepower;
				}
				set
				{
					mInstance.Horsepower = value;
				}
			}


			/// <summary>
			/// Operating Weight.
			/// </summary>
			public string OperatingWeight
			{
				get
				{
					return mInstance.OperatingWeight;
				}
				set
				{
					mInstance.OperatingWeight = value;
				}
			}


			/// <summary>
			/// Blade Capacity.
			/// </summary>
			public string BladeCapacity
			{
				get
				{
					return mInstance.BladeCapacity;
				}
				set
				{
					mInstance.BladeCapacity = value;
				}
			}
		}


		/// <summary>
		/// Provides extended API for working with SKU fields.
		/// </summary>
        [RegisterAllProperties]
		public class ProductFields : AbstractHierarchicalObject<ProductFields>
		{
		    /// <summary>
			/// The content item of type <see cref="Construction" /> that is a target of the extended API.
			/// </summary>
			private readonly Construction mInstance;


			/// <summary>
			/// The <see cref="PublicStatusInfo" /> object related to product based on value of <see cref="SKUInfo.SKUPublicStatusID" /> column. 
			/// </summary>
			private PublicStatusInfo mPublicStatus = null;


			/// <summary>
			/// The <see cref="ManufacturerInfo" /> object related to product based on value of <see cref="SKUInfo.SKUManufacturerID" /> column. 
			/// </summary>
			private ManufacturerInfo mManufacturer = null;


			/// <summary>
			/// The <see cref="DepartmentInfo" /> object related to product based on value of <see cref="SKUInfo.SKUDepartmentID" /> column. 
			/// </summary>
			private DepartmentInfo mDepartment = null;


			/// <summary>
			/// The <see cref="SupplierInfo" /> object related to product based on value of <see cref="SKUInfo.SKUSupplierID" /> column. 
			/// </summary>
			private SupplierInfo mSupplier = null;


			/// <summary>
			/// The <see cref="TaxClassInfo" /> object related to product based on value of <see cref="SKUInfo.SKUTaxClassID" /> column. 
			/// </summary>
			private TaxClassInfo mTaxClass = null;


			/// <summary>
			/// The <see cref="BrandInfo" /> object related to product based on value of <see cref="SKUInfo.SKUBrandID" /> column. 
			/// </summary>
			private BrandInfo mBrand = null;


			/// <summary>
			/// The <see cref="CollectionInfo" /> object related to product based on value of <see cref="SKUInfo.SKUCollectionID" /> column. 
			/// </summary>
			private CollectionInfo mCollection = null;


			/// <summary>
			/// The shortcut to <see cref="SKUInfo" /> object which is a target of this extended API.
			/// </summary>
			private SKUInfo SKU
			{
				get 
				{
					return mInstance.SKU;
				}
			}

						
			/// <summary>
			/// Initializes a new instance of the <see cref="ProductFields" /> class with SKU related fields of type <see cref="Construction" /> .
			/// </summary>
			/// <param name="instance">The content item of type Construction that is a target of the extended API.</param>
			public ProductFields(Construction instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// SKUID.
			/// </summary>
			public int ID
			{
				get
				{
					return (SKU != null) ? SKU.SKUID : 0;
				}
				set
				{
					if (SKU != null)
					{
						SKU.SKUID = value;
					}
				}
			}


			/// <summary>
			/// Allows you to specify the product identifier. You can use this number or string, for example, in your accounting records.
			/// </summary>
			public string SKUNumber
			{
				get
				{
					return (SKU != null) ? SKU.SKUNumber : @"";
				}
				set
				{
					if (SKU != null)
					{
						SKU.SKUNumber = value;
					}
				}
			}


			/// <summary>
			/// Package weight.
			/// </summary>
			public double Weight
			{
				get
				{
					return (SKU != null) ? SKU.SKUWeight : 0;
				}
				set
				{
					if (SKU != null)
					{
						SKU.SKUWeight = value;
					}
				}
			}


			/// <summary>
			/// Package height.
			/// </summary>
			public double Height
			{
				get
				{
					return (SKU != null) ? SKU.SKUHeight : 0;
				}
				set
				{
					if (SKU != null)
					{
						SKU.SKUHeight = value;
					}
				}
			}


			/// <summary>
			/// Package width.
			/// </summary>
			public double Width
			{
				get
				{
					return (SKU != null) ? SKU.SKUWidth : 0;
				}
				set
				{
					if (SKU != null)
					{
						SKU.SKUWidth = value;
					}
				}
			}


			/// <summary>
			/// Package depth.
			/// </summary>
			public double Depth
			{
				get
				{
					return (SKU != null) ? SKU.SKUDepth : 0;
				}
				set
				{
					if (SKU != null)
					{
						SKU.SKUDepth = value;
					}
				}
			}


			/// <summary>
			/// Gets <see cref="PublicStatusInfo" /> object based on value of <see cref="SKUInfo.SKUPublicStatusID" /> column. 
			/// </summary>
			public PublicStatusInfo PublicStatus
			{	
				get
				{
					if (SKU == null)
					{
						return null;
					}

					var id = SKU.SKUPublicStatusID;

				    if ((mPublicStatus == null) && (id > 0))
				    {
                        mPublicStatus = PublicStatusInfo.Provider.Get(id);
				    }

				    return mPublicStatus;
				}
			}


			/// <summary>
			/// Gets <see cref="ManufacturerInfo" /> object based on value of <see cref="SKUInfo.SKUManufacturerID" /> column. 
			/// </summary>
			public ManufacturerInfo Manufacturer
			{	
				get
				{
					if (SKU == null)
					{
						return null;
					}

					var id = SKU.SKUManufacturerID;

				    if ((mManufacturer == null) && (id > 0))
				    {
                        mManufacturer = ManufacturerInfo.Provider.Get(id);
				    }

				    return mManufacturer;
				}
			}


			/// <summary>
			/// Gets <see cref="DepartmentInfo" /> object based on value of <see cref="SKUInfo.SKUDepartmentID" /> column. 
			/// </summary>
			public DepartmentInfo Department
			{	
				get
				{
					if (SKU == null)
					{
						return null;
					}

				    var id = SKU.SKUDepartmentID;

				    if ((mDepartment == null) && (id > 0))
				    {
				        mDepartment = DepartmentInfo.Provider.Get(id);
                    }

					return mDepartment;
				}
			}


			/// <summary>
			/// Gets <see cref="SupplierInfo" /> object based on value of <see cref="SKUInfo.SKUSupplierID" /> column. 
			/// </summary>
			public SupplierInfo Supplier
			{	
				get
				{
					if (SKU == null)
					{
						return null;
					}

					var id = SKU.SKUSupplierID;

				    if ((mSupplier == null) && (id > 0))
				    {
                        mSupplier = SupplierInfo.Provider.Get(id);
                    }

				    return mSupplier;
				}
			}


			/// <summary>
			/// Gets <see cref="TaxClassInfo" /> object based on value of <see cref="SKUInfo.SKUTaxClassID" /> column. 
			/// </summary>
			public TaxClassInfo TaxClass
			{	
				get
				{
					if (SKU == null)
					{
						return null;
					}

					var id = SKU.SKUTaxClassID;

				    if ((mTaxClass == null) && (id > 0))
				    {
						mTaxClass = TaxClassInfo.Provider.Get(id);
				    }
				    
				    return mTaxClass;
				}
			}


			/// <summary>
			/// Gets <see cref="BrandInfo" /> object based on value of <see cref="SKUInfo.SKUBrandID" /> column. 
			/// </summary>
			public BrandInfo Brand
			{	
				get
				{
					if (SKU == null)
					{
						return null;
					}

					var id = SKU.SKUBrandID;

					if ((mBrand == null) && (id > 0))
					{
						mBrand = BrandInfo.Provider.Get(id);
					}

					return mBrand;
				}
			}


			/// <summary>
			/// Gets <see cref="CollectionInfo" /> object based on value of <see cref="SKUInfo.SKUCollectionID" /> column. 
			/// </summary>
			public CollectionInfo Collection
			{	
				get
				{
					if (SKU == null)
					{
						return null;
					}

					var id = SKU.SKUCollectionID;

					if ((mCollection == null) && (id > 0))
					{
						mCollection = CollectionInfo.Provider.Get(id);
					}

					return mCollection;
				}
			}


			/// <summary>
			/// Localized name of product.
			/// </summary>
			public string Name
			{
				get
				{
					return mInstance.DocumentSKUName;
				}
				set
				{
					mInstance.DocumentSKUName = value;
				}
			}


			/// <summary>
			/// Localized description of product.
			/// </summary>
			public string Description
			{
				get
				{
					return mInstance.DocumentSKUDescription;
				}
				set
				{
					mInstance.DocumentSKUDescription = value;
				}
			}


			/// <summary>
			/// Localized short description of product.
			/// </summary>
			public string ShortDescription
			{
				get
				{
					return mInstance.DocumentSKUShortDescription;
				}
				set
				{
					mInstance.DocumentSKUShortDescription = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="Construction" /> class.
		/// </summary>
		public Construction() : base(CLASS_NAME)
		{
			mFields = new ConstructionFields(this);
			mProduct = new ProductFields(this);
		}

		#endregion
	}
}
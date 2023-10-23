using CMS.DocumentEngine.Types.DancingGoatCore;
using Kentico.Content.Web.Mvc;
using System.Collections.Generic;

namespace DancingGoat.Models.Excavators
{
    public class ExcavatorRepository
    {
        private readonly IPageRetriever pageRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcavatorRepository"/> class that returns excavators.
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public ExcavatorRepository(IPageRetriever pageRetriever)
        {
            this.pageRetriever = pageRetriever;
        }

        /// <summary>
        /// Returns an enumerable collection of excavators ordered by the date of publication.
        /// </summary>
        /// <param name="filter">Instance of a product filter.</param>
        /// <param name="count">The number of excavators to return. Use 0 as value to return all records.</param>
        public IEnumerable<Excavator> GetExcavators(IRepositoryFilter filter, int count = 0)
        {
            return pageRetriever.Retrieve<Excavator>(
                query => query
                    .TopN(count)
                        .WhereTrue("SKUEnabled")
                        .Where(filter?.GetWhereCondition())
                        .FilterDuplicates()
                        .OrderByDescending("SKUInStoreFrom"),
                cache => cache
                    .Key($"{nameof(ExcavatorRepository)}|{nameof(GetExcavators)}|{filter?.GetCacheKey()}|{count}")
                    // Include dependency on all pages of this type to flush cache when a new page is created.
                    .Dependencies((_, builder) => builder.Pages(Excavator.CLASS_NAME).ObjectType("ecommerce.sku")));
        }
    }
}
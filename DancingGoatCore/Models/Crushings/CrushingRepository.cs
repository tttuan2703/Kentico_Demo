using CMS.DocumentEngine.Types.DancingGoatCore;
using Kentico.Content.Web.Mvc;
using System.Collections.Generic;

namespace DancingGoat.Models.Crushings
{
    public class CrushingRepository
    {
        private readonly IPageRetriever pageRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrushingRepository"/> class that returns crushings.
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public CrushingRepository(IPageRetriever pageRetriever)
        {
            this.pageRetriever = pageRetriever;
        }

        /// <summary>
        /// Returns an enumerable collection of crushings ordered by the date of publication.
        /// </summary>
        /// <param name="filter">Instance of a product filter.</param>
        /// <param name="count">The number of crushings to return. Use 0 as value to return all records.</param>
        public IEnumerable<Crushing> GetCrushings(IRepositoryFilter filter, int count = 0)
        {
            return pageRetriever.Retrieve<Crushing>(
                query => query
                    .TopN(count)
                        .WhereTrue("SKUEnabled")
                        .Where(filter?.GetWhereCondition())
                        .FilterDuplicates()
                        .OrderByDescending("SKUInStoreFrom"),
                cache => cache
                    .Key($"{nameof(CrushingRepository)}|{nameof(GetCrushings)}|{filter?.GetCacheKey()}|{count}")
                    // Include dependency on all pages of this type to flush cache when a new page is created.
                    .Dependencies((_, builder) => builder.Pages(Crushing.CLASS_NAME).ObjectType("ecommerce.sku")));
        }
    }
}
using CMS.DocumentEngine.Types.DancingGoatCore;
using Kentico.Content.Web.Mvc;
using System.Collections.Generic;

namespace DancingGoat.Models.Dozers
{
    public class DozerRepository
    {
        private readonly IPageRetriever pageRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="DozerRepository"/> class that returns dozers.
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public DozerRepository(IPageRetriever pageRetriever)
        {
            this.pageRetriever = pageRetriever;
        }

        /// <summary>
        /// Returns an enumerable collection of dozers ordered by the date of publication.
        /// </summary>
        /// <param name="filter">Instance of a product filter.</param>
        /// <param name="count">The number of dozers to return. Use 0 as value to return all records.</param>
        public IEnumerable<Dozer> GetDozers(IRepositoryFilter filter, int count = 0)
        {
            return pageRetriever.Retrieve<Dozer>(
                query => query
                    .TopN(count)
                        .WhereTrue("SKUEnabled")
                        .Where(filter?.GetWhereCondition())
                        .FilterDuplicates()
                        .OrderByDescending("SKUInStoreFrom"),
                cache => cache
                    .Key($"{nameof(DozerRepository)}|{nameof(GetDozers)}|{filter?.GetCacheKey()}|{count}")
                    // Include dependency on all pages of this type to flush cache when a new page is created.
                    .Dependencies((_, builder) => builder.Pages(Dozer.CLASS_NAME).ObjectType("ecommerce.sku")));
        }
    }
}
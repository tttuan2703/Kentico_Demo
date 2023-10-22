using Kentico.Content.Web.Mvc;
using System.Linq;

namespace CMS.DocumentEngine.Types.DancingGoatCore
{
    /// <summary>
    /// Represents a collection of site description information.
    /// </summary>
    public class SiteDescriptionRepository
    {
        private readonly IPageRetriever pageRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoBannerRepository"/> class that returns contact information.
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public SiteDescriptionRepository(IPageRetriever pageRetriever)
        {
            this.pageRetriever = pageRetriever;
        }

        /// <summary>
        /// Returns site description information.
        /// </summary>
        public SiteDescription GetSiteDescriptionContact()
        {
            return pageRetriever.Retrieve<SiteDescription>(
                query => query
                    .TopN(1),
                cache => cache
                    .Key($"{nameof(VideoBannerRepository)}|{nameof(GetSiteDescriptionContact)}"))
                .FirstOrDefault();
        }
    }
}
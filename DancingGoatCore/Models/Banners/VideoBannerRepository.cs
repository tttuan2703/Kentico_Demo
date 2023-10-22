using CMS.DocumentEngine.Types.KomatsuDemo;
using Kentico.Content.Web.Mvc;
using System.Linq;

namespace CMS.DocumentEngine.Types.DancingGoatCore
{
    /// <summary>
    /// Represents a collection of site description information.
    /// </summary>
    public class VideoBannerRepository
    {
        private readonly IPageRetriever pageRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoBannerRepository"/> class that returns contact information.
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public VideoBannerRepository(IPageRetriever pageRetriever)
        {
            this.pageRetriever = pageRetriever;
        }

        /// <summary>
        /// Returns site description information.
        /// </summary>
        public VideoBanner GetVideoBannerContact()
        {
            return pageRetriever.Retrieve<VideoBanner>(
                query => query
                    .TopN(1),
                cache => cache
                    .Key($"{nameof(VideoBannerRepository)}|{nameof(GetVideoBannerContact)}"))
                .FirstOrDefault();
        }
    }
}
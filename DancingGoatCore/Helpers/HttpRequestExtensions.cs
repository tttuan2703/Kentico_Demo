
using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Search;
using DancingGoat.Models;
using Microsoft.AspNetCore.Http;

namespace DancingGoat.Helpers
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Headers != null && request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return true;
            }

            return false;
        }

        public static IEnumerable<SearchResultItemModel> FilterKomatsuProduct(this IEnumerable<SearchResultItemModel> searchResultItems)
        {
            var result = new List<SearchResultItemModel>();
            foreach (var searchResultItem in searchResultItems)
            {
                var _search = searchResultItem as SearchResultProductItemModel;
                var typeSearch = _search?.Type;
                if (typeSearch == nameof(Dozer)
                    || typeSearch == nameof(Excavator)
                    || typeSearch == nameof(Crushing))
                {
                    result.Add(searchResultItem);
                }
            }

            return result;
        }

    }
}

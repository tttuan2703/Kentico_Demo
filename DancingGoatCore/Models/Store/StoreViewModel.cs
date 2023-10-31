using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class StoreViewModel
    {
        public IEnumerable<ProductListItemViewModel> DozerProducts { get; set; }

        public IEnumerable<ProductListItemViewModel> ExcavatorProducts { get; set; }

        public IEnumerable<ProductListItemViewModel> CrushingProducts { get; set; }

        public IEnumerable<ProductListItemViewModel> FeaturedProducts { get; set; }

        public IEnumerable<ProductListItemViewModel> HotTipProducts { get; set; }
    }
}
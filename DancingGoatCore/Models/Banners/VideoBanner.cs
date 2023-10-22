using DancingGoat.Models.Banners;

namespace CMS.DocumentEngine.Types.KomatsuDemo
{
    public partial class VideoBanner : IVideoBannerDescription
    {
        public new string _VideoPath
        {
            get
            {
                return mFields._VideoPath;
            }
        }
    }
}
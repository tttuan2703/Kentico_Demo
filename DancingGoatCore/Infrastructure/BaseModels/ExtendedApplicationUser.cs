using CMS.Helpers;
using CMS.Membership;
using Kentico.Membership;

namespace DancingGoat.Infrastructure.BaseModels
{
    public class ExtendedApplicationUser : ApplicationUser
    {
        public string UserStatus { get; set; }

        public override void MapFromUserInfo(UserInfo userInfo)
        {
            base.MapFromUserInfo(userInfo);
            UserStatus = ValidationHelper.GetString(userInfo.GetValue("UserStatus"), "");
        }

        public override void MapToUserInfo(UserInfo userInfo)
        {
            base.MapToUserInfo(userInfo);
            var t = this;
            userInfo.SetValue("UserStatus", UserStatus);
        }
    }
}

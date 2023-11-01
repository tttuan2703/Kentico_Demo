using CMS.Helpers;
using CMS.Membership;
using CMS.Scheduler;

namespace DancingGoatCore.SchedulerTasks
{
    public class UpdateUserStatusTask : ITask
    {
        public string Execute(TaskInfo ti)
        {
            // Get the user ID from the task data
            int userId = ValidationHelper.GetInteger(ti.TaskData, 0);

            // Get the user
            UserInfo user = UserInfoProvider.GetUserInfo(userId);
            if (user != null)
            {
                // Update the status
                user.SetValue("UserStatus", "Old");

                // Save the changes
                UserInfoProvider.SetUserInfo(user);
            }

            return null;
        }
    }
}
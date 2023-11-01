using CMS.Activities.Loggers;
using CMS.Base;
using CMS.Base.UploadExtensions;
using CMS.ContactManagement;
using CMS.Core;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.Scheduler;
using CMS.SiteProvider;
using DancingGoat.Infrastructure.BaseModels;
using DancingGoat.Models;
using Kentico.Content.Web.Mvc;
using Kentico.Membership;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.CodeDom.Compiler;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DancingGoat.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEventLogService eventLogService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly IMembershipActivityLogger membershipActivitiesLogger;
        private readonly ApplicationUserManager<ApplicationUser> userManager;
        private readonly IMessageService emailService;
        private readonly IAvatarService avatarService;
        private readonly ISiteService siteService;

        public AccountController(IEventLogService eventLogService,
                                    SignInManager<ApplicationUser> signInManager,
                                    IStringLocalizer<SharedResources> localizer,
                                    IMembershipActivityLogger membershipActivitiesLogger,
                                    ApplicationUserManager<ApplicationUser> userManager,
                                    IMessageService emailService,
                                    IAvatarService avatarService,
                                    ISiteService siteService)
        {
            this.eventLogService = eventLogService;
            this.signInManager = signInManager;
            this.localizer = localizer;
            this.membershipActivitiesLogger = membershipActivitiesLogger;
            this.userManager = userManager;
            this.emailService = emailService;
            this.avatarService = avatarService;
            this.siteService = siteService;
        }

        // GET: Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var signInResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.StaySignedIn, false);
                if (!signInResult.Succeeded)
                {
                    var errMsg = "Your sign-in attempt was not successful. Please try again.";
                    if (signInResult.IsNotAllowed)
                    {
                        errMsg = "Your account requires activation before logging in.";
                    }

                    ModelState.AddModelError(string.Empty, localizer[errMsg].ToString());
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                eventLogService.LogException("AccountController", "Login", ex);
                ModelState.AddModelError(string.Empty, localizer[ex.Message].ToString());
                return View(model);
            }

            ContactManagementContext.UpdateUserLoginContact(model.UserName);
            membershipActivitiesLogger.LogLogin(model.UserName);

            var decodedReturnUrl = WebUtility.UrlDecode(returnUrl);
            if (!string.IsNullOrEmpty(decodedReturnUrl) && Url.IsLocalUrl(decodedReturnUrl))
            {
                return Redirect(decodedReturnUrl);
            }

            return Redirect(Url.Kentico().PageUrl(ContentItemIdentifiers.HOME));
        }

        // POST: Account/Logout
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            signInManager.SignOutAsync();
            return Redirect(Url.Kentico().PageUrl(ContentItemIdentifiers.HOME));
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var appUser = AppUserConstructor(model);
                var registerResult = await userManager.CreateAsync(appUser, model.Password);

                if (!registerResult.Succeeded)
                {
                    foreach (var error in registerResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(model);
                }

                membershipActivitiesLogger.LogRegistration(model.UserName);
                model.IsSuccessfulRegistration = true;

                UpdateUserStatus(model);
                return View(model);
            }
            catch (Exception ex)
            {
                eventLogService.LogException("AccountController", "Register", ex);
                ModelState.AddModelError(string.Empty, localizer["Your registration was not successful."]);
                return View(model);
            }
        }

        // GET: Account/RetrievePassword
        public ActionResult RetrievePassword()
        {
            return View();
        }

        // POST: Account/RetrievePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RetrievePassword(RetrievePasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, localizer["Not found account!"].ToString());
                    return View(model);
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var url = Url.Action(nameof(ResetPassword),
                                        "Account",
                                        new { userId = user.Id, token },
                                        RequestContext.URL.Scheme);

                await emailService.SendEmailAsync(user.Email, localizer["Request for changing your password"],
                string.Format(localizer["You have submitted a request to change your password. " +
                "Please click <a href=\"{0}\">this link</a> to set a new password.<br/><br/> " +
                "If you did not submit the request please let us know."], url));

                model.IsSuccessfulRetrievePassword = true;
            }
            catch (Exception ex)
            {
                eventLogService.LogException("AccountController", "RetrievePassword", ex);
                ModelState.AddModelError(string.Empty, localizer[ex.Message].ToString());
            }

            return View(model);
            // Get an instance of the IMessageService interface.
            //var emailMessage = new EmailMessage
            //{
            //    Recipients = user.Email,
            //    Subject = localizer["Request for changing your password"],
            //    Body = string.Format(localizer["You have submitted a request to change your password. " +
            //            "Please click <a href=\"{0}\">this link</a> to set a new password.<br/><br/> " +
            //            "If you did not submit the request please let us know."], url)
            //};

            //EmailSender.SendEmail("DancingGoatCore", emailMessage, true);
        }

        // GET: Account/ResetPassword
        [HttpGet]
        public async Task<ActionResult> ResetPassword(int userId, string token)
        {
            var model = new ResetPasswordViewModel();
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return NotFound();
                }

                var user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return NotFound();
                }

                model.UserId = user.Id;
                model.Token = token;
            }
            catch (Exception ex)
            {
                eventLogService.LogException("AccountController", "ResetPassword", ex);
                ModelState.AddModelError(string.Empty, localizer[ex.Message].ToString());
            }

            return View(model);
        }

        // POST: Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    return NotFound();
                }

                var resetResult = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (resetResult.Succeeded) // if successed
                {
                    return View("ResetPasswordSucceeded");
                }

                foreach (var error in resetResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception ex)
            {
                eventLogService.LogException("AccountController", "ResetPassword", ex);
                ModelState.AddModelError(string.Empty, localizer[ex.Message].ToString());
            }

            return View(model);
        }

        // GET: Account/YourAccount
        [Authorize]
        public async Task<ActionResult> YourAccount(bool avatarUpdateFailed = false)
        {
            var model = new YourAccountViewModel();
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }

                model.User = user;
                model.AvatarUpdateFailed = avatarUpdateFailed;
            }
            catch (Exception ex)
            {
                eventLogService.LogException("AccountController", "YourAccount", ex);
                ModelState.AddModelError(string.Empty, localizer[ex.Message].ToString());
            }

            return View(model);
        }

        // GET: Account/Edit
        [Authorize]
        public async Task<ActionResult> Edit()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var model = new PersonalDetailsViewModel(user);
            return View(model);
        }

        // POST: Account/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PersonalDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.UserName = User.Identity.Name;
                return View(model);
            }

            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                // Set full name only if it was automatically generated
                if (user.FullName == UserInfoProvider.GetFullName(user.FirstName, null, user.LastName))
                {
                    user.FullName = UserInfoProvider.GetFullName(model.FirstName, null, model.LastName);
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;

                await userManager.UpdateAsync(user);

                return RedirectToAction(nameof(YourAccount));
            }
            catch (Exception ex)
            {
                eventLogService.LogException("AccountController", "Edit", ex);
                ModelState.AddModelError(string.Empty, localizer["Personal details save failed"]);

                model.UserName = User.Identity.Name;
                return View(model);
            }
        }


        // POST: Account/ChangeAvatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> ChangeAvatar(IFormFile avatarUpload)
        {
            object routeValues = null;

            if (avatarUpload != null && avatarUpload.Length > 0)
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (!avatarService.UpdateAvatar(avatarUpload.ToUploadedFile(), user.Id, siteService.CurrentSite.SiteName))
                {
                    routeValues = new { avatarUpdateFailed = true };
                }
            }

            return RedirectToAction(nameof(YourAccount), routeValues);
        }

        #region Private Methods

        private ExtendedApplicationUser AppUserConstructor(RegisterViewModel model)
        {
            var applicationUser = new ExtendedApplicationUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.UserName,
                FullName = UserInfoProvider.GetFullName(model.FirstName, null, model.LastName),
                Enabled = true,
                UserStatus = "New"
            };

            //user.SetValue("UserStatus", "New");

            //// Map properties from ApplicationUser to UserInfo
            //var applicationUser = new ExtendedApplicationUser();
            //applicationUser.MapToUserInfo(user);
            return applicationUser;
        }

        private void UpdateUserStatus(RegisterViewModel user)
        {
            // Create a new TaskInterval object
            TaskInterval interval = new TaskInterval
            {
                StartTime = DateTime.Now.AddSeconds(15),
                Every = 1,
                BetweenStart = DateTime.Now,
                Day = "1",
                Period = "1",
                UseSpecificTime = false,
                Order = "1",
                Days = new System.Collections.Generic.List<DayOfWeek> { 
                    DayOfWeek.Sunday,DayOfWeek.Monday,DayOfWeek.Tuesday,DayOfWeek.Wednesday,DayOfWeek.Thursday,DayOfWeek.Friday,DayOfWeek.Saturday
                }, // This will set the task to run every day of the week
                BetweenEnd = DateTime.Now.AddDays(1) // This will set the end time to be 24 hours from now
            };
            // Create a new scheduled task
            TaskInfo newTask = new TaskInfo
            {
                TaskAssemblyName = "DancingGoatCore.SchedulerTasks",
                TaskClass = "DancingGoatCore.SchedulerTasks.UpdateUserStatusTask",
                TaskEnabled = true,
                TaskNextRunTime = DateTime.Now.AddSeconds(15),
                TaskDisplayName = "Update user status for " + user.UserName,
                TaskName = "UpdateUserStatusTask" + user.UserName.Split('@')[0] + "_" + Guid.NewGuid(),
                TaskType = ScheduledTaskTypeEnum.Standard,
                //TaskInterval = "Once", // Run once
                TaskDeleteAfterLastRun = true,
                TaskRunInSeparateThread = false,
                TaskData = user.UserName,
                TaskSiteID = SiteContext.CurrentSiteID,
                TaskAvailability = TaskAvailabilityEnum.LiveSite,
            };

            var taskStartTime = DateTime.Now.AddSeconds(10);
            newTask.TaskInterval = SchedulingHelper.EncodeInterval(new TaskInterval { Period = "once", UseSpecificTime = true, StartTime = taskStartTime });

            // Save the scheduled task
            TaskInfoProvider.SetTaskInfo(newTask);
        }

        #endregion Private Methods
    }
}
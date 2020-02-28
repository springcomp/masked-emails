using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4.Quickstart.UI
{
    [SecurityHeaders]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        /// <summary>
        /// Show Profile Page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View("Profile", await BuildViewModelAsync());
        }

        private async Task<ProfileViewModel> BuildViewModelAsync()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            return new ProfileViewModel {
                UserName = user.UserName,
                EmailAddress = user.Email,
            };
        }
    }
}
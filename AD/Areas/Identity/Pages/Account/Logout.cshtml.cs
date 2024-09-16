// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using AD.Models;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AD.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private AD.Data.Identity _context;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger, Data.Identity context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                LogInOutLog logInOutLog = new LogInOutLog
                {
                    Timestamp = DateTime.Now,
                    ActionType = "LogOut",
                    User = user
                };
                _context.logInOutLogs.Add(logInOutLog);
                await _context.SaveChangesAsync();
                return LocalRedirect(returnUrl);
            }
            else
            {
                
                return RedirectToPage();
            }
        }
    }
}

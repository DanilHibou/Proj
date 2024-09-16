// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Microsoft.AspNetCore.Identity;
using AD.Models;
using Kerberos.NET;
using Kerberos.NET.Client;
using Kerberos.NET.Credentials;
using AD.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Google.Apis.Admin.Directory.directory_v1.Data;

namespace AD.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ADContext _context;
        private AD.Data.Identity _context2;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, 
            ILogger<LoginModel> logger, ADContext context, Data.Identity context2)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
            _context2 = context2;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            public string UserName { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Запомнить вход")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var allowedUser = _context.AllowedUsers.FirstOrDefault(u => u.UserName == Input.UserName);
                if(allowedUser != null)
                {                   
                    var adLoginResult = ADHelper.ADLogin(Input.UserName, Input.Password);                    
                    if (!adLoginResult)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }

                    
                    var user = await _userManager.FindByNameAsync(Input.UserName);
                    if (user == null)
                    {
                        var identityResult = await _userManager.CreateAsync(new ApplicationUser
                        {
                            UserName = Input.UserName,
                        }, Input.Password);

                        if (identityResult != IdentityResult.Success)
                        {

                            foreach (IdentityError error in identityResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            LogInOutLog logInOutLog = new LogInOutLog
                            {
                                Timestamp = DateTime.Now,
                                ActionType = "Login",
                                User = user
                            };
                            _context2.logInOutLogs.Add(logInOutLog);
                            await _context2.SaveChangesAsync();
                            _logger.LogInformation("User logged in.");

                            return Page();
                        }
                    }

                    
                    var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        LogInOutLog logInOutLog = new LogInOutLog 
                        { 
                            Timestamp = DateTime.Now,
                            ActionType = "Login",
                            User = user
                        };
                        _context2.logInOutLogs.Add(logInOutLog);
                        await _context2.SaveChangesAsync();
                        return LocalRedirect(returnUrl);
                    }
                    
                }
                else
                {
                    LogInOutLog logInOutLog = new LogInOutLog
                    {
                        Timestamp = DateTime.Now,
                        ActionType = "Invalid Login Attempt",
                        UserName = Input.UserName
                    };
                    _context2.logInOutLogs.Add(logInOutLog);
                    await _context2.SaveChangesAsync();
                    return RedirectToPage("/AccessDenied");
                }
            }
                

            return Page();
        }
    }
}

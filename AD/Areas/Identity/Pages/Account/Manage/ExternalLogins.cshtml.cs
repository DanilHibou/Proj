// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AD.Areas.Identity.Pages.Account;
using AD.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static AD.Helper;

namespace AD.Areas.Identity.Pages.Account.Manage
{
    public class ExternalLoginsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private AD.Data.Identity _context;
        private TokenProtector _tokenProtector;

        public ExternalLoginsModel(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,
            IUserStore<ApplicationUser> userStore, AD.Data.Identity context, TokenProtector tokenProtector)
        { 
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
            _context = context;
            _tokenProtector = tokenProtector;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> OtherLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool ShowRemoveButton { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Ошибка при загрузке пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            CurrentLogins = await _userManager.GetLoginsAsync(user);
            OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();

            string passwordHash = null;
            if (_userStore is IUserPasswordStore<ApplicationUser> userPasswordStore)
            {
                passwordHash = await userPasswordStore.GetPasswordHashAsync(user, HttpContext.RequestAborted);
            }

            ShowRemoveButton = passwordHash != null || CurrentLogins.Count > 1;
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //Unable to load user with ID
                return NotFound($"Ошибка при загрузке пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (!result.Succeeded)
            {
                
                StatusMessage = "Внешний аккаунт для входа не был удален.";
                return RedirectToPage();
            }
            
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Внешний аккаунт для входа был удален.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

       
        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Ошибка при загрузке пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var info = await _signInManager.GetExternalLoginInfoAsync(userId);
            if (info == null)
            {
                throw new InvalidOperationException($"Ошибка при загрузке внешних провайдеров.");
            }

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                StatusMessage = "Вы не можете связать эту учетную запись в данный момент.";
                return RedirectToPage();
            }
            var refreshToken = info.AuthenticationTokens.FirstOrDefault(t => t.Name == "refresh_token")?.Value;
            string SecuredToken = _tokenProtector.Protect(refreshToken);
            string provider = info.LoginProvider; 
            if (refreshToken == null)
            {
                
            }
            else
            {
                if(provider == "Google")
                {
                    var accessToken = info.AuthenticationTokens.FirstOrDefault(t => t.Name == "access_token")?.Value;
                    var googleOauthTokens = new GoogleOauthTokens
                    {
                        User = user, 
                        refreshToken = SecuredToken, 
                        RefreshTokenIssued = DateTime.Now 
                        
                    };
                    _context.GoogleOauthTokens.Add(googleOauthTokens);
                    await _context.SaveChangesAsync();
                }
                else if(provider == "Yandex")
                {
                    var yandexOauthTokens = new YandexOauthTokens
                    {
                        User = user, 
                        refreshToken = SecuredToken, 
                        RefreshTokenIssued = DateTime.Now,                       
                    };
                    _context.YandexOauthTokens.Add(yandexOauthTokens);
                    await _context.SaveChangesAsync();
                }                
                           }                 
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = "The external login was added.";
            return RedirectToPage();
        }
    }
}

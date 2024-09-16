using AD.Areas.Identity.Pages.Account;
using AD.Data;
using AD.Models;
using Azure.Core;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using static AD.Helper;
using System.Diagnostics;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using Microsoft.AspNetCore.Authorization;


namespace AD.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public Helper helper;
        private AD.Data.Identity _context;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly Helper? _helper;
        private CookieProtector _cookieProtector;
        [TempData]
        public string StatusMessage { get; set; }
        public HomeController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, 
            AD.Data.Identity context, Helper helper, CookieProtector cookieProtector)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _helper = helper;
            _cookieProtector = cookieProtector;
        }
        //private readonly ILogger<HomeController> _logger;        

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
           
            var accessToken = HttpContext.Request.Cookies["access_token"];
            
            if (user != null)
            {
                
                Console.WriteLine("User Logged in");
                if(accessToken != null)
                {
                    Console.WriteLine("Access token exists: " + accessToken);
                   

                }
                else
                {
                    Console.WriteLine("Access token is missing");
                    await _helper.GetGoogleToken(user, HttpContext);
                }
            }
            else
            {
                Console.WriteLine("Empty");
            }


            

            return View();
        }
        public IActionResult Create()
        {
            //return View();
            return PartialView("_PartialView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserAccountNames userAccountNames)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userAccountNames);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
            //return PartialView("_PartialView", userAccountNames);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
        

    }
}
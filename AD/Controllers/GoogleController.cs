using AD.Areas.Identity.Pages.Account;
using AD.Data;
using AD.Models;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static AD.Helper;


namespace AD.Controllers
{
    [Authorize]
    public class GoogleController : Controller
    {
        private readonly ADContext _context;        
        private AD.Data.Identity _contextIdentity;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly Helper? _helper;
        private CookieProtector _cookieProtector;
        private readonly IHttpClientFactory _httpClientFactory;
        [TempData]
        public string StatusMessage { get; set; }
        public GoogleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
            AD.Data.Identity contextIdentity, Helper helper, CookieProtector cookieProtector, ADContext context, IHttpClientFactory httpClientFactory)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _contextIdentity = contextIdentity;
            _helper = helper;
            _cookieProtector = cookieProtector;
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            string? accessToken = HttpContext.Request.Cookies["access_token"];
           
            if (user != null)
            {                
                if (accessToken != null)
                {
                    var googleGroups = await _context.GoogleGroups.ToListAsync();
                    var googleOus = await _context.GoogleOU.ToListAsync();
                    var viewModel = new GoogleModel
                    {
                        googleGroupsList = googleGroups,
                        googleOUList = googleOus
                    };

                    return View(viewModel);
                }
                else
                {                    
                    await _helper.GetGoogleToken(user, HttpContext);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View();                
            }            
                      
        }
        public class GoogleModel()
        {            
            public GoogleGroups? googleGroups { get; set; }
            public List<GoogleGroups>? googleGroupsList { get; set; }            
            public List<string>? SelectedOUPaths { get; set; }
            public GoogleDomain? GoogleDomain { get; set; }
            public IEnumerable<GoogleDomain>? GoogleDomains { get; set; }
            public GoogleOU? googleOU { get; set; }
            public List<GoogleOU>? googleOUList { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetGoogleGroups()
        {
            var googleGr = await _context.GoogleGroups.ToListAsync();
            //var googleDom = new 
            var googledomains = await _context.GoogleDomain.ToListAsync();

            var viewModel = new GoogleModel
            {
                googleGroupsList = googleGr,
                GoogleDomains = googledomains 

            };
            return View(viewModel);
            
        }
        [HttpPost]
        public async Task<IActionResult> GetGoogleGroups(string domain)
        {

            var googledomains = await _context.GoogleDomain.ToListAsync();
            var accessToken = HttpContext.Request.Cookies["access_token"];
            if (accessToken != null)
            {

                string unencryptedToken = _cookieProtector.Unprotect(accessToken);
                HttpClient client = _httpClientFactory.CreateClient();                
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", unencryptedToken);
                    var request = new HttpRequestMessage(HttpMethod.Get, $"https://admin.googleapis.com/admin/directory/v1/groups?domain={domain}");

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        Groups? result = JsonConvert.DeserializeObject<Groups>(json);

                        if (result.GroupsValue == null)
                        {
                            var view1 = new GoogleModel
                            {
                                GoogleDomains = googledomains
                            };
                            return View(view1);
                        }

                        var googleGroups = new List<GoogleGroups>();

                        foreach (var group in result.GroupsValue)
                        {
                            googleGroups.Add(new GoogleGroups
                            {
                                Groupid = group.Id,
                                GroupName = group.Name
                            });
                        }

                        var view = new GoogleModel
                        {
                            googleGroupsList = googleGroups,
                            GoogleDomains = googledomains
                        };

                        return View(view);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                
                    
            }
            else
            {
                return RedirectToAction("Index"); 
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveGroupsGoogle(GoogleModel model)
        {
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var entry in model.googleGroupsList)
                {
                    if (entry.isChecked == true)
                    {
                        var groups = new GoogleGroups
                        {
                           
                            Groupid = entry.Groupid,
                            GroupName = entry.GroupName
                        };

                        _context.GoogleGroups.Add(entry);
                    }
                }
                await _context.SaveChangesAsync();
            }

            

            return RedirectToAction(nameof(Index)); 
        }
        public async Task<IActionResult> DeleteGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aDOU = await _context.GoogleGroups
                .FirstOrDefaultAsync(m => m.id == id);
            if (aDOU == null)
            {
                return NotFound();
            }

            return View(aDOU);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var aDOU = await _context.GoogleGroups.FindAsync(id);
            if (aDOU != null)
            {
                _context.GoogleGroups.Remove(aDOU);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> GetGoogleOU()
        {
            
            var googleOU = await _context.GoogleOU.ToListAsync();

            var viewModel = new GoogleModel
            {
                googleOUList = googleOU

            };
            return View(viewModel);

        }
        [HttpPost]
        public async Task<IActionResult> GetGoogleOU(string domain)
        {

            var googledomains = await _context.GoogleDomain.ToListAsync();
            var accessToken = HttpContext.Request.Cookies["access_token"];
            if (accessToken != null)
            {

                string unencryptedToken = _cookieProtector.Unprotect(accessToken);
                HttpClient client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", unencryptedToken);
                var request = new HttpRequestMessage(HttpMethod.Get, "https://admin.googleapis.com/admin/directory/v1/customer/my_customer/orgunits?orgUnitPath=/&type=all");

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<OrgUnits>(json);

                    if (result == null)
                    {
                        return View();
                    }

                    var googleOUs = new List<GoogleOU>();

                    foreach (var ou in result.OrganizationUnits)
                    {
                        googleOUs.Add(new GoogleOU
                        {
                            OUName = ou.Name,
                            OUPath = ou.OrgUnitPath,
                            OUid = ou.OrgUnitId
                        });
                    }

                    var view = new GoogleModel
                    {
                        googleOUList = googleOUs
                    };

                    return View(view);
                }
                else
                {
                    return RedirectToAction("Index"); 
                }
            }
            else
            {
                return RedirectToAction("Index"); 
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveOUGoogle(GoogleModel model)
        {
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var entry in model.googleOUList)
                {
                    if (entry.isChecked == true)
                    {
                        var ou = new GoogleOU
                        {
                            OUName = entry.OUName,
                            OUPath = entry.OUPath,
                            OUid = entry.OUid                            
                        };

                        _context.GoogleOU.Add(entry);
                    }
                }
                await _context.SaveChangesAsync();
            }

           

            return RedirectToAction(nameof(Index)); 
        }
        public async Task<IActionResult> DeleteOU(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aDOU = await _context.GoogleOU
                .FirstOrDefaultAsync(m => m.id == id);
            if (aDOU == null)
            {
                return NotFound();
            }

            return View(aDOU);
        }

        // POST: Account/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOU(int id)
        {
            var aDOU = await _context.GoogleOU.FindAsync(id);

            if (aDOU != null)
            {
                var dependent = _context.organizationalDivisions
                    .Where(r => r.GoogleOUid == id)
                    .ToList();
                if (dependent.Any())
                {
                    foreach (var org in dependent)
                    {
                        org.GoogleOUid = null;
                    }
                }
                _context.GoogleOU.Remove(aDOU);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool GoogleOUExist(int id)
        {
            return _context.GoogleOU.Any(e => e.id == id);
        }
        private bool GoogleGroupExist(int id)
        {
            return _context.GoogleGroups.Any(e => e.id == id);
        }


        public class GroupResponse
        {
            public string id { get; set; }
            public string email { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public bool adminCreated { get; set; }
            public string directMembersCount { get; set; }
            public string kind { get; set; }
            public string etag { get; set; }
            public List<string> aliases { get; set; }
            public List<string> nonEditableAliases { get; set; }
        }
    }
}

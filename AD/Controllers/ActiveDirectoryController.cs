using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices;
using AD.Data;
using AD.Models;
using AD.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Client;
using System.DirectoryServices.AccountManagement;
using System.Net.Sockets;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authentication.Cookies;
using static AD.Controllers.AccountController;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Admin.Directory.directory_v1.Data;


namespace AD.Controllers
{
    [Authorize]
    //[Authorize(Roles="Admin")]   

    //[Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
    public class ActiveDirectoryController : Controller
    {
        //private ADEntity ADEntity { get; set; }
        private static string ldap = "";
        //private InputModel password {  get; set; }
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private static string adminUsername = "";
        private static string adminPassword = "";
        private static string domainName = "";
        //private ADDomain domain;
        private readonly ADContext _context;
        public ActiveDirectoryController(ADContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var ADou = await _context.ADOU.ToListAsync();
            var ADgroups = await _context.ADGroups.ToListAsync();
            var UsAl = await _context.AllowedUsers.ToListAsync();
            //var ADDomain = await _context.DomainController.ToListAsync();
            var viewModel = new ActiveDirectoryModel
            {
                ADOUList = ADou,
                aDGroupsList = ADgroups,
                AllowedUsersList = UsAl
            };

            return View(viewModel);
        }

        public class ActiveDirectoryModel()
        {
            public ADOU ADOU { get; set; }
            public List<ADOU> ADOUList { get; set; }
            public ADGroups aDGroups { get; set; }
            public List<ADGroups> aDGroupsList { get; set; }
            public List<string> SelectedOUPaths { get; set; }
            public AD ad { get; set; }
            public List<AD> adList { get; set; }
            public AllowedUsers AllowedUsers { get; set; }
            public List<AllowedUsers> AllowedUsersList { get; set; }
        }
        public class AD
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? UserName { get; set; }
            public string? distinguishedName { get; set; }
            public bool isChecked { get; set; }
        }
        public async Task<IActionResult> FindUser()
        {
            var adUser = new List<AD>();
            var viewModel = new ActiveDirectoryModel
            {
                adList = adUser
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> FindUser(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                using (DirectoryEntry entry = new DirectoryEntry(ldap))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = $"(&(objectClass=user)(samAccountName=*{username}*))";
                        var results = await Task.Run(() => searcher.FindAll());

                        if (results == null || results.Count == 0)
                        {
                            TempData["Error"] = $"Пользователь {username} не найден";
                            return RedirectToAction(nameof(FindUser));
                        }

                        var adUsers = new List<AD>();
                        foreach (SearchResult result in results)
                        {
                            if (result != null)
                            {
                                using (DirectoryEntry user = result.GetDirectoryEntry())
                                {
                                    var adUser = new AD
                                    {
                                        FirstName = user.Properties["givenName"].Value?.ToString(),
                                        LastName = user.Properties["sn"].Value?.ToString(),
                                        UserName = user.Properties["samAccountName"].Value?.ToString(),
                                        distinguishedName = user.Properties["distinguishedName"].Value?.ToString()
                                    };
                                    adUsers.Add(adUser);
                                }
                            }

                        }

                        var viewModel = new ActiveDirectoryModel
                        {
                            adList = adUsers
                        };
                        return View(viewModel);
                    }
                }
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> SaveUser(ActiveDirectoryModel model)
        {
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var entry in model.adList)
                {
                    if (entry.isChecked == true)
                    {
                        var ouAD = new AllowedUsers
                        {
                            UserName = entry.UserName
                        };
                        _context.AllowedUsers.Add(ouAD);
                    }
                }
                await _context.SaveChangesAsync();
            }



            return RedirectToAction(nameof(SaveUser));
        }
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var AllowedUs = await _context.AllowedUsers
                .FirstOrDefaultAsync(m => m.id == id);
            if (AllowedUs == null)
            {
                return NotFound();
            }

            return View(AllowedUs);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var AllowedUs = await _context.AllowedUsers.FindAsync(id);
            if (AllowedUs != null)
            {
                _context.AllowedUsers.Remove(AllowedUs);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult GetGroupsAD()
        {
            var ADGroups = new List<ADGroups>();
            var viewModel = new ActiveDirectoryModel
            {
                aDGroupsList = ADGroups
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult GetGroupsAD(int id)
        {
            if (HttpContext.Request.Method == "POST")
            {
                using (DirectoryEntry root = new DirectoryEntry(ldap))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(root))
                    {
                        searcher.Filter = "(objectClass=group)";

                        SearchResultCollection results = searcher.FindAll();
                        List<ADGroups> Groups = new List<ADGroups>();
                        foreach (SearchResult result in results)
                        {
                            DirectoryEntry entry = result.GetDirectoryEntry();
                            Groups.Add(new ADGroups
                            {

                                GroupName = entry.Properties["cn"].Value?.ToString(),
                                GroupPath = entry.Properties["distinguishedName"].Value?.ToString()
                            });
                        }
                        var viewModel = new ActiveDirectoryModel
                        {
                            aDGroupsList = Groups
                        };
                        return View(viewModel);
                    }

                }

            }
            else
            {
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> SaveGroupsAD(ActiveDirectoryModel model)
        {
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var entry in model.aDGroupsList)
                {
                    if (entry.isChecked == true)
                    {

                        _context.ADGroups.Add(entry);
                    }
                }
                await _context.SaveChangesAsync();
            }



            return RedirectToAction(nameof(GetOUAD));
        }
        public async Task<IActionResult> EditGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aDGr = await _context.ADGroups.FindAsync(id);
            if (aDGr == null)
            {
                return NotFound();
            }
            return View(aDGr);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(int id, ADGroups aDGr)
        {
            if (id != aDGr.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aDGr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ADOUExist(aDGr.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aDGr);
        }
        public async Task<IActionResult> DeleteGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aDGR = await _context.ADGroups
                .FirstOrDefaultAsync(m => m.id == id);
            if (aDGR == null)
            {
                return NotFound();
            }

            return View(aDGR);
        }

        // POST: Account/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var adGr = await _context.ADGroups.FindAsync(id);
            if (adGr != null)
            {
                _context.ADGroups.Remove(adGr);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult GetOUAD()
        {
            var ADouList = new List<ADOU>();
            var viewModel = new ActiveDirectoryModel
            {
                ADOUList = ADouList
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult GetOUAD(int? id)
        {
            if (HttpContext.Request.Method == "POST")
            {
                using (DirectoryEntry root = new DirectoryEntry(ldap))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(root))
                    {
                        searcher.Filter = "(objectClass=organizationalUnit)";

                        SearchResultCollection results = searcher.FindAll();
                        List<ADOU> OUs = new List<ADOU>();
                        foreach (SearchResult result in results)
                        {
                            DirectoryEntry entry = result.GetDirectoryEntry();
                            OUs.Add(new ADOU
                            {
                                OUName = entry.Properties["Name"].Value?.ToString(),
                                OUPath = entry.Properties["distinguishedName"].Value?.ToString()
                            });
                        }

                        var viewModel = new ActiveDirectoryModel
                        {
                            ADOUList = OUs
                        };

                        return View(viewModel);
                    }
                }
            }
            else
            {

                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveOU(ActiveDirectoryModel model)
        {
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var entry in model.ADOUList)
                {
                    if (entry.isChecked == true)
                    {

                        _context.ADOU.Add(entry);
                    }
                }
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(GetOUAD));
        }
        public async Task<IActionResult> EditOU(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aDOU = await _context.ADOU.FindAsync(id);
            if (aDOU == null)
            {
                return NotFound();
            }
            return View(aDOU);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOU(int id, ADOU aDOU)
        {
            if (id != aDOU.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aDOU);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ADOUExist(aDOU.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aDOU);
        }
        public async Task<IActionResult> DeleteOU(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aDOU = await _context.ADOU
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
            var aDOU = await _context.ADOU.FindAsync(id);
            if (aDOU != null)
            {
                _context.ADOU.Remove(aDOU);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ADOUExist(int id)
        {
            return _context.ADOU.Any(e => e.id == id);
        }
        private bool ADGroupExist(int id)
        {
            return _context.ADGroups.Any(e => e.id == id);
        }
        private bool UserAllowed(int id)
        {
            return _context.AllowedUsers.Any(e => e.id == id);
        }
    }
}
        





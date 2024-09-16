using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AD;
using AD.Data;
using AD.Models;
using System.DirectoryServices.AccountManagement;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AD.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using static AD.Helper;
using System.DirectoryServices.ActiveDirectory;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace AD.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ADContext _context;
        private static string ldap = "";
        private static string domainName = "";
        private AD.Data.Identity _contextIdentity;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly Helper? _helper;
        private CookieProtector _cookieProtector;
        private readonly IHttpClientFactory _httpClientFactory;
        [TempData]
        public string StatusMessage { get; set; }
        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
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
        public class AccountModel()
        {
            public UserAccountNames? UserAccountName { get; set; }
            public List<AD.Models.UserAccountNames>? UserAccountNames { get; set; }            
            public GoogleOU? GoogleOU { get; set; }
            public IEnumerable<GoogleOU>? googleOUs { get; set; }
            public GoogleDomain? GoogleDomain { get; set; }
            public IEnumerable<GoogleDomain>? GoogleDomains { get; set; }
            public ADGroups? ADGroup { get; set; }
            public IEnumerable<ADGroups>? aDGroups { get; set; }
            public GoogleGroups? GoogleGroup { get; set; }
            public IEnumerable<GoogleGroups>? GoogleGroups { get; set; }
            public ADOU? ADOU { get; set; }
            public IEnumerable<ADOU>? ADOUs { get; set; }        
            public OrganizationalDivisions? organizationalDivision { get; set; }
            public IEnumerable<OrganizationalDivisions> organizationalDivisions { get; set; }
            public bool? Value {  get; set; } 

        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

             var accessToken = HttpContext.Request.Cookies["access_token"];            
            if (user != null)
            {
                var UserAccountNames = await _context.UserAccountNames
                    .Where(s => s.organizationalDivisionsid == null)
                    .ToListAsync();
                var divis = await _context.organizationalDivisions.ToListAsync();
                //var division = await _context.workerSubdivisions.ToListAsync();
                //var ADDomain = await _context.DomainController.ToListAsync();
                var viewModel = new AccountModel
                {
                    UserAccountNames = UserAccountNames,
                    organizationalDivisions = divis

                    //workerSubdivisions = division
                };
                if (accessToken != null)
                {
                    

                    return View(viewModel);
                }
                else
                {   
                    
                    await _helper.GetGoogleToken(user, HttpContext);
                    return View(viewModel);
                }
            }
            else
            {
               return BadRequest();
            }        
            
            
        }
        [HttpPost]
        public async Task<IActionResult> Index(int? id, string? searchString)
        {
            if (id != null)
            {
                
                var UserAccountNames = await _context.UserAccountNames
                .Where(s => s.organizationalDivisionsid == id)
                .ToListAsync();
                var divis = await _context.organizationalDivisions.ToListAsync();
                //var division = await _context.workerSubdivisions.ToListAsync();
                //var ADDomain = await _context.DomainController.ToListAsync();
                var viewModel = new AccountModel
                {
                    UserAccountNames = UserAccountNames,
                    organizationalDivisions = divis

                    //workerSubdivisions = division
                };

                return View(viewModel);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                var userAccountNames = await _context.UserAccountNames
                .Where(u => u.LastName.StartsWith(searchString))
                .ToListAsync();
                var divis = await _context.organizationalDivisions.ToListAsync();
                var viewModel = new AccountModel
                {
                    UserAccountNames = userAccountNames,
                    organizationalDivisions = divis
                };

                return View(viewModel);

            }

            return View();
        }
        [HttpGet]
        public IActionResult GetSubDivisions(int parentId)
        {
            var subDivisions = _context.organizationalDivisions
                                       .Where(d => d.ParentId == parentId)
                                       .Select(d => new { d.id, d.Name })
                                       .ToList();
            return Json(subDivisions);
        }

        [HttpGet]
        public async Task<IActionResult> GetChildren(int parentId)
        {
            var children = await _context.organizationalDivisions
                                         .Where(d => d.ParentId == parentId)
                                         .ToListAsync();
            return Json(children);
        }
        
        public async Task<IActionResult> GetChildDivisions(int parentId)
        {
            var childDivisions = await _context.organizationalDivisions
                .Where(d => d.ParentId == parentId)
                .ToListAsync();

            return Json(childDivisions);
        }

        // GET: Account/Create
        public async Task<IActionResult> Create()
        {
            //return View();
            UserAccountNames userAccountNames = new UserAccountNames();
            var disions = await _context.organizationalDivisions
                .Where(d => d.ADOUid != null || d.GoogleOUid != null)
                                .ToListAsync();
            var googleOUs = await _context.GoogleOU.ToListAsync();
            var googledomains = await _context.GoogleDomain.ToListAsync();
            AccountModel viewModel = new AccountModel
            {
                UserAccountName = userAccountNames,
                organizationalDivisions= disions,
               // workerSubdivisions = disions
                googleOUs = googleOUs,
                GoogleDomains = googledomains
            };           

         
            return PartialView("_PartialView", viewModel);
           
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountModel model)
        {
            //if (ModelState.IsValid)
            //{
                
            //}
            string fullname = model.UserAccountName.LastName + " " + model.UserAccountName.FirstName + " " + model.UserAccountName.SurName;
            string convertedFullName = NameConverter.ConvertFullNameToLatin(fullname);
            string username = NameConverter.FormatName(convertedFullName);

            var user = new UserAccountNames
            {
                FirstName = model.UserAccountName.FirstName,
                LastName = model.UserAccountName.LastName,
                SurName = model.UserAccountName.SurName,
                //workerType = model.UserAccountName.workerType,
                UserName = username,
                organizationalDivisionsid = model.organizationalDivision?.id
                //workerSubdivisionid = model.workerSubdivision.id
            };

            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
            //return PartialView("_PartialView", userAccountNames);
        }        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccountNames = await _context.UserAccountNames
                .FirstOrDefaultAsync(m => m.id == id);
            if (userAccountNames == null)
            {
                return NotFound();
            }

            return View(userAccountNames);
        }       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccountNames = await _context.UserAccountNames.FindAsync(id);
            if (userAccountNames == null)
            {
                return NotFound();
            }
            return View(userAccountNames);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,UserName,FirstName,LastName,SurName,workerType")] UserAccountNames userAccountNames)
        {
            if (id != userAccountNames.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAccountNames);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAccountNamesExists(userAccountNames.id))
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
            return View(userAccountNames);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccountNames = await _context.UserAccountNames
                .FirstOrDefaultAsync(m => m.id == id);
            if (userAccountNames == null)
            {
                return NotFound();
            }

            return View(userAccountNames);
        }
                
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userAccountNames = await _context.UserAccountNames.FindAsync(id);
            if (userAccountNames != null)
            {
                _context.UserAccountNames.Remove(userAccountNames);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AddUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccountNames = await _context.UserAccountNames.FindAsync(id);
                 
            //var adous = await _context.ADOU.ToListAsync();

            AccountModel viewModel = new AccountModel
            {
                UserAccountName = userAccountNames,
               //ADOUs = adous

            };
            if (userAccountNames == null)
            {
                return NotFound();
            }            
            //return PartialView(userAccountNames);
            return PartialView("AddUserView", viewModel);
            //return View(userAccountNames);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(int id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            //var userAccountNames = await _context.UserAccountNames.FindAsync(id);
            //var aDOU = userAccountNames.organizationalDivisionsid
             var userAccountNames = await _context.UserAccountNames
            .Include(u => u.organizationalDivisions)
            .ThenInclude(od => od.ADOU)
            .FirstOrDefaultAsync(u => u.id == id);
            if (userAccountNames == null)
            {
                return NotFound();
            }
            try
                {
                var check = new ADCheck();
                bool userExists = await check.UserExists(userAccountNames.UserName);
                if (!userExists)
                {
                    var ou = userAccountNames.organizationalDivisions?.ADOU;
                    if (ou == null)
                    {
                        TempData["ErrorMessage"] = $"Организационное подразделение найдено";
                        return RedirectToAction(nameof(Index));
                    }
                    string ldapPath = ou.OUPath;
                    string password = _helper.PasswordGen();
                    string pwd = password;                   
                    using (PrincipalContext entry = new PrincipalContext(ContextType.Domain))
                    {
                        using (UserPrincipal newUser = new UserPrincipal(entry))
                        {
                            newUser.DisplayName = userAccountNames.LastName + " " + userAccountNames.FirstName + " " + userAccountNames.SurName;
                            newUser.GivenName = userAccountNames.FirstName;
                            newUser.Surname = userAccountNames.LastName;
                            newUser.SamAccountName = userAccountNames.UserName;
                            newUser.UserPrincipalName = userAccountNames.UserName + domainName;
                            newUser.Save();
                            newUser.SetPassword(pwd);
                            newUser.Enabled = true;
                            newUser.UserCannotChangePassword = true;
                            newUser.PasswordNeverExpires = true;
                            newUser.Save();

                            newUser.Dispose();
                        }
                    }
                    //userAccountNames.ADOUid = ADOU.id;
                    userAccountNames.isADCreated = true;
                    userAccountNames.ADCreated = DateTime.Now;
                    await _context.SaveChangesAsync();
                    TempData["Password"] = $"{pwd}";                    
                    TempData["UserCreated"] = $"Пользователь {userAccountNames.UserName} создан";
                    
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = $"Пользователь с UserName {userAccountNames.UserName} уже существует";
                    return RedirectToAction(nameof(Index));
                }
                    
                }
                catch (Exception e)
                {
                
                TempData["ErrorMessage"] = "Ошибка при добавлении пользователя.";
                return RedirectToAction(nameof(Index));
                
            }    
        }
        public async Task<IActionResult> AddUserGroupAD(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccountNames = await _context.UserAccountNames.FindAsync(id);
            
            var adGroups = await _context.ADGroups.ToListAsync();
           

            AccountModel viewModel = new AccountModel
            {
                UserAccountName = userAccountNames,
                aDGroups = adGroups
            };
            if (userAccountNames == null)
            {
                return NotFound();
            }
            
            return PartialView("AddUserGroupAD", viewModel);
            
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserGroupAD(int id, ADGroups ADGroup)
        {

            if (id == null)
            {
                return NotFound();
            }
            var userAccountNames = await _context.UserAccountNames.FindAsync(id);
            if (userAccountNames == null)
            {
                return NotFound();
            }
            try
            {                
                string ldapPath = ldap;
                var groupAD = await _context.ADGroups.FindAsync(ADGroup.id);
                string GroupAD = groupAD.GroupName;

                using (PrincipalContext entry = new PrincipalContext(ContextType.Domain))
                {
                    using (GroupPrincipal group = GroupPrincipal.FindByIdentity(entry, GroupAD))
                    {
                        if (group != null)
                        {
                           
                            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(entry, userAccountNames.UserName);

                            if (userPrincipal != null)
                            {
                                
                                group.Members.Add(userPrincipal);
                                group.Save();
                            }
                            else
                            {
                                
                                TempData["ErrorMessage"] = "Пользователь не найден.";
                                return RedirectToAction(nameof(Index));
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Группа не найдена.";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                
                TempData["SuccessMessage"] = $"Пользователь {userAccountNames.UserName} добавлен в группу {GroupAD}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                
                TempData["ErrorMessage"] = "Ошибка при добавлении пользователя.";
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> AddUserGroupGoogle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccountNames = await _context.UserAccountNames.FindAsync(id);
            var adGroupsGoogle = await _context.GoogleGroups.ToListAsync();


            AccountModel viewModel = new AccountModel
            {
                GoogleGroups = adGroupsGoogle
            };
            if (userAccountNames == null)
            {
                return NotFound();
            }
            return PartialView("AddUserGroupGoogle", viewModel);
            //return View(userAccountNames);
        }
        [HttpPost]
        public async Task<IActionResult> AddUserGroupGoogle(int id, GoogleGroups GoogleGroup)
        {
            var accessToken = HttpContext.Request.Cookies["access_token"];
            if (id == null)
            {
                return NotFound();
            }
            var userAccountNames = await _context.UserAccountNames.FindAsync(id);
            if (userAccountNames == null)
            {
                return NotFound();
            }
            if (accessToken != null)
            {
                string unencryptedToken = _cookieProtector.Unprotect(accessToken);
                HttpClient client = _httpClientFactory.CreateClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", unencryptedToken);
                    
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"https://admin.googleapis.com/admin/directory/v1/groups/{GoogleGroup.Groupid}/members");
                    var memberInfo = new 
                    {
                        email = userAccountNames.UserName + "@" + "koriphey.ru",
                        role = "MEMBER"
                    };
                    string jsonContent = JsonConvert.SerializeObject(memberInfo);
                    request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = $"Пользователь {userAccountNames.UserName} добавлен в группу {GoogleGroup.GroupName}";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Ошибка при добавлении пользователя.";
                        return RedirectToAction(nameof(Index));
                    }
                

            }
            else
            {
                return NotFound(); 
            }
        }

        public async Task<IActionResult> GoogleAddUser(int? id)
        {
            

            if (id == null)
            {
                return RedirectToAction(nameof(Index));
                //return NotFound();
            }

            var userAccountNames = await _context.UserAccountNames.FindAsync(id);
            //GoogleOU? googleOU = new GoogleOU();
            //var googleOUs = await _context.GoogleOU.ToListAsync();
            var googledomains = await _context.GoogleDomain.ToListAsync();

            AccountModel viewModel = new AccountModel
            {
                UserAccountName = userAccountNames,                
                //googleOUs = googleOUs,
                GoogleDomains = googledomains

            };
            if (userAccountNames == null)
            {
                return RedirectToAction(nameof(Index));
                //return NotFound();
            }
            //return PartialView(userAccountNames);
            return PartialView("AddGoogle", viewModel);
            //return View(userAccountNames);
        }
        //HttpClient client = _httpClientFactory.CreateClient();
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> GoogleAddUser(int id, GoogleDomain GoogleDomain)
        {
            string pwd = string.Empty;
            var accessToken = HttpContext.Request.Cookies["access_token"];
            if (accessToken != null)
            {
                string UnEncryptedToken = string.Empty;
                UnEncryptedToken = _cookieProtector.Unprotect(accessToken);                
                if (id == null)
                {
                    return NotFound();
                }
                //var userAccountNames = await _context.UserAccountNames.FindAsync(id);
                var userAccountNames = await _context.UserAccountNames
                                                       .Include(u => u.organizationalDivisions)
                                                       .ThenInclude(od => od.GoogleOU)
                                                       .FirstOrDefaultAsync(u => u.id == id);
                if (userAccountNames == null)
                {
                    return NotFound();
                }
                GoogleCredential? cred = GoogleCredential.FromAccessToken(UnEncryptedToken);
                var service = new DirectoryService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = cred,
                    //ApplicationName = ApplicationName
                });

                
                
                var OU = userAccountNames.organizationalDivisions?.GoogleOU;
                var googleDomain = GoogleDomain.Name;
                string PrimaryEmail = userAccountNames.UserName + "@" + googleDomain;
                
                var resp = await _helper.GoogleCheck(PrimaryEmail, UnEncryptedToken);
               
                if(resp == true)
                {
                    TempData["ErrorMessage"] = $"Пользователь с почтой {PrimaryEmail} уже существует.";  
                }
                else
                {
                    string Hashedpwd = string.Empty;
                    string password = _helper.PasswordGen();
                    pwd = password;
                    //Hashedpwd = Helper.HashPassword(password);

                    User newUser = new User
                    {
                        OrgUnitPath = OU.OUPath,
                        PrimaryEmail = PrimaryEmail,                        
                        Name = new UserName { GivenName = userAccountNames.FirstName, FamilyName = userAccountNames.LastName },
                        Password = pwd, 
                        ChangePasswordAtNextLogin = true,
                        //HashFunction = "crypt"
                    };                    
                    User createdUser = service.Users.Insert(newUser).Execute();
                                      
                    userAccountNames.UserKey = createdUser.Id;  
                    userAccountNames.isGoogleCreated = true;
                    await _context.SaveChangesAsync();
                    TempData["Password"] = $"{pwd}";
                    
                    TempData["UserCreated"] = $"Пользователь с {PrimaryEmail} добавлен";
                   
                    
                }                    
               
            }
            else
            {
                RedirectToPage("Index");
            }
            return RedirectToAction(nameof(Index));                       
        }

        public async Task<IActionResult> UserInfo(int? id)
        {
            var userAccountNames = await _context.UserAccountNames.FindAsync(id);
            AccountModel info = new AccountModel
            {
                UserAccountName = userAccountNames
            };
            return PartialView("UserInfo", info);
        }
        
        
        [HttpGet]
        public async Task<IActionResult> ResetPasswordAD(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccountNames = await _context.UserAccountNames.FindAsync(id);

            if (userAccountNames == null)
            {
                return NotFound();

            }
            AccountModel model = new AccountModel
            {
                UserAccountName = userAccountNames,
                Value = true

            };
            return PartialView("ResetPasswordAD", model);
            
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordAD(int? id, bool value)
        {
           
            if (id == null)
            {
                return NotFound();
            }
            var userAccountNames = await _context.UserAccountNames.FindAsync(id);
            if (userAccountNames == null)
            {
                TempData["ErrorMessage"] = "Пользователь не найден.";
                return RedirectToAction(nameof(Index));
            }            
            try
            {
               
                    string password = _helper.PasswordGen();
                    string pwd = password;
                    using (PrincipalContext entry = new PrincipalContext(ContextType.Domain))
                    {
                        
                        using (UserPrincipal user = UserPrincipal.FindByIdentity(entry, userAccountNames.UserName))
                        {                            
                            user.SetPassword(pwd);
                            if(value)
                            {
                            user.PasswordNeverExpires = false;
                            user.UserCannotChangePassword = false;
                            
                            }
                        user.ExpirePasswordNow();

                        user.Save();
                                                                                     
                        }
                    }
                    TempData["Password"] = $"{pwd}";
                    
                    TempData["UserCreated"] = $"Пароль для {userAccountNames.UserName} сброшен";
                    return RedirectToAction(nameof(Index));
                
                
                  
                
            }              

            
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при изменении пароля";
                return RedirectToAction(nameof(Index));
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> ResetPasswordGoogle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var userAccountNames = await _context.UserAccountNames.FindAsync(id);
            if (userAccountNames == null)
            {
                return NotFound();
            }
            AccountModel model = new AccountModel
            {
                UserAccountName = userAccountNames,
                Value = true

            };
            
            return PartialView("ResetPasswordGoogle", model);

        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordGooglePost(AccountModel model, bool value)
        {
            var accessToken = HttpContext.Request.Cookies["access_token"];
            if (model.UserAccountName.id == null)
            {
                return NotFound();
            }
            var userAccountNames = await _context.UserAccountNames.FindAsync(model.UserAccountName.id);
            if (userAccountNames == null)
            {
                return NotFound();
            }
            if (accessToken != null)
            {
                string unencryptedToken = _cookieProtector.Unprotect(accessToken);
                HttpClient client = _httpClientFactory.CreateClient();
                string pwd = _helper.PasswordGen();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", unencryptedToken);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"https://admin.googleapis.com/admin/directory/v1/users/{userAccountNames.UserKey}");
                var memberInfo = new
                {
                    
                    password = pwd,
                    changePasswordAtNextLogin = value

                };
                string jsonContent = JsonConvert.SerializeObject(memberInfo);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Password"] = $"{pwd}";
                    
                    TempData["UserCreated"] = $"Пароль для {userAccountNames.UserName} сброшен";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Ошибка при добавлении пользователя.";
                    return RedirectToAction(nameof(Index));
                }


            }
            else
            {
                return NotFound(); 
            }

        }

        [HttpGet]
        public async Task<IActionResult> SuspendGoogle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccountNames = await _context.UserAccountNames.FindAsync(id);

            if (userAccountNames == null)
            {
                return NotFound();

            }
            AccountModel model = new AccountModel
            {
                UserAccountName = userAccountNames,
                Value = true

            };
            return PartialView("SuspendGoogle", model);
        }
        [HttpPost]
        public async Task<IActionResult> SuspendGooglePost(AccountModel model, bool value)
        {
            var accessToken = HttpContext.Request.Cookies["access_token"];
            if (model.UserAccountName.id == null)
            {
                return NotFound();
            }
            var userAccountNames = await _context.UserAccountNames.FindAsync(model.UserAccountName.id);
            if (userAccountNames == null)
            {
                return NotFound();
            }
            if (accessToken != null)
            {
                string unencryptedToken = _cookieProtector.Unprotect(accessToken);
                HttpClient client = _httpClientFactory.CreateClient();
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", unencryptedToken);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"https://admin.googleapis.com/admin/directory/v1/users/{userAccountNames.UserKey}");
                var memberInfo = new
                {

                    suspended = value     
                };
                string jsonContent = JsonConvert.SerializeObject(memberInfo);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    
                   
                    TempData["UserCreated"] = $"Пользователь {userAccountNames.UserName} заблокирован";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Ошибка при блокировке пользователя.";
                    return RedirectToAction(nameof(Index));
                }


            }
            else
            {
                return NotFound(); 
            }

        }
       
        [HttpGet]
        public async Task<IActionResult> SuspendAD(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccountNames = await _context.UserAccountNames.FindAsync(id);

            if (userAccountNames == null)
            {
                return NotFound();

            }
            AccountModel model = new AccountModel
            {
                UserAccountName = userAccountNames,
                Value = true

            };
            return PartialView("SuspendAD", model);
        }
        [HttpPost]
        public async Task<IActionResult> SuspendAD(int? id, bool value)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userAccountNames = await _context.UserAccountNames.FindAsync(id);
            if (userAccountNames == null)
            {
                TempData["ErrorMessage"] = "Пользователь не найден.";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                               
                using (PrincipalContext entry = new PrincipalContext(ContextType.Domain))
                {

                    using (UserPrincipal user = UserPrincipal.FindByIdentity(entry, userAccountNames.UserName))
                    {
                        user.Enabled = value;
                        user.Save();

                    }
                }

               
                bool action = false;
                string test = string.Empty;
                if(value)
                {
                    
                    test = "разблокирован";
                }
                else
                {
                    test = "заблокирован";
                }
                TempData["UserCreated"] = $"Пользователь {userAccountNames.UserName} {test}";
                return RedirectToAction(nameof(Index));


               

            }


            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка выполнении ";
                return RedirectToAction(nameof(Index));
            }
        }
        private bool UserAccountNamesExists(int id)
        {
            return _context.UserAccountNames.Any(e => e.id == id);
        }

    }
}

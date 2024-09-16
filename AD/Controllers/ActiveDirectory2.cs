using AD.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using System.Reflection.PortableExecutable;
using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace AD.Controllers
{
    [Authorize]
    public class ActiveDirectory2 : Controller
    {
        private string ldap = "123";
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        public ActiveDirectory2(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;            
        }
         
        public async Task<IActionResult> GetAllUsers() 
        {
            string ldapPath = ldap;

            using (DirectoryEntry entry = new DirectoryEntry(ldapPath))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(entry))
                {
                    searcher.Filter = "(objectClass=user)";

                    var users = new List<ActiveDirectoryUser>();

                    var result = await Task.Run(() => searcher.FindAll());
                    foreach (SearchResult searchResult in result)
                    {
                        using (DirectoryEntry user = searchResult.GetDirectoryEntry())
                        {
                            users.Add(new ActiveDirectoryUser
                            {
                                Name = user.Properties["givenName"].Value.ToString() + " " + user.Properties["sn"].Value.ToString(),
                                Email = user.Properties["mail"].Value.ToString(),
                                SAMAccountName = user.Properties["samAccountName"].Value.ToString()
                            });
                        }
                    }
                    return View(users);
                }
            }
        }
        public async Task<IActionResult> GetUser(string userName) 
        {

            using (DirectoryEntry entry = new DirectoryEntry(ldap))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(entry))
                {
                    searcher.Filter = "(&(objectClass=user)(samAccountName=" + userName + "))";
                    var result = await Task.Run(() => searcher.FindOne());
                    if (result == null)
                    {
                        return NotFound();
                    }
                    using (DirectoryEntry user = result.GetDirectoryEntry())
                    {
                        var adUser = new ActiveDirectoryUser
                        {
                            Name = user.Properties["givenName"].Value.ToString() + " " + user.Properties["sn"].Value.ToString(),
                            Email = user.Properties["mail"].Value.ToString(),
                            SAMAccountName = user.Properties["samAccountName"].Value.ToString()
                        };
                        return Ok(adUser);
                    }
                }
            }
        }
        public async Task AddMultipleUsers(List<ActiveDirectoryUser> users)
        {
          
            DirectoryEntry entry = new DirectoryEntry(ldapPath);

            foreach (var user in users)
            {
                DirectoryEntry newUser = entry.Children.Add("CN=" + user.Name, "user");
                newUser.Properties["samAccountName"].Value = user.SAMAccountName;
                newUser.Properties["givenName"].Value = user.FirstName;
                newUser.Properties["sn"].Value = user.LastName;
                newUser.Properties["mail"].Value = user.Email;                
                newUser.CommitChanges();
                newUser.Close();
            }
            entry.Close();
        }


       
        public class ActiveDirectoryUser
        {
            public string Name { get; set; }
            public string SAMAccountName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }
    }
}

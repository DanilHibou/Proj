using AD.Areas.Identity.Pages.Account;
using AD.Data;
using AD.Models;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using static AD.Controllers.DataController;

namespace AD.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        private readonly ADContext _context;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private ADOU ADOU { get; set; }
        public DataController(ADContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;

        }


        public async Task<IActionResult> Index()
        {
            var GoogleDomains = await _context.GoogleDomain.ToListAsync();
            var ADDomain = await _context.DomainController.ToListAsync();
            //var WorkerType = await _context.WorkerType.ToListAsync();

            var viewModel = new AccountIndexViewModel
            {
                GoogleDomain = GoogleDomains,
                ADDomain = ADDomain,
                //WorkerType = WorkerType
            };

            return View(viewModel);
            //return View();
        }
        public async Task<IActionResult> EditGoogle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var GoogleDomains = await _context.GoogleDomain.FindAsync(id);
            if (GoogleDomains == null)
            {
                return NotFound();
            }
            return View(GoogleDomains);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGoogle(int id, GoogleDomain googleDomain)
        {
            if (id != googleDomain.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(googleDomain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoogleExist(googleDomain.id))
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
            return View(googleDomain);
        }
        public IActionResult CreateGoogleDomains()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGoogleDomains(GoogleDomain GoogleDomain)
        {
            if (ModelState.IsValid)
            {
                _context.GoogleDomain.Add(GoogleDomain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(GoogleDomain);
        }
        public async Task<IActionResult> GoogleDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var GoogleDomain = await _context.GoogleDomain
                .FirstOrDefaultAsync(m => m.id == id);
            if (GoogleDomain == null)
            {
                return NotFound();
            }

            return View(GoogleDomain);
        }
               
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GoogleDelete(int id)
        {
            var GoogleDomain = await _context.GoogleDomain.FindAsync(id);
            if (GoogleDomain != null)
            {
                _context.GoogleDomain.Remove(GoogleDomain);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> EditAD(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var DomainControllers = await _context.DomainController.FindAsync(id);
            if (DomainControllers == null)
            {
                return NotFound();
            }
            return View(DomainControllers);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAD(int id, ADDomain domainController)
        {
            if (id != domainController.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(domainController);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ADExist(domainController.Id))
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
            return View(domainController);
        }        
        
        public async Task<IActionResult> DeleteAD(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domainController = await _context.DomainController
                .FirstOrDefaultAsync(m => m.Id == id);
            if (domainController == null)
            {
                return NotFound();
            }

            return View(domainController);
        }

        // POST: Account/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAD(int id)
        {
            var domainController = await _context.DomainController.FindAsync(id);
            if (domainController != null)
            {
                _context.DomainController.Remove(domainController);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }        
        public IActionResult TestDomainConnection()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestDomainConnection(ADDomain domain)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(domain.DomainName) || string.IsNullOrEmpty(domain.DomainControllerAddress))
                    {
                        TempData["Message1"] = "Необходимо указать данные";
                        return View();
                    }
                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain, domain.DomainControllerAddress))
                    {

                        domain.IsActive = true;
                        _context.DomainController.Add(domain);
                        await _context.SaveChangesAsync();
                        ViewBag.Message = "Соединение с доменом установлено";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message2"] = "Ошибка при установлении соединения с доменом: " + ex.Message;
                }
            }
            return View();
        }       
         
        public class AccountIndexViewModel
        {
            public List<AD.Models.GoogleDomain>? GoogleDomain { get; set; }
            public List<AD.Models.ADDomain>? ADDomain { get; set; }
        }                 
        private bool GoogleExist(int id)
        {
            return _context.GoogleDomain.Any(e => e.id == id);
        }
        private bool ADExist(int id)
        {
            return _context.DomainController.Any(e => e.Id == id);
        }



    }

}

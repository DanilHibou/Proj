using AD.Areas.Identity.Pages.Account;
using AD.Data;
using AD.Models;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static AD.Controllers.AccountController;

namespace AD.Controllers
{
    [Authorize]
    public class ManagerController : Controller
    {
        private readonly ADContext _context;
        private static string ldap = "";
        private static string domainName = "";
        private AD.Data.Identity _contextIdentity;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly Helper? _helper;
        //private CookieProtector _cookieProtector;        
        public ManagerController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
            AD.Data.Identity contextIdentity, Helper helper, /*CookieProtector cookieProtector,*/ ADContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _contextIdentity = contextIdentity;
            _helper = helper;
            //_cookieProtector = cookieProtector;
            _context = context;
        }
        public class ManagerModel()
        {
            public Location? Location { get; set; }
            public IEnumerable<Location> Locations { get; set; }
            //public WorkerSubdivision? workerSubdivision { get; set; }
            //public IEnumerable<WorkerSubdivision> workerSubdivisions { get; set; }
            public ADOU? aDOU { get; set; }
            public IEnumerable<ADOU> aDOUs { get; set; }
            public GoogleOU? googleOU { get; set; }
            public IEnumerable<GoogleOU> googleOUs { get; set; }
            
            public OrganizationalDivisions? organizationalDivision { get; set; }
            public IEnumerable<OrganizationalDivisions> organizationalDivisions { get; set; }
            //public WorkerPosition? workerPosition { get; set; }
            //public IEnumerable<WorkerPosition>? workerPositions { get; set; }           
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
           // var accessToken = HttpContext.Request.Cookies["access_token"];
            if (user != null)
            {
                var location = await _context.Location.ToListAsync();
                var divis = await _context.organizationalDivisions.ToListAsync();
                //var workerSubdivisions = await _context.workerSubdivisions
                //          .Include(ws => ws.ADOU)
                //          .Include(ws => ws.GoogleOU)
                //          .ToListAsync();
                //var ADDomain = await _context.DomainController.ToListAsync();
                var viewModel = new ManagerModel
                {
                    Locations = location,
                    organizationalDivisions = divis
                    //workerSubdivisions = workerSubdivisions

                };
                return View(viewModel);               
            }
            else
            {
                return BadRequest();
            }          

        }
        [HttpGet]
        public async Task<IActionResult> MainOrg()
        {

            //var workerdision = await _context.workerSubdivisions.ToListAsync();
            var divis = await _context.organizationalDivisions.ToListAsync();
            var googleou = await _context.GoogleOU.ToListAsync();
            var ad = await _context.ADOU.ToListAsync();

            ManagerModel viewModel = new ManagerModel
            {
                //workerSubdivisions = workerdision,
                organizationalDivisions = divis,
                googleOUs = googleou,
                aDOUs = ad

            };
            //if (userAccountNames == null)
            //{
            //    return NotFound();
            //}
            //return PartialView(userAccountNames);
            return PartialView("MainOrg", viewModel);
            //return View(userAccountNames);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MainOrg(OrganizationalDivisions organizationalDivision, ADOU? aDOU, GoogleOU? googleOU)
        {
            //if (ModelState.IsValid)
            //{

            //}
            OrganizationalDivisions division;

            if (organizationalDivision.id == 0)
            {
                division = new OrganizationalDivisions
                {
                    Name = organizationalDivision.Name,
                    Description = organizationalDivision.Description
                };
            }
            else
            {
                division = new OrganizationalDivisions
                {
                    Name = organizationalDivision.Name,
                    Description = organizationalDivision.Description,
                    ParentId = organizationalDivision.id
                };

                if (aDOU.id > 0 && googleOU.id > 0)
                {
                    division.ADOUid = aDOU.id;
                    division.GoogleOUid = googleOU.id;
                }
            }

            _context.Add(division);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));           
        }
        public async Task<IActionResult> EditOrg(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var org = await _context.organizationalDivisions.FindAsync(id);
            if (org == null)
            {
                return NotFound();
            }
            return PartialView("EditOrg",org);            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrg(int id)
        {
            
            return View();
        }
        public async Task<IActionResult> DeleteOrg(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.organizationalDivisions
                .FirstOrDefaultAsync(m => m.id == id);
            if (location == null)
            {
                return NotFound();
            }
            return PartialView("DeleteOrg", location);
            //return View(location);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrg(int id)
        {
            var location = await _context.organizationalDivisions.FindAsync(id);
            if (location != null)
            {
                var dependentOrg = _context.organizationalDivisions
                                           .Where(u => u.ParentId == id)
                                           .ToList();
                if(dependentOrg.Any())
                {
                    foreach (var org in dependentOrg)
                    {
                        org.ParentId = null;
                    }
                }                

                var dependentUsers = _context.UserAccountNames
                                             .Where(u => u.organizationalDivisionsid == id)
                                             .ToList();
                if(dependentUsers.Any())
                {
                    foreach (var user in dependentUsers)
                    {
                        user.organizationalDivisionsid = null;
                    }
                }
                location.ParentId = null;
                _context.organizationalDivisions.Remove(location);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CreateDivision()
        {   
            
            var adou = await _context.ADOU.ToListAsync();
            var googleou = await _context.GoogleOU.ToListAsync();

            ManagerModel viewModel = new ManagerModel
            {
                //workerSubdivisions = workerdision,
                googleOUs = googleou,
                aDOUs = adou

            };
            //if (userAccountNames == null)
            //{
            //    return NotFound();
            //}
            //return PartialView(userAccountNames);
            return PartialView("CreateDivision", viewModel);
            //return View(userAccountNames);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDivision(ADOU aDOU, GoogleOU googleOU)
        {
            if (ModelState.IsValid)
            {
                //WorkerSubdivision worker = new WorkerSubdivision
                //{
                //    Name = workerSubdivision.Name,
                //    Description = workerSubdivision.Description,
                //    //typeName = workerSubdivision.typeName,
                //    ADOUid = aDOU.id,
                //    GoogleOUid = googleOU.id
                //};
                //_context.Add(worker);
                //await _context.SaveChangesAsync();
                //_context.Location.Add(aDOU);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> EditDivision(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var location = await _context.workerSubdivisions.FindAsync(id);
            //if (location == null)
            //{
            //    return NotFound();
            //}
            //return View(location);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDivision(int id)
        {
            
            return View();
        }
        public async Task<IActionResult> DeleteDivision(int? id)
        {
            
            return View();
            //return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDivision(int id)
        {
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult CreateLocation()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLocation(Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Location.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }
        public async Task<IActionResult> EditLocation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLocation(int id, Location location)
        {
            if (id != location.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExist(location.id))
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
            return View(location);
        }
        public async Task<IActionResult> DeleteLocation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _context.Location.FindAsync(id);
            if (location != null)
            {
                _context.Location.Remove(location);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool LocationExist(int id)
        {
            return _context.Location.Any(e => e.id == id);
        }
        //private bool WorkerExist(int id)
        //{
        //    return _context.workerSubdivisions.Any(e => e.id == id);
        //}

    }
}

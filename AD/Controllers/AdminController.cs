//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;

//namespace AD.Controllers
//{
//    public class AdminController : Controller
//    {
//        //Identity _context;
//        RoleManager<IdentityRole> _roleManager;
//        public AdminController(RoleManager<IdentityRole> manager)
//        {
//            _roleManager = manager;
//        }

//        public async Task<IActionResult> GetRoles()
//        {
//            await _roleManager.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
//            return View(await _roleManager.Roles.ToListAsync());
//        }
//        public AdminController() { }
        
//    }
//}

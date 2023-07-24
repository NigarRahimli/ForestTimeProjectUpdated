using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ForestTime.WebUI.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            var findRole = await _roleManager.FindByNameAsync(name);
            if (findRole != null)
            {
                return View();
            }

            IdentityRole role = new()
            {
                Name = name
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {

                return RedirectToAction("Index");
            }
            return View();
        }



    }
}

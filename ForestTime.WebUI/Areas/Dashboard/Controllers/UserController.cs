using ForestTime.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Claims;

namespace ForestTime.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]

    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IHttpContextAccessor contextAccessor, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [Authorize(Roles = "Admin,Moderator")]

        public async Task<IActionResult> Profile()
        {
            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            return View(user);
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AddRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(string userId, string Name)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var findUser = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, findUser);
            var addRole = await _userManager.AddToRoleAsync(user, Name);
            if (addRole.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
using ForestTime.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ForestTime.WebUI.ViewComponents
{
    public class AuthViewComponent:ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public AuthViewComponent(UserManager<User> userManager, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _httpContext = httpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user=await _userManager.FindByIdAsync(userId);
            
            return View("Auth",user);
        }
    }
}

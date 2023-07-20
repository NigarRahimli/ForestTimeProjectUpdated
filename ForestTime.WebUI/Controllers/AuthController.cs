using ForestTime.WebUI.DTOs;
using ForestTime.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ForestTime.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        //login ucun ist edilir,register olub olmadigini yoxlaya bilirsiniz
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContext;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContext = httpContext;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var findUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (findUser == null)
            {
                return RedirectToAction("Login");
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(findUser, loginDTO.Password, false, false);
            if (result.Succeeded)
            {
                string cont = _httpContext.HttpContext.Request.Query["controller"].ToString();
                string act = _httpContext.HttpContext.Request.Query["action"].ToString();
                string id = _httpContext.HttpContext.Request.Query["id"].ToString();
                if (!String.IsNullOrWhiteSpace(cont))
                {
                    return RedirectToAction(act, cont, new { Id = id });
                }

                return RedirectToAction("Index", "Home");
            }
            return View(loginDTO);
        }
        //bu html seh acir
        public IActionResult Register()
        {
            return View();
        }
        //bu register htmlde post metodunu ise salir
        [HttpPost]
        //async bir birini gozleyen method yaratmaq ucun
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            User user = new()
            {
                FirstName = registerDTO.FirstName,

                LastName = registerDTO.LastName,
                PhotoUrl = "/image/avatar.png",
                //bu hisse identityden gelir doldurmaq mecburiyyetindeyik
                UserName = registerDTO.Email,
                Email = registerDTO.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(user, registerDTO.Password, false, false);//login  

                string cont = _httpContext.HttpContext.Request.Query["controller"].ToString();
                string act = _httpContext.HttpContext.Request.Query["action"].ToString();
                string id = _httpContext.HttpContext.Request.Query["id"].ToString();
                if (!String.IsNullOrWhiteSpace(cont))
                {
                    return RedirectToAction(act, cont, new { Id = id });
                }

                return RedirectToAction("Index", "Home");
            }
            return View(
                );
        }
    }
}

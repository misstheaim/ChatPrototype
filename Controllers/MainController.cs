using Microsoft.AspNetCore.Mvc;
using ChatPrototype.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ChatPrototype.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (user == null)
            {
                return NotFound("This user does not exists");
            }

            //Здесь будет подключение к БД

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) }; 
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");

            await ControllerContext.HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));
            
            return RedirectToAction("Index", "Main");
        }

        public async Task<IActionResult> Logout ()
        {
            await ControllerContext.HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login", "Main");
        }
    }
}

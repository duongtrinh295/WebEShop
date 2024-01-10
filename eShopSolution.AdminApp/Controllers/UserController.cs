using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.AdminApp.Controllers
{
    public class UserController : Controller
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
        public IActionResult Login(LoginRequest request)
        {
            if(!ModelState.IsValid)
                return View(ModelState);
            return View();
        } 
    }
}

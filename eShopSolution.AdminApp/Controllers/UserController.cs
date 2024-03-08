using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eShopSolution.AdminApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        public UserController(IUserApiClient userApiClient, IConfiguration configuration) 
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 1)
        {

            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageSize = pageSize,
                PageIndex = pageIndex

            };
            var data = await _userApiClient.GetUsersPagings(request);
            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            return View(result.ResultObj);    
        }

        [HttpGet]
        public  IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RegisterUser(request);
           
            if (result.IsSuccessed)
                return RedirectToAction("Index");

            if (result.Message !=null)    
                ModelState.AddModelError("", result.Message);
            
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                if (user != null)
                {
                    var updateRequest = new UserUpdateRequest()
                    {
                        Dob = user.Dob,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Id = id
                    };
                    return View(updateRequest);
                }
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.UpdateUser(request.Id, request);
            if (result.IsSuccessed)
                return RedirectToAction("Index");

            ModelState.AddModelError("", result.Message ?? "");
            return View(request);

        }

        [HttpGet]
        public ActionResult Delete(Guid id) 
        {
            return View( new UserDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.Delete(request.Id);

            if (result.IsSuccessed)
                return RedirectToAction("Index");
            
            ModelState.AddModelError("", result.Message);

            return View(request);
        }
      
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login","User");
        }
    }
}

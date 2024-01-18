using eShopSolution.Application.System.Users;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly  IUserService _userService;
        public UsersController(IUserService userService) 
        {
            _userService = userService;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var resultToken = await _userService.Authencate(request);
            if(string.IsNullOrEmpty(resultToken))
                return BadRequest("Username or password is inconrrect.");

            return Ok(resultToken);
        }

        [HttpPost("regiter")]
        [AllowAnonymous]
        public async Task<IActionResult> Regiter([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var resutl = await _userService.Register(request);
            if (!resutl)
                return BadRequest("Regiter is unsuccessful.");
            return Ok(resutl);
        }
    }
}

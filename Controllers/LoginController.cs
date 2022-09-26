using Microsoft.AspNetCore.Mvc;
using CoreApiTest.Interface;
using CoreApiTest.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CoreApiTest.Controllers
{
    [Route("api/auth/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtHelpers _jwtHelpers;
        public class LoginModel
        {
            public string Account { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        public LoginController(IUserService userService, JwtHelpers jwtHelpers)
        {
            _userService = userService;
            _jwtHelpers = jwtHelpers;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserOnLogin(loginModel.Account);
                if(user == null)
                {
                    return BadRequest(new
                    {
                        message = $"找不到使用者{loginModel.Account}"
                    });
                }
                if(user.Password == loginModel.Password)
                {
                    var token = _jwtHelpers.GenerateToken(user, "admin");
                    return Ok(new
                    {
                        id = user.Id,
                        name = user.Name,
                        email = user.Email,
                        token
                    });
                }
            }
            return BadRequest(new
            {
                message = "帳號或密碼錯誤"
            });
        }
        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> AuthUser()
        {
            if(User.Identity?.Name is not null)
            {
                int id = Int32.Parse(User.Identity.Name);
                var user = await _userService.GetUserById(id);
                return Ok(new
                {
                    data = user
                });
            }
            return NotFound();
        }

        [HttpGet("refresh_token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            if (User.Identity?.Name is not null)
            {
                int id = Int32.Parse(User.Identity.Name);
                var user = await _userService.GetUserById(id);
                if (user is not null)
                {
                    var token = _jwtHelpers.GenerateToken(user, "admin");
                    return Ok(new
                    {
                        refreshToken = token
                    });
                }
            }
            return NotFound();
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiTest.Controllers
{
    [Route("auth")]
    [Controller]
    public class OAuthController : Controller
    {
        [HttpGet("login")]
        public IActionResult SignInGoogle(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action("Callback", controller: "OAuth", values: new { returnUrl });
            return new ChallengeResult(provider, new AuthenticationProperties { RedirectUri = redirectUrl ?? "/" });
        }

        public IActionResult Callback(string? returnUrl = null, string? remoteError = null)
        {
            var claims = HttpContext.User;
            // 略...後續流程可直接參考官方範例，或自訂
            return Ok();
        }

        [HttpGet("test")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/api/order/15");
        }
    }
}
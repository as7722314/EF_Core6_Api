using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CoreApiTest.Controllers
{
    [Route("auth")]
    [Controller]
    public class OAuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OAuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("login")]
        public IActionResult SignInGoogle(string host = "https://accounts.google.com/o/oauth2/v2/auth")
        {
            //var redirectUrl = Url.Action("Callback", controller: "OAuth", values: new { returnUrl });
            //return new ChallengeResult(provider, new AuthenticationProperties { RedirectUri = redirectUrl ?? "/" });
            StringBuilder StrParam = new();

            StrParam.Append("scope=https://www.googleapis.com/auth/userinfo.email&");
            StrParam.Append("client_id=788479309658-mop99gob9ridpae7tquipog19oflddss.apps.googleusercontent.com&");
            StrParam.Append("redirect_uri=https://localhost:5069/auth/success&");
            StrParam.Append("response_type=code&");
            StrParam.Append("access_type=offline&");
            StrParam.Append("state=state_parameter_passthrough_value");

            return Redirect(host + "?" + StrParam.ToString());
        }

        [HttpGet("success")]
        public async Task<IActionResult> Callback()
        {
            var userClaims = HttpContext.User;
            var authCode = HttpContext.Request.Query["code"].ToString();

            if (authCode.Length == 0)
            {
                return Ok("沒有獲取授權碼!!");
            }

            //用Authorize code 去交換 Access token 及 Refresh token
            string StrAccToken = "";
            string StrRefToken = "";
            var result = await AuthCodeChgToken(authCode, StrAccToken, StrRefToken);

            return Redirect("/api/user");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/api/order/15");
        }

        private async Task<string> AuthCodeChgToken(string AuthCode, string AccToken, string RefToken)
        {
            //Token網址
            string host = "https://oauth2.googleapis.com/token";

            //Post參數
            StringBuilder StrParam = new StringBuilder();
            //Authorize code
            StrParam.Append("code=" + AuthCode + "&");
            //client_id(Google API Console 中可以找到 此專案的編號)
            StrParam.Append("client_id=788479309658-mop99gob9ridpae7tquipog19oflddss.apps.googleusercontent.com&");
            //client_secret(Google API Console 中可以找到 此專案的密碼)
            StrParam.Append("client_secret=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&");
            //redirect_uri(Google API Console 中設定CallBack的頁面)
            StrParam.Append("redirect_uri=https://localhost:5069/auth/success&");
            //grant_type
            StrParam.Append("grant_type=authorization_code");

            string uri = host + "?" + StrParam.ToString();

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();
            }

            //接收回傳內容 Json
            string StrReJson = "";
            return StrReJson;
        }
    }
}
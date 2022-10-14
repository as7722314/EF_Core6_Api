using Microsoft.AspNetCore.Mvc;

namespace CoreApiTest.Controllers
{
    [Route("api/tool")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        //半形轉全形
        [HttpGet("sbc/{sbcString}")]
        public IActionResult ToSBC(string sbcString)
        {
            char[] c = sbcString.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return Ok(new string(c));
        }

        //全形轉半形
        [HttpGet("dbc/{dbcString}")]
        public IActionResult ToDBC(string dbcString)
        {
            char[] c = dbcString.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return Ok(new string(c));
        }
    }
}
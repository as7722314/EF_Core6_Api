using CoreApiTest.Exceptions;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiTest.Controllers
{
    [Route("api/bulletin")]
    [ApiController]
    public class BulletinController : ControllerBase
    {
        [HttpGet]
        public string StartBulletinSync()
        {
            //var jobID = BackgroundJob.Schedule(() => Console.WriteLine("You checkout new product into your checklist!"), TimeSpan.FromSeconds(5));
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Sent similar product offer and suuggestions"), "*/5 * ? * * *");
            return "offer sent12!";
            //return $"You have done payment and receipt sent on your jobID = {jobID}!";
        }

        [HttpGet("test")]
        public IActionResult ErrorFilter()
        {
            //return Ok();
            throw new UnAuthException();
        }
    }
}
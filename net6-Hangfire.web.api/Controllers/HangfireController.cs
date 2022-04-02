using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace net6_Hangfire.web.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {

        //Fire&Forget Job example
        [HttpPost]
        [Route("[action]")]
        public IActionResult Welcome()
        {

            var jobId = BackgroundJob.Enqueue(() => SendWelcomeEmail("Welcome to our app."));

            return Ok($"Job Id: {jobId}. Welcome email sent too user!");
        }

        //Delayed Fire&Forget Job example
        [HttpPost]
        [Route("[action]")]
        public IActionResult Discount()
        {
            var jobId = BackgroundJob.Schedule(() =>
            SendWelcomeEmail("Welcome to our app."),
            TimeSpan.FromSeconds(30));

            return Ok($"Job Id: {jobId}. Discount will be sent in 30 seconds!");
        }

        //Recurring Job example
        [HttpPost]
        [Route("[action]")]
        public IActionResult DatabaseUpdate()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Database updated"), Cron.Minutely);

            return Ok($"Database check job initiated!");
        }

        //Continuous Job example
        [HttpPost]
        [Route("[action]")]
        public IActionResult Confirm()
        {
            int timeInSeconds = 30;
            var parentJobId = BackgroundJob.Schedule(() => SendWelcomeEmail("You asked to be unsubscribed!"),TimeSpan.FromSeconds(30));

            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You were unsubscribed!"));

            return Ok("Confirmation job created!");
        }

        public void SendWelcomeEmail(string text)
        {
            Console.WriteLine(text);
        }
    }
}

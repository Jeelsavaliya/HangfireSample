using Hangfire;
using HangfireSample.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangfireSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        private readonly IMailService _mailService;
        public HangfireController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpGet]
        [Route("FireAndForgetJob")]
        public string FireAndForgetJob()
        {
            //Fire - and - Forget Jobs
            //Fire - and - forget jobs are executed only once and almost immediately after creation.
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Welcome user in Fire and Forget Job Demo!"));

            return $"Job ID: {jobId}. Welcome user in Fire and Forget Job Demo!";
        }

        [HttpGet]
        [Route("DelayedJob")]
        public string DelayedJob()
        {
            //Delayed Jobs
            //Delayed jobs are executed only once too, but not immediately, after a certain time interval.
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Welcome user in Delayed Job Demo!"), TimeSpan.FromSeconds(60));

            return $"Job ID: {jobId}. Welcome user in Delayed Job Demo!";
        }

        [HttpGet]
        [Route("ContinuousJob")]
        public string ContinuousJob()
        {
            //Fire - and - Forget Jobs
            //Fire - and - forget jobs are executed only once and almost immediately after creation.
            var parentjobId = BackgroundJob.Enqueue(() => Console.WriteLine("Welcome user in Fire and Forget Job Demo!"));

            //Continuations
            //Continuations are executed when its parent job has been finished.
            BackgroundJob.ContinueJobWith(parentjobId, () => Console.WriteLine("Welcome Sachchi in Continuos Job Demo!"));

            return "Welcome user in Continuos Job Demo!";
        }

        [HttpGet]
        [Route("RecurringJob")]
        public string RecurringJobs()
        {
            //Recurring Jobs
            //Recurring jobs fire many times on the specified CRON schedule.
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Welcome user in Recurring Job Demo!"), Cron.Hourly);

            return "Welcome user in Recurring Job Demo!";
        }

        [HttpGet]
        [Route("BatchesJob")]
        public string BatchesJob()
        {
            //Batches - This option is available into hangfire Pro only
            //Batch is a group of background jobs that is created atomically and considered as a single entity.
            //Commenting the code as it's only available into Pro version


            //var batchId = BatchJob.StartNew(x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Batch Job 1"));
            //    x.Enqueue(() => Console.WriteLine("Batch Job 2"));
            //});

            return "Welcome user in Batches Job Demo!";
        }

        [HttpGet]
        [Route("BatchContinuationsJob")]
        public string BatchContinuationsJob()
        {
            //Batch Continuations - This option is available into hangfire Pro only
            //Batch continuation is fired when all background jobs in a parent batch finished.
            //Commenting the code as it's only available into Pro version

            //var batchId = BatchJob.StartNew(x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Batch Job 1"));
            //    x.Enqueue(() => Console.WriteLine("Batch Job 2"));
            //});

            //BatchJob.ContinueBatchWith(batchId, x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Last Job"));
            //});

            return "Welcome user in Batch Continuations Job Demo!";
        }


        [HttpPost]
        [Route("welcome")]
        public IActionResult welcome(string emailAddress)
        {
            var mailSubject = $"Welcome {emailAddress}";
            var mailBody = $"<p>Thanks for join us!</p>";
            var jobId = BackgroundJob.Schedule(() => _mailService.SendEmailAsync(emailAddress, mailSubject, mailBody), TimeSpan.FromSeconds(10));

            return Ok($"JobId : {jobId} Completed. Welcome Mail Sent");
        }
    }
}

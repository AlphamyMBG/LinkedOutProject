using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BackendApp.Model;
using BackendApp.Model.Enums;
using BackendApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace BackendApp.Controller
{
    [Route("/api/[Controller]")]
    [ApiController]
    public class LinkedOutJobController(ILinkedOutJobService jobService, IInterestService interestService) : ControllerBase
    {
        private readonly ILinkedOutJobService jobService = jobService;
        private readonly IInterestService interestService = interestService;

        [HttpPost]
        public IActionResult CreateJob(JobPost job)
            => this.jobService.AddJob(job) ? this.Ok(job.Id) : this.Conflict();
        
        [Route("{id}")]
        [HttpPost]
        public IActionResult UpdateJob(ulong id, JobPost job)
            => this.jobService.UpdateJob(id, job) switch
            {
                UpdateResult.KeyAlreadyExists => this.Conflict(),
                UpdateResult.NotFound => this.NotFound(),
                UpdateResult.Ok => this.Ok(),
                _  => throw new Exception("Something went terribly wrong for you to be here.") 
            };
        
        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete(ulong id)
            => this.jobService.RemoveJob(id) ? this.Ok() : this.NotFound();

        [HttpGet]
        public IActionResult GetAll()
            => this.Ok(this.jobService.GetAllJobs());

        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(ulong id)
        {
            var user = this.jobService.GetJobById(id);
            return user is not null ? this.Ok(user) : this.NotFound();
        }

        [Route("{jobId}/interest/set/{userId}")]
        [HttpPost]
        public IActionResult DeclareInterest(uint userId, uint jobId)
        {
            return this.interestService.DeclareInterestForJob(userId, jobId).ToResultObject(this);
        }

        [Route("{jobId}/interest/unset/{userId}")]
        [HttpPost]
        public IActionResult RemoveInterest(uint userId, uint jobId)
        {
            return this.interestService.RemoveInterestForJob(userId, jobId).ToResultObject(this);
        }

    }
}
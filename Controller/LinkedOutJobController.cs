using System;
using System.Collections.Generic;
using System.Linq;
using BackendApp.Model;
using BackendApp.Model.Enums;
using BackendApp.Model.Requests;
using BackendApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BackendApp.Auth.AuthConstants.PolicyNames;


namespace BackendApp.Controller
{
    [Route("/api/[Controller]")]
    [ApiController]
    public class JobController
    (
        IJobService jobService, 
        IInterestService interestService, 
        IRegularUserService regularUserService,
        IRecommendationService recommendationService
    ) 
    : ControllerBase
    {
        private readonly IJobService jobService = jobService;
        private readonly IInterestService interestService = interestService;
        private readonly IRegularUserService regularUserService = regularUserService;
        private readonly IRecommendationService recommendationService = recommendationService;

        [HttpPost]
        [Authorize( IsAdminPolicyName )]
        public IActionResult CreateJob(JobPost job)
            => this.jobService.AddJob(job) ? this.Ok(job.Id) : this.Conflict();
        
        [Route("{id}")]
        [HttpPost]
        [Authorize(IsAdminPolicyName )]
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
        [Authorize( CreatedJobPolicyName )]
        public IActionResult Delete(ulong id)
            => this.jobService.RemoveJob(id) ? this.Ok() : this.NotFound();

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
            => this.Ok(this.jobService.GetAllJobs());

        [Route("{id}")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get(ulong id)
        {
            var user = this.jobService.GetJobById(id);
            return user is not null ? this.Ok(user) : this.NotFound();
        }

        [Route("{jobId}/interest/set/{userId}")]
        [HttpPost]
        [Authorize( Policy = HasIdEqualToUserIdParamPolicyName )]
        public IActionResult DeclareInterest(uint userId, uint jobId)
        {
            return this.interestService.DeclareInterestForJob(userId, jobId).ToResultObject(this);
        }

        [Route("{jobId}/interest/unset/{userId}")]
        [HttpPost]
        [Authorize( Policy = HasIdEqualToUserIdParamPolicyName )]
        public IActionResult RemoveInterest(uint userId, uint jobId)
        {
            return this.interestService.RemoveInterestForJob(userId, jobId).ToResultObject(this);
        }

        [HttpPost("by/{userId}")]
        [Authorize( HasIdEqualToUserIdParamPolicyName )]
        public IActionResult CreateJob(ulong userId, JobCreationRequest request)
        {   
            var creatorOfJob = this.regularUserService.GetUserById(userId);
            if(creatorOfJob is null) return this.NotFound("User not found.");

            var resultingJob = this.jobService
                .CreateNewJobPost(creatorOfJob, request.Title, request.Description, request.Requirements);
                
            return resultingJob is not null ? this.Ok(resultingJob) : this.Conflict();
        }

        [HttpGet("by/{userId}")]
        [AllowAnonymous]
        public IActionResult GetJobsPostedBy(ulong userId)
        {   
            var user = this.regularUserService.GetUserById(userId);
            if(user is null) return this.NotFound("User not found.");
            return this.Ok(this.jobService.GetJobPostsBy(user));
        }

        [HttpGet("reccomend/{userId}/")]
        [AllowAnonymous]
        public IActionResult RecommendJobsTo(ulong userId)
        {
            var user = this.regularUserService.GetUserById(userId);
            if(user is null) return this.NotFound("User not found");
            return this.Ok(this.recommendationService.RecommendJobs(user, 20));
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using BackendApp.Data;
using BackendApp.Model;
using BackendApp.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.Service
{
    public interface IInterestService
    {
        UpdateResult DeclareInterestForPost(uint userId, uint postId);
        UpdateResult RemoveInterestForPost(uint userId, uint postId);
        UpdateResult DeclareInterestForJob(uint userId, uint jobId);
        UpdateResult RemoveInterestForJob(uint userId, uint jobId);

    }
    public class InterestService
    (ApiContext dbContext, ILinkedOutPostService postService, ILinkedOutJobService jobService, IRegularUserService userService) 
    : IInterestService
    {
        private readonly ApiContext dbContext = dbContext;
        private readonly ILinkedOutPostService postService = postService;
        private readonly ILinkedOutJobService jobService = jobService;
        private readonly IRegularUserService userService = userService;


        private UpdateResult DeclareInterestFor<T>
        (DbSet<T> dbSet, uint userId, T postBase) 
        where T : PostBase
        {
            RegularUser? userInDb = this.userService.GetUserById(userId);
            if(userInDb is null) return UpdateResult.NotFound;

            //Save new data
            postBase.InterestedUsers.Add(userInDb);
            this.dbContext.SaveChanges();
            return UpdateResult.Ok;
        }
        private UpdateResult RemoveInterestFor<T>
        (DbSet<T> dbSet, uint userId, PostBase postBase) 
        where T : PostBase
        {
            RegularUser? userInDb = this.userService.GetUserById(userId);
            if(userInDb is null) return UpdateResult.NotFound;

            //Save new data
            postBase.InterestedUsers = postBase.InterestedUsers.Where(user => user.Id != userId).ToList();
            this.dbContext.SaveChanges();
            return UpdateResult.Ok;
        }

        public UpdateResult DeclareInterestForJob(uint userId, uint jobId)
        {
            var jobInDb = this.jobService.GetJobById(jobId);
            if(jobInDb is null) return UpdateResult.NotFound;
            return this.DeclareInterestFor(this.dbContext.JobPosts, userId, jobInDb);
        }
        public UpdateResult RemoveInterestForJob(uint userId, uint jobId)
        {
            var jobInDb = this.jobService.GetJobById(jobId);
            if(jobInDb is null) return UpdateResult.NotFound;
            return RemoveInterestFor(this.dbContext.JobPosts, userId, jobInDb);
        }
        public UpdateResult DeclareInterestForPost(uint userId, uint jobId)
        {
            var postInDb = this.postService.GetPostById(jobId);
            if(postInDb is null) return UpdateResult.NotFound;
            return this.DeclareInterestFor(this.dbContext.Posts, userId, postInDb);
        }
        public UpdateResult RemoveInterestForPost(uint userId, uint jobId)
        {
            var postInDb = this.postService.GetPostById(jobId);
            if(postInDb is null) return UpdateResult.NotFound;
            return this.RemoveInterestFor(this.dbContext.Posts, userId, postInDb);
        }
    }
}
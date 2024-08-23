using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApp.Data;
using BackendApp.Model;
using BackendApp.Model.Enums;

namespace BackendApp.Service
{
    public interface ILinkedOutJobService
    {

        public JobPost? GetJobById(ulong id);
        public JobPost[] GetAllJobs();
        public bool AddJob(JobPost post);
        public bool RemoveJob(ulong id);
        public UpdateResult UpdateJob(ulong id, JobPost postContent);
        public JobPost? CreateNewJobPost(RegularUser user, string title, string description, string requirements);
        public UpdateResult AddInterestedUser(ulong id, RegularUser user);
        public JobPost[] GetJobPostsBy(RegularUser user);
    
    }
    public class LinkedOutJobService(ApiContext context) : ILinkedOutJobService
    {
        private readonly ApiContext context = context;
        
        public bool AddJob(JobPost job)
        {
            if(this.GetJobById(job.Id) != null) return false;
            this.context.JobPosts.Add(job);
            this.context.SaveChanges();
            return true;
        }

        public JobPost[] GetAllJobs()
            => [.. this.context.JobPosts];

        public JobPost? GetJobById(ulong id)
            => this.context.JobPosts.FirstOrDefault(post => post.Id == id);

        public bool RemoveJob(ulong id)
        {
            JobPost? post = this.GetJobById(id);
            if(post == null) return false;

            this.context.JobPosts.Remove(post);
            this.context.SaveChanges();
            return true;
        }

        public UpdateResult UpdateJob(ulong id, JobPost JobContent)
        {
            //Check if user exists
            JobPost? jobInDb = this.GetJobById(id);
            if(jobInDb is null) return UpdateResult.NotFound;

            //Save new data
            jobInDb.Update(JobContent);
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }

        public UpdateResult AddInterestedUser(ulong id, RegularUser user)
        {
            JobPost? jobInDb = this.GetJobById(id);
            if(jobInDb is null) return UpdateResult.NotFound;

            //Save new data
            jobInDb.InterestedUsers.Add(user);
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }

        public UpdateResult RemoveInterestedUser(ulong id, RegularUser user)
        {
            JobPost? jobInDb = this.GetJobById(id);
            if(jobInDb is null) return UpdateResult.NotFound;

            //Save new data
            var interestedUsers = jobInDb.InterestedUsers;
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }

        public JobPost? CreateNewJobPost(RegularUser user, string title, string description, string requirements)
        {
            var job = new JobPost(user, [], DateTime.Now, title, description, requirements);
            if(!this.AddJob(job)) return null;
            return job;
        }

        public JobPost[] GetJobPostsBy(RegularUser user)
            => this.context.JobPosts
                .Where( jobPost => jobPost.PostedBy == user)
                .ToArray();
    }           
}
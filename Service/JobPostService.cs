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
        // public LinkedOutPost[] GetPostsByContent(string content);
        // public LinkedOutPost[] GetPostsOfUser(ulong id);
        // public LinkedOutPost[] GetPostPostsOfUserWithId(ulong id);
        // public LinkedOutPost[] GetAdPostsOfUserWithId(ulong id);
        public JobPost[] GetAllJobs();
        public bool AddJob(JobPost post);
        public bool RemoveJob(ulong id);
        public UpdateResult UpdateJob(ulong id, JobPost postContent);
        public UpdateResult AddInterestedUser(ulong id, RegularUser user);
    
    }
    public class LinkedOutJobService(ApiContext context) : ILinkedOutJobService
    {
        private readonly ApiContext context = context;
        
        public bool AddJob(JobPost job)
        {
            if(this.GetJobById(job.Id) != null) return false;
            this.context.JobPost.Add(job);
            this.context.SaveChanges();
            return true;
        }

        public JobPost[] GetAllJobs()
            => [.. this.context.JobPost];

        public JobPost? GetJobById(ulong id)
            => this.context.JobPost.FirstOrDefault(post => post.Id == id);

        public bool RemoveJob(ulong id)
        {
            JobPost? post = this.GetJobById(id);
            if(post == null) return false;

            this.context.JobPost.Remove(post);
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
    }
}
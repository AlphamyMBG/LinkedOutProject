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

        public LinkedOutJob? GetJobById(ulong id);
        // public LinkedOutPost[] GetPostsByContent(string content);
        // public LinkedOutPost[] GetPostsOfUser(ulong id);
        // public LinkedOutPost[] GetPostPostsOfUserWithId(ulong id);
        // public LinkedOutPost[] GetAdPostsOfUserWithId(ulong id);
        public LinkedOutJob[] GetAllJobs();
        public bool AddJob(LinkedOutJob post);
        public bool RemoveJob(ulong id);
        public UpdateResult UpdateJob(ulong id, LinkedOutJob postContent);
        public UpdateResult AddInterestedUser(ulong id, LinkedOutUser user);
    
    }
    public class LinkedOutJobService(ApiContext context) : ILinkedOutJobService
    {
        private readonly ApiContext context = context;
        
        public bool AddJob(LinkedOutJob job)
        {
            if(this.GetJobById(job.Id) != null) return false;
            this.context.LinkedOutJobs.Add(job);
            this.context.SaveChanges();
            return true;
        }

        public LinkedOutJob[] GetAllJobs()
            => [.. this.context.LinkedOutJobs];

        public LinkedOutJob? GetJobById(ulong id)
            => this.context.LinkedOutJobs.FirstOrDefault(post => post.Id == id);

        public bool RemoveJob(ulong id)
        {
            LinkedOutJob? post = this.GetJobById(id);
            if(post == null) return false;

            this.context.LinkedOutJobs.Remove(post);
            this.context.SaveChanges();
            return true;
        }

        public UpdateResult UpdateJob(ulong id, LinkedOutJob JobContent)
        {
            //Check if user exists
            LinkedOutJob? jobInDb = this.GetJobById(id);
            if(jobInDb is null) return UpdateResult.NotFound;

            //Save new data
            jobInDb.Update(JobContent);
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }

        public UpdateResult AddInterestedUser(ulong id, LinkedOutUser user)
        {
            LinkedOutJob? jobInDb = this.GetJobById(id);
            if(jobInDb is null) return UpdateResult.NotFound;

            //Save new data
            jobInDb.InterestedUsers.Add(user);
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }

        public UpdateResult RemoveInterestedUser(ulong id, LinkedOutUser user)
        {
            LinkedOutJob? jobInDb = this.GetJobById(id);
            if(jobInDb is null) return UpdateResult.NotFound;

            //Save new data
            var interestedUsers = jobInDb.InterestedUsers;
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }
    }
}
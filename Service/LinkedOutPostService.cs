using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApp.Data;
using BackendApp.Model;
using BackendApp.Model.Enums;

namespace BackendApp.Service
{
    public interface ILinkedOutPostService
    {

        public LinkedOutPost? GetPostById(ulong id);
        // public LinkedOutPost[] GetPostsByContent(string content);
        // public LinkedOutPost[] GetPostsOfUser(ulong id);
        // public LinkedOutPost[] GetPostPostsOfUserWithId(ulong id);
        // public LinkedOutPost[] GetAdPostsOfUserWithId(ulong id);
        public LinkedOutPost[] GetAllPosts();
        public bool AddPost(LinkedOutPost post);
        public bool RemovePost(ulong id);
        public UpdateResult UpdatePost(ulong id, LinkedOutPost postContent);
    
    }
    public class LinkedOutPostService(ApiContext context) : ILinkedOutPostService
    {
        private readonly ApiContext context = context;
        public bool AddPost(LinkedOutPost post)
        {
            if(this.GetPostById(post.Id) != null) return false;
            this.context.LinkedOutPosts.Add(post);
            this.context.SaveChanges();
            return true;
        }

        public LinkedOutPost[] GetAllPosts()
            => this.context.LinkedOutPosts.ToArray();

        public LinkedOutPost? GetPostById(ulong id)
            => this.context.LinkedOutPosts.FirstOrDefault(post => post.Id == id);

        public bool RemovePost(ulong id)
        {
            LinkedOutPost? post = this.GetPostById(id);
            if(post == null) return false;

            this.context.LinkedOutPosts.Remove(post);
            this.context.SaveChanges();
            return true;
        }

        public UpdateResult UpdatePost(ulong id, LinkedOutPost postContent)
        {
            //Check if user exists
            LinkedOutPost? postInDb = this.GetPostById(id);
            if(postInDb is null) return UpdateResult.NotFound;

            //Save new data
            postInDb.Update(postContent);
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }
    }
}
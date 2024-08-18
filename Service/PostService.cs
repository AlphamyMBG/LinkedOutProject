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

        public Post? GetPostById(ulong id);
        // public LinkedOutPost[] GetPostsByContent(string content);
        // public LinkedOutPost[] GetPostsOfUser(ulong id);
        // public LinkedOutPost[] GetPostPostsOfUserWithId(ulong id);
        // public LinkedOutPost[] GetAdPostsOfUserWithId(ulong id);
        public Post[] GetAllPosts();
        public bool AddPost(Post post);
        public bool AddPost(string post, ulong creatorId);
        public bool RemovePost(ulong id);
        public UpdateResult UpdatePost(ulong id, Post postContent);
    
    }
    public class LinkedOutPostService(ApiContext context) : ILinkedOutPostService
    {
        private readonly ApiContext context = context;
        public bool AddPost(Post post)
        {
            if(this.GetPostById(post.Id) != null) return false;
            this.context.Posts.Add(post);
            this.context.SaveChanges();
            return true;
        }

        public bool AddPost(string content, ulong creatorId)
        {
            var user = context.RegularUsers.FirstOrDefault(x => x.Id == creatorId) ?? null;
            if(user is null) return false;
            var post = new Post(
                user,
                [],
                DateTime.Now,
                content,
                []
            );
            return this.AddPost(post);
        }

        public Post[] GetAllPosts()
            => this.context.Posts.ToArray();

        public Post? GetPostById(ulong id)
            => this.context.Posts.FirstOrDefault(post => post.Id == id);

        public bool RemovePost(ulong id)
        {
            Post? post = this.GetPostById(id);
            if(post == null) return false;

            this.context.Posts.Remove(post);
            this.context.SaveChanges();
            return true;
        }

        public UpdateResult UpdatePost(ulong id, Post postContent)
        {
            //Check if user exists
            Post? postInDb = this.GetPostById(id);
            if(postInDb is null) return UpdateResult.NotFound;

            //Save new data
            postInDb.Update(postContent);
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }
    }
}
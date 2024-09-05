using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApp.Data;
using BackendApp.Model;
using BackendApp.Model.Enums;

namespace BackendApp.Service
{
    public interface IPostService
    {
        public Post? GetPostById(ulong id);
        public Post[] GetAllPosts();
        public bool AddPost(Post post);
        public bool RemovePost(ulong id);
        public Post? CreateNewPost(string post, RegularUser creator);
        public UpdateResult UpdatePost(ulong id, Post postContent);
        public Post? ReplyToPost(ulong originalPostId, string content, RegularUser replyGuy);
        public Post[] GetPostsFrom(RegularUser user, bool includeReplies = false);
    }

    public sealed class LinkedOutPostService(ApiContext context) : IPostService
    {
        private readonly ApiContext context = context;
        public bool AddPost(Post post)
        {
            this.context.Posts.Add(post);
            this.context.SaveChanges();
            return true;
        }

        public Post? CreateNewPost(string content, RegularUser creator)
        {
            var post = new Post(
                creator,
                [],
                DateTime.Now,
                content,
                [],
                false
            );
            if(!this.AddPost(post)) return null;
            return post;
        }

        public Post? ReplyToPost(ulong originalPostId, string content, RegularUser replyGuy)
        {
            var ogPost = this.GetPostById(originalPostId);
            if(ogPost is null) return null;
            var reply = new Post(
                replyGuy,
                [],
                DateTime.Now,
                content,
                [],
                true
            );
            ogPost.Replies.Add(reply);
            this.context.SaveChanges();
            return reply;
        }

        public Post[] GetAllPosts()
            => this.context.Posts.ToArray();

        public Post? GetPostById(ulong id)
            => this.context.Posts.FirstOrDefault(post => post.Id == id);

        public bool RemovePost(ulong id)
        {
            Post? post = this.GetPostById(id);
            if(post == null) return false;
            if(post.IsReply)
            {
                var originalPost = this.context.Posts.FirstOrDefault( a => a.Replies.Contains(post));
                originalPost?.Replies.Remove(post);
            }
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

        public Post[] GetPostsFrom(RegularUser user, bool includeReplies = false)
        {
            if(includeReplies)
                return this.context.Posts.Where( x => x.PostedBy == user).ToArray();
            return this.context.Posts.Where( x => x.PostedBy == user && x.IsReply ).ToArray();
        }
    }
}
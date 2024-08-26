using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{

    public class Post(
        RegularUser postedBy,
        List<RegularUser> interestedUsers,
        DateTime postedAt,
        string content,
        List<Post> replies
    ) : PostBase(postedBy, interestedUsers, postedAt)
    {
        private Post(DateTime postedAt, string content)
        : this
        (new("","","","","","","",[],""),[], postedAt, content, [])
        {}

        public string Content { get; set; } = content;
        public List<Post> Replies { get; set; } = replies;
        public void Update( Post post ){
            this.Id = post.Id;
            this.Content = post.Content;
            this.Replies = [.. post.Replies];
            this.PostedBy = post.PostedBy;
            this.InterestedUsers = [.. post.InterestedUsers];
        }
    }
}
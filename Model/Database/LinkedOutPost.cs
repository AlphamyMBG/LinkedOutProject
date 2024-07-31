using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{

    public class LinkedOutPost(
        LinkedOutUser postedBy,
        List<LinkedOutUser> interestedUsers,
        DateTime postedAt,
        string content,
        List<LinkedOutPost> replies
    ) : LinkedOutPostBase(postedBy, interestedUsers, postedAt)
    {
        public string Content { get; set; } = content;
        public List<LinkedOutPost> Replies { get; set; } = replies;
        public void Update( LinkedOutPost post ){
            this.Id = post.Id;
            this.Content = post.Content;
            this.Replies = [.. post.Replies];
            this.PostedBy = post.PostedBy;
            this.InterestedUsers = [.. post.InterestedUsers];
        }
    }
}
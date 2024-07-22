using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{

    public class LinkedOutPost(
        string content,
        List<LinkedOutPost> replies,
        LinkedOutUser postedBy,
        List<LinkedOutUser> interestedUsers
    ) : LinkedOutPostBase(postedBy, interestedUsers)
    {
        public string Content { get; set; } = content;
        public List<LinkedOutPost> Replies { get; set; } = replies;
        public void Update( LinkedOutPost post ){
            this.Id = post.Id;
            this.Content = post.Content;
            this.Replies = post.Replies.ToList();
            this.PostedBy = post.PostedBy;
            this.InterestedUsers = post.InterestedUsers.ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{

    public enum PostVariant{
        Post,
        Ad
    }
    public class LinkedOutPost(
        string content,
        List<LinkedOutPost> replies,
        LinkedOutUser postedBy,
        List<LinkedOutUser> interestedUsers,
        PostVariant postVariant
    )
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public ulong Id {get; set;}
        public string Content { get; set; } = content;
        public List<LinkedOutPost> Replies { get; set; } = replies;
        public LinkedOutUser PostedBy { get; set; } = postedBy;
        public List<LinkedOutUser> InterestedUsers { get; set; } = interestedUsers;
        public PostVariant PostVariant{ get; set; } = postVariant;

        public void Update( LinkedOutPost post ){
            this.Id = post.Id;
            this.Content = post.Content;
            this.Replies = post.Replies.ToList();
            this.PostedBy = post.PostedBy;
            this.InterestedUsers = post.InterestedUsers.ToList();
            this.PostVariant = post.PostVariant;
        }
    }
}
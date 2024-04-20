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
    )
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public ulong Id {get; set;}
        public string Content { get; set; } = content;
        public List<LinkedOutPost> Replies { get; set; } = replies;
        public LinkedOutUser PostedBy { get; set; } = postedBy;
        public List<LinkedOutUser> InterestedUsers { get; set; } = interestedUsers;


    }
}
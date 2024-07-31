using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{
    public class LinkedOutPostBase(
        LinkedOutUser postedBy, 
        List<LinkedOutUser> interestedUsers,
        DateTime postedAt
    )
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public ulong Id {get; set;}
        public LinkedOutUser PostedBy {get; set;} = postedBy;
        public List<LinkedOutUser> InterestedUsers { get; set; } = interestedUsers;
        public DateTime PostedAt {get; set;} = postedAt;
        public void Update(LinkedOutPostBase postBase)
        {
            this.PostedBy = postBase.PostedBy;
            this.InterestedUsers = postBase.InterestedUsers;
        }
    }
}
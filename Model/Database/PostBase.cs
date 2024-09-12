using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{
    public class PostBase(
        RegularUser postedBy, 
        List<RegularUser> interestedUsers,
        DateTime postedAt
    )
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public long Id {get; set;}
        public RegularUser PostedBy {get; set;} = postedBy;
        public List<RegularUser> InterestedUsers { get; set; } = interestedUsers;
        public DateTime PostedAt {get; set;} = postedAt;
        public void Update(PostBase postBase)
        {
            this.PostedBy = postBase.PostedBy;
            this.InterestedUsers = postBase.InterestedUsers;
            this.PostedAt = postBase.PostedAt;
        }
    }
}
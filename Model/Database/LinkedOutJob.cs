using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{
    public class LinkedOutJob
    (   string jobTitle,
        string description,
        string requirements,
        LinkedOutUser postedBy,
        List<LinkedOutUser> interestedUsers 
    ) : LinkedOutPostBase(postedBy, interestedUsers)
    {
        public string JobTitle {get; set;} = jobTitle;
        public string Description {get; set;} = description;
        public string Requirements {get; set;} = requirements;

        public void Update(LinkedOutJob job) {
            base.Update(job);
            this.PostedBy = job.PostedBy;
            this.InterestedUsers = job.InterestedUsers;
            this.JobTitle = job.JobTitle;
            this.Description = job.Description;
            this.Requirements = job.Requirements;
        }
    }
}
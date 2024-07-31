using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{
    public class Connection
    (
        RegularUser sentBy, 
        RegularUser sentTo, 
        bool accepted,
        DateTime timestamp
    )
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public ulong Id {get; set;}
        public RegularUser SentBy {get; set;} = sentBy;
        public RegularUser SentTo {get; set;} = sentTo;
        public DateTime Timestamp {get; set;} = timestamp;
        public bool Accepted {get; set;} = accepted;
    
        public void Update(Connection connection)
        {
            this.SentBy = connection.SentBy;
            this.SentTo = connection.SentTo;
            this.Timestamp = connection.Timestamp;
            this.Accepted = connection.Accepted;
        }

    }
}
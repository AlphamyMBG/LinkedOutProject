using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{
    public class Message
    (
        string content,
        RegularUser sentBy, 
        RegularUser sentTo, 
        DateTime timestamp
    )
    {
        protected Message(string content, DateTime timestamp)
        : this(content, null!, null!, timestamp)
        {}

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public long Id {get; set;}
        public string Content {get; set;} = content;
        public virtual RegularUser SentBy {get; set;} = sentBy;
        public virtual RegularUser SentTo {get; set;} = sentTo;
        public DateTime Timestamp {get; set;} = timestamp;
    
        public void Update(Message message)
        {
            this.Content = message.Content;
            this.SentBy = message.SentBy;
            this.SentTo = message.SentTo;
            this.Timestamp = message.Timestamp;
        }

    }
}
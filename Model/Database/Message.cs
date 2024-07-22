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
        LinkedOutUser sentBy, 
        LinkedOutUser sentTo, 
        DateTime timestamp
    )
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public ulong Id {get; set;}
        public string Content {get; set;} = content;
        public LinkedOutUser SentBy {get; set;} = sentBy;
        public LinkedOutUser SentTo {get; set;} = sentTo;
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
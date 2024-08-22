using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{
    public class Notification
    (string content, bool read, RegularUser toUser, DateTime timestamp)
    {
        private Notification(string content, bool read, DateTime timestamp)
        : this(content, read, new("","","","","","","",[],""), timestamp)
        {}
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public ulong Id {get; set;}
        public string Content {get; set;} = content;
        public bool Read {get; set;} = read;
        public RegularUser ToUser {get; set;} = toUser; 
        public DateTime Timestamp {get; set;} = timestamp;

        public void Update(Notification notification){
            this.Content = notification.Content;
            this.Id = notification.Id;
            this.Read = notification.Read;
            this.ToUser = notification.ToUser;
        }
    }
}
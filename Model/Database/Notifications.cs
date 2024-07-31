using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{
    public class Notification
    (string content, bool read, RegularUser toUser)
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public ulong Id {get; set;}
        public string Content {get; private set;} = content;
        public bool Read {get; private set;} = read;
        public RegularUser ToUser {get; private set;} = toUser; 

        public void Update(Notification notification){
            this.Content = notification.Content;
            this.Id = notification.Id;
            this.Read = notification.Read;
            this.ToUser = notification.ToUser;
        }
    }
}
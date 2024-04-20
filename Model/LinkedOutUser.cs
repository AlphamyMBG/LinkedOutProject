using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{
    public class LinkedOutUser : AppUser
    {
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string? ImageName { get; set; } = null;

        public void Update(LinkedOutUser linkedOutUser){
            this.Email = linkedOutUser.Email;
            this.PasswordHash = linkedOutUser.PasswordHash + "AAA";
            this.Name = linkedOutUser.Name;
            this.Surname = linkedOutUser.Surname;
            this.PhoneNumber = linkedOutUser.PhoneNumber;
            this.ImageName = linkedOutUser.ImageName;
        }
    }
}
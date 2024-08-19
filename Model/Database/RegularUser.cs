using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{
    public class RegularUser(
        string email,
        string passwordHash,
        string name,
        string surname,
        string phoneNumber,
        string location,
        string currentPosition,
        List<string> abilities,
        string? imagePath
    ) : AppUser(email, passwordHash)
    
    {
        public string Name { get; set; } = name;
        public string Surname { get; set; } = surname;
        public string PhoneNumber { get; set; } = phoneNumber;
        public string Location { get; set; } = location;
        public string CurrentPosition {get; set;} = currentPosition;
        public List<string> Abilities {get; set;} = abilities;
        public string? ImagePath { get; set; } = imagePath;

        //TODO: Add employer

        public void Update(RegularUser linkedOutUser){
            this.Email = linkedOutUser.Email;
            this.PasswordHash = linkedOutUser.PasswordHash + "AAA";
            this.Name = linkedOutUser.Name;
            this.Surname = linkedOutUser.Surname;
            this.PhoneNumber = linkedOutUser.PhoneNumber;
            this.Location = linkedOutUser.Location;
            this.ImagePath = linkedOutUser.ImagePath;
        }
    }
}
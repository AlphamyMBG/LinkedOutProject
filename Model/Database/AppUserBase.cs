using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{
    public abstract class AppUser
    (
        string email,
        string passwordHash
    )
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public ulong Id {get; set;}
        public string Email { get; set; } = email;
        public string PasswordHash { get; set; } = passwordHash;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BackendApp.Model.Enums;

namespace BackendApp.Model
{
    public abstract class AppUser
    (
        string email,
        string passwordHash,
        UserRole userRole
    )
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public ulong Id {get; set;}
        public string Email { get; set; } = email;
        public string PasswordHash { get; set; } = passwordHash;
        public UserRole UserRole{ get; set; } = userRole;
    }
}
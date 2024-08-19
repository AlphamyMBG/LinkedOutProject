using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Model
{
    public class AdminUser
    (string email, string passwordHash) 
    : AppUser
    (email: email, passwordHash: passwordHash)
    {}
}
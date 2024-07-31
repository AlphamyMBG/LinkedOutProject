using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackendApp.Model;
using BackendApp.Data;
using BackendApp.Model.Requests;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using BackendApp.Model.Enums;

namespace BackendApp.Service
{
    public interface IRegularUserService{
        public RegularUser? GetUserByEmail(string email);
        public RegularUser? GetUserById(ulong id);
        public RegularUser? GetUserByName(string name);
        public RegularUser? GetUserBySurname(string surname);
        public RegularUser[] GetAllUsers();
        public bool AddUser(RegularUser user);
        public bool RemoveUser(ulong id);
        public UpdateResult Update(ulong id, RegularUser user);
    }
    public class RegularUserService(ApiContext context) : IRegularUserService
    {
        private readonly ApiContext context = context;

        public bool AddUser(RegularUser user)
        {
            if(this.GetUserByEmail(user.Email) != null) return false;
            this.context.RegularUsers.Add(user);
            this.context.SaveChanges();
            return true;
        }

        public RegularUser[] GetAllUsers()
            => this.context.RegularUsers.ToArray();

        public RegularUser? GetUserByEmail(string email) 
            => this.context.RegularUsers.FirstOrDefault( userInDb => userInDb.Email == email );

        public RegularUser? GetUserById(ulong id)
            => this.context.RegularUsers.FirstOrDefault( userInDb => userInDb.Id == id );

        public RegularUser? GetUserByName(string name)
            => this.context.RegularUsers.FirstOrDefault( userInDb => userInDb.Name == name );

        public RegularUser? GetUserBySurname(string surname)
            => this.context.RegularUsers.FirstOrDefault( userInDb => userInDb.Surname == surname );

        public UpdateResult Update(ulong id, RegularUser user)
        {
            //Check if user exists
            RegularUser? userInDb = this.GetUserById(id);
            if(userInDb is null) return UpdateResult.NotFound;
            
            //Check if user with same email exists
            RegularUser? userWithEmail = this.GetUserByEmail(user.Email);
            if(userWithEmail is not null && userWithEmail.Id != id) return UpdateResult.KeyAlreadyExists;

            //Save new data
            userInDb.Update(user);
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }

        public bool RemoveUser(ulong id){
            RegularUser? user = this.GetUserById(id);
            if( user is null ) return false;

            this.context.RegularUsers.Remove(user);
            this.context.SaveChanges();
            return true;
        }
    }
}





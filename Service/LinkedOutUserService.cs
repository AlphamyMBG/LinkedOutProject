using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackendApp.Model;
using BackendApp.Data;
using BackendApp.Model.Requests;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.Service
{

    public enum UpdateResult{
        KeyAlreadyExists,
        NotFound,
        Ok
    }

    public interface ILinkedOutUserService{
        public LinkedOutUser? GetUserByEmail(string email);
        public LinkedOutUser? GetUserById(ulong id);
        public LinkedOutUser? GetUserByName(string name);
        public LinkedOutUser? GetUserBySurname(string surname);
        public LinkedOutUser[] GetAllUsers();
        public bool AddUser(LinkedOutUser user);
        public bool RemoveUser(ulong id);
        public UpdateResult Update(ulong id, LinkedOutUser user);
    }
    public class LinkedOutUserService(ApiContext context) : ILinkedOutUserService
    {
        private readonly ApiContext context = context;

        public bool AddUser(LinkedOutUser user)
        {
            if(this.GetUserByEmail(user.Email) != null) return false;
            this.context.LinkedOutUsers.Add(user);
            this.context.SaveChanges();
            return true;
        }

        public LinkedOutUser[] GetAllUsers()
            => this.context.LinkedOutUsers.ToArray();

        public LinkedOutUser? GetUserByEmail(string email) 
            => this.context.LinkedOutUsers.FirstOrDefault( userInDb => userInDb.Email == email );

        public LinkedOutUser? GetUserById(ulong id)
            => this.context.LinkedOutUsers.FirstOrDefault( userInDb => userInDb.Id == id );

        public LinkedOutUser? GetUserByName(string name)
            => this.context.LinkedOutUsers.FirstOrDefault( userInDb => userInDb.Name == name );

        public LinkedOutUser? GetUserBySurname(string surname)
            => this.context.LinkedOutUsers.FirstOrDefault( userInDb => userInDb.Surname == surname );

        public UpdateResult Update(ulong id, LinkedOutUser user)
        {
            //Check if user exists
            LinkedOutUser? userInDb = this.GetUserById(id);
            if(userInDb is null) return UpdateResult.NotFound;
            
            //Check if user with same email exists
            LinkedOutUser? userWithEmail = this.GetUserByEmail(user.Email);
            if(userWithEmail is not null && userWithEmail.Id != id) return UpdateResult.KeyAlreadyExists;

            //Save new data
            userInDb.Update(user);
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }

        public bool RemoveUser(ulong id){
            LinkedOutUser? user = this.GetUserById(id);
            if( user is null ) return false;

            this.context.LinkedOutUsers.Remove(user);
            this.context.SaveChanges();
            return true;
        }
    }

}





using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackendApp.Model;
using BackendApp.Data;
using BackendApp.Model.Requests;
using Newtonsoft.Json;
using BackendApp.Service;
using BackendApp.Model.Enums;

namespace BackendApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LinkedOutUserController(
        ILinkedOutUserService userService
    ) : ControllerBase
    {
        private readonly ILinkedOutUserService linkedOutUserService = userService;

        [Route("register")]
        [HttpPost]
        public IActionResult Register( RegisterRequest request ){
            LinkedOutUser newUser = new(
                email: request.Email,
                passwordHash: request.Password + "AAA",
                name: request.Name,
                surname: request.Surname,
                phoneNumber: request.PhoneNumber,
                imagePath: request.ImageName,
                location: request.Location,
                currentPosition: "",
                abilities: []
            );
            bool added = this.linkedOutUserService.AddUser(newUser);
            return added ? new JsonResult(this.Ok(newUser.Id)) : new JsonResult(this.Conflict());
        }

        [HttpPost]
        public IActionResult Create(LinkedOutUser user){
            bool added = this.linkedOutUserService.AddUser(user);
            return added ? new JsonResult(this.Ok(user.Id)) : new JsonResult(this.Conflict());
        }

        [Route("{id}")]
        [HttpPost]
        public IActionResult Update(ulong id, LinkedOutUser user)
            => this.linkedOutUserService.Update(id, user) switch
            {
                UpdateResult.KeyAlreadyExists => new JsonResult(this.Conflict()),    
                UpdateResult.NotFound => new JsonResult(this.NotFound()),    
                UpdateResult.Ok => new JsonResult(this.Ok()),
                _ => throw new Exception("Something went terribly wrong for you to be here.")
            };

        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete(ulong id)
            => this.linkedOutUserService.RemoveUser(id) ? new JsonResult(this.Ok()) : new JsonResult(this.NotFound());
        

        [HttpGet]
        public IActionResult GetAll(){
            return new JsonResult(this.linkedOutUserService.GetAllUsers());
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(ulong id){
            var user = this.linkedOutUserService.GetUserById(id);
            return new JsonResult(user is not null ? this.Ok(user) : this.NotFound());
        }
    }
}
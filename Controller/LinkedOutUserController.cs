using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackendApp.Model;
using BackendApp.Data;
using BackendApp.Model.Requests;
using Newtonsoft.Json;
using BackendApp.Service;
using BackendApp.Model.Enums;
using BackendApp.auth;
using Microsoft.AspNetCore.Authorization;
using static BackendApp.Auth.AuthConstants.PolicyNames;


namespace BackendApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController(
        IRegularUserService userService
    ) : ControllerBase
    {
        private readonly IRegularUserService linkedOutUserService = userService;

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register( RegisterRequest request ){
            RegularUser newUser = new(
                email: request.Email,
                passwordHash: EncryptionUtility.HashPassword(request.Password),
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
        [Authorize( IsAdminPolicyName )]
        public IActionResult Create(RegularUser user){
            bool added = this.linkedOutUserService.AddUser(user);
            return added ? new JsonResult(this.Ok(user.Id)) : new JsonResult(this.Conflict());
        }

        [Route("{id}")]
        [HttpPost]
        [Authorize( Policy = HasIdEqualToIdParamPolicyName )]
        public IActionResult Update(ulong id, RegularUser user)
        {
            user.PasswordHash = EncryptionUtility.HashPassword(user.PasswordHash);
            return this.linkedOutUserService.Update(id, user) switch
            {
                UpdateResult.KeyAlreadyExists => new JsonResult(this.Conflict()),    
                UpdateResult.NotFound => new JsonResult(this.NotFound()),    
                UpdateResult.Ok => new JsonResult(this.Ok()),
                _ => throw new Exception("Something went terribly wrong for you to be here.")
            };
        }
        [Route("{id}")]
        [HttpDelete]
        [Authorize( Policy = HasIdEqualToIdParamPolicyName )]
        public IActionResult Delete(ulong id)
            => this.linkedOutUserService.RemoveUser(id) 
            ? new JsonResult(this.Ok("User successfully deleted.")) 
            : new JsonResult(this.NotFound("User not found."));
        

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll(){
            var users = this.linkedOutUserService
                .GetAllUsers()
                .Select( a => RegularUser.MapNewWithHiddenPassword(a));
            return new JsonResult(users);
        }

        [Route("{id}")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get(ulong id){
            var user = this.linkedOutUserService.GetUserById(id);
            return new JsonResult(
                user is not null 
                ? this.Ok(RegularUser.MapNewWithHiddenPassword(user)) 
                : this.NotFound()
            );
        }
        
    }
}
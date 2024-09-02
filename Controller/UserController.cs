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

        [Route("search/{searchString}")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SearchByUsername(string searchString)
        {
            return this.Ok(this.linkedOutUserService.SearchByUsername(searchString));
        }

        [Route("email/{email}")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetUserByEmail(string email)
        {
            var user = this.linkedOutUserService.GetUserByEmail(email);
            if(user is null) return this.NotFound("User not found.");
            return this.Ok(user);
        }
        
    }
}
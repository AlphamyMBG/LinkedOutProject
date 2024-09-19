using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BackendApp.Model;
using BackendApp.Model.Enums;
using BackendApp.Model.Requests;
using BackendApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BackendApp.Auth.AuthConstants.PolicyNames;


namespace BackendApp.Controller
{

    [Route("api/[Controller]")]
    [ApiController]
    public class PostController
    (IPostService postService, IInterestService interestService, IRegularUserService userService, INotificationService notificationService) 
    : ControllerBase
    {
        private readonly IPostService postService = postService;
        private readonly IInterestService interestService = interestService;
        private readonly IRegularUserService userService = userService;
        private readonly INotificationService notificationService = notificationService;

        [HttpPost]
        [Authorize( IsAdminPolicyName )]
        public IActionResult CreatePost(Post post)
            => this.postService.AddPost(post) ? this.Ok(post.Id) : this.Conflict();
        
        [Route("create/{userId}")]
        [HttpPost]
        [Authorize( Policy = HasIdEqualToUserIdParamPolicyName)]
        public IActionResult CreatePost(long userId, PostCreationRequest request)
        {
            var user = this.userService.GetUserById(userId);
            if(user is null) return this.NotFound("User not found.");
            var resultPost = this.postService.CreateNewPost(request.Content, user, request.PostFiles); 
            return resultPost is not null ? this.Ok(resultPost) : this.Conflict();
        }

        [Route("reply/{postId}/{userId}")]
        [HttpPost]
        [Authorize( HasIdEqualToUserIdParamPolicyName )]
        public IActionResult ReplyToPost(long postId, long userId, PostCreationRequest request)
        {
            var user = this.userService.GetUserById(userId);
            if(user is null) return this.NotFound("User not found.");
            var resultPost = this.postService.ReplyToPost(postId, request.Content, user, request.PostFiles);
            return resultPost is not null ? this.Ok(resultPost) : this.NotFound("Original Post not found.");
        }
        
        [Route("{id}")]
        [HttpDelete]
        [Authorize( CreatedPostPolicyName )]
        public IActionResult Delete(long id)
            => this.postService.RemovePost(id) ? this.Ok() : this.NotFound();

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
            => this.Ok(this.postService.GetAllPosts());

        [Route("{id}")]
        [HttpGet]
        [Authorize]
        public IActionResult Get(long id)
        {
            var user = this.postService.GetPostById(id);
            return user is not null ? this.Ok(user) : this.NotFound();
        }

        [Route("{postId}/interest/set/{userId}")]
        [HttpPost]
        [Authorize( Policy = HasIdEqualToUserIdParamPolicyName )]
        public IActionResult DeclareInterest(uint userId, uint postId)
        {   
            var user = this.userService.GetUserById(userId);
            var post = this.postService.GetPostById(postId);
            if(user is null || post is null) return this.NotFound("User not found.");
            
            var result = this.interestService.DeclareInterestForPost(userId, postId);
            if(result == UpdateResult.Ok){
                this.notificationService.SendNotificationTo(post.PostedBy, $"{user.Name} was interested in your post!", post);
            }
            return result.ToResultObject(this);
        }

        [Route("{postId}/interest/unset/{userId}")]
        [HttpPost]
        [Authorize( Policy = HasIdEqualToUserIdParamPolicyName )]
        public IActionResult RemoveInterest(uint userId, uint postId)
        {
            return this.interestService.RemoveInterestForPost(userId, postId).ToResultObject(this);
        }

    }
}
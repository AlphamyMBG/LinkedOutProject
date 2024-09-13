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
    (IPostService postService, IInterestService interestService, IRegularUserService userService) 
    : ControllerBase
    {
        private readonly IPostService postService = postService;
        private readonly IInterestService interestService = interestService;
        private readonly IRegularUserService userService = userService;

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
            var resultPost = this.postService.CreateNewPost(request.Content, user); 
            return resultPost is not null ? this.Ok(resultPost) : this.Conflict();
        }

        [Route("reply/{postId}/{userId}")]
        [HttpPost]
        [Authorize( HasIdEqualToUserIdParamPolicyName )]
        public IActionResult ReplyToPost(long postId, long userId, PostCreationRequest request)
        {
            var user = this.userService.GetUserById(userId);
            if(user is null) return this.NotFound("User not found.");
            var resultPost = this.postService.ReplyToPost(postId, request.Content, user);
            return resultPost is not null ? this.Ok(resultPost) : this.NotFound("Original Post not found.");
        }
        
        [Route("{id}")]
        [HttpPost]
        [Authorize( IsAdminPolicyName )]
        public IActionResult UpdatePost(long id, Post post)
            => this.postService.UpdatePost(id, post) switch
            {
                UpdateResult.KeyAlreadyExists => this.Conflict(),
                UpdateResult.NotFound => this.NotFound(),
                UpdateResult.Ok => this.Ok(),
                _  => throw new Exception("Something went terribly wrong for you to be here.") 
            };
        
        [Route("{id}")]
        [HttpDelete]
        [Authorize]
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
            return this.interestService.DeclareInterestForPost(userId, postId).ToResultObject(this);
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
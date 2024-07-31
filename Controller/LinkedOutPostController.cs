using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BackendApp.Model;
using BackendApp.Model.Enums;
using BackendApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace BackendApp.Controller
{

    [Route("api/[Controller]")]
    [ApiController]
    public class LinkedOutPostController
    (ILinkedOutPostService postService, IInterestService interestService) 
    : ControllerBase
    {
        private readonly ILinkedOutPostService postService = postService;
        private readonly IInterestService interestService = interestService;

        [HttpPost]
        public IActionResult CreatePost(Post post)
            => this.postService.AddPost(post) ? this.Ok(post.Id) : this.Conflict();
        
        [Route("{id}")]
        [HttpPost]
        public IActionResult UpdatePost(ulong id, Post post)
            => this.postService.UpdatePost(id, post) switch
            {
                UpdateResult.KeyAlreadyExists => this.Conflict(),
                UpdateResult.NotFound => this.NotFound(),
                UpdateResult.Ok => this.Ok(),
                _  => throw new Exception("Something went terribly wrong for you to be here.") 
            };
        
        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete(ulong id)
            => this.postService.RemovePost(id) ? this.Ok() : this.NotFound();

        [HttpGet]
        public IActionResult GetAll()
            => this.Ok(this.postService.GetAllPosts());

        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(ulong id)
        {
            var user = this.postService.GetPostById(id);
            return user is not null ? this.Ok(user) : this.NotFound();
        }

        [Route("{postId}/interest/set/{userId}")]
        [HttpPost]
        public IActionResult DeclareInterest(uint userId, uint postId)
        {
            return this.interestService.DeclareInterestForPost(userId, postId).ToResultObject(this);
        }

        [Route("{postId}/interest/unset/{userId}")]
        [HttpPost]
        public IActionResult RemoveInterest(uint userId, uint postId)
        {
            return this.interestService.RemoveInterestForPost(userId, postId).ToResultObject(this);
        }
    }
}
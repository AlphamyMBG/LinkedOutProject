using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BackendApp.Model;
using BackendApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace BackendApp.Controller
{

    [Route("api/[Controller]")]
    [ApiController]
    public class LinkedOutPostController(ILinkedOutPostService postService) : ControllerBase
    {
        private readonly ILinkedOutPostService postService = postService;

        [HttpPost]
        public IActionResult CreatePost(LinkedOutPost post)
            => this.postService.AddPost(post) ? this.Ok(post.Id) : this.Conflict();
        
        [Route("{id}")]
        [HttpPost]
        public IActionResult UpdatePost(ulong id, LinkedOutPost post)
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
    }
}
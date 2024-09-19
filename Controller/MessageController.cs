using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApp.Model;
using BackendApp.Model.Enums;
using BackendApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BackendApp.Auth.AuthConstants.PolicyNames;


namespace BackendApp.Controller
{
    [Route("api/[Controller]")]
    [ApiController]
    public class MessageController(IMessageService messageService, IRegularUserService userService) : ControllerBase
    {
        private readonly IMessageService messageService = messageService;  
        private readonly IRegularUserService userService = userService;  

        [Route("{id}")]
        [HttpDelete]
        [Authorize( SentMessagePolicyName )]
        public IActionResult Delete(long id)
            => this.messageService.RemoveMessage(id) ? this.Ok() : this.NotFound();

        [HttpGet]
        [Authorize( IsAdminPolicyName )]
        public IActionResult GetAll()
            => this.Ok(this.messageService.GetAllMessages());

        [Route("{id}")]
        [HttpGet]
        [Authorize( SentMessagePolicyName )]
        public IActionResult Get(long id)
        {
            var Message = this.messageService.GetMessageById(id);
            return Message is not null ? this.Ok(Message) : this.NotFound();
        }

        [Route("send/{senderId}/{receipientId}")]
        [HttpPost]
        [Authorize( HasIdEqualToSenderIdPolicyName )]
        public IActionResult Send(uint senderId, uint receipientId, string content)
        {
            return this.messageService.SendMessage(senderId, receipientId, content) ? this.Ok() : this.NotFound();
        }

        [Route("chat/{userAId}/{userBId}")]
        [HttpGet]
        [Authorize( IsMemberOfConversationPolicyName )]
        public IActionResult GetChatHistory(uint userAId, uint userBId) 
            => this.Ok(this.messageService.GetConversationBetween(userAId, userBId));

        [Route("chat/{userAId}/{userBId}/{startAt}/{endAfter}")]
        [HttpGet]
        [Authorize( IsMemberOfConversationPolicyName )]
        public IActionResult GetChatHistory(uint userAId, uint userBId, int startAt, int endAfter) 
            => this.Ok(this.messageService.GetRangeOfConversationBetween(userAId, userBId, startAt, endAfter));
        

        [Route("chat/members/{userId}")]
        [HttpGet]
        [Authorize( HasIdEqualToUserIdParamPolicyName )]
        public IActionResult GetUsers(long userId)
        {
            var user = this.userService.GetUserById(userId);
            if(user is null) return this.NotFound("User not found.");
            return this.Ok(this.messageService.GetMembersOfChatsWith(user));
        }
    }
}
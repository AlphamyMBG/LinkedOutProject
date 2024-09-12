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
    public class MessageController(IMessageService messageService) : ControllerBase
    {
        private readonly IMessageService messageService = messageService;  

        [HttpPost]
        [Authorize( IsAdminPolicyName )]
        public IActionResult CreateMessage(Message message)
            => this.messageService.AddMessage(message) ? this.Ok(message.Id) : this.Conflict();

        [Route("{id}")]
        [HttpPost]
        [Authorize( IsAdminPolicyName )]
        public IActionResult UpdateMessage(long id, Message notification)
            => this.messageService.UpdateMessage(id, notification) switch
            {
                UpdateResult.KeyAlreadyExists => this.Conflict(),
                UpdateResult.NotFound => this.NotFound(),
                UpdateResult.Ok => this.Ok(),
                _  => throw new Exception("Something went terribly wrong for you to be here.") 
            };

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
        [Authorize]
        public IActionResult GetChatHistory(uint userAId, uint userBId) 
            => this.Ok(this.messageService.GetConversationBetween(userAId, userBId));

        [Route("chat/{userAId}/{userBId}/{startAt}/{endAfter}")]
        [HttpGet]
        [Authorize]
        public IActionResult GetChatHistory(uint userAId, uint userBId, int startAt, int endAfter) 
            => this.Ok(this.messageService.GetRangeOfConversationBetween(userAId, userBId, startAt, endAfter));

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApp.Model;
using BackendApp.Model.Enums;
using BackendApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace BackendApp.Controller
{
    [Route("api/[Controller]")]
    [ApiController]
    public class MessageController(IMessageService messageService) : ControllerBase
    {
        private readonly IMessageService messageService = messageService;  

        [HttpPost]
        public IActionResult CreateMessage(Message message)
            => this.messageService.AddMessage(message) ? this.Ok(message.Id) : this.Conflict();

        [Route("{id}")]
        [HttpPost]
        public IActionResult UpdateMessage(ulong id, Message notification)
            => this.messageService.UpdateMessage(id, notification) switch
            {
                UpdateResult.KeyAlreadyExists => this.Conflict(),
                UpdateResult.NotFound => this.NotFound(),
                UpdateResult.Ok => this.Ok(),
                _  => throw new Exception("Something went terribly wrong for you to be here.") 
            };

        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete(ulong id)
            => this.messageService.RemoveMessage(id) ? this.Ok() : this.NotFound();

        [HttpGet]
        public IActionResult GetAll()
            => this.Ok(this.messageService.GetAllMessages());

        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(ulong id)
        {
            var Message = this.messageService.GetMessageById(id);
            return Message is not null ? this.Ok(Message) : this.NotFound();
        }
    }
}
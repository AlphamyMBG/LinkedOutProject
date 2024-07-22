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
    public class NotificationController(INotificationService notifService) : ControllerBase
    {
        private readonly INotificationService notificationService = notifService;  

        [HttpPost]
        public IActionResult CreateNotification(Notification notif)
            => this.notificationService.AddNotifications(notif) ? this.Ok(notif.Id) : this.Conflict();
        
        [Route("{id}")]
        [HttpPost]
        public IActionResult UpdateNotification(ulong id, Notification post)
            => this.notificationService.UpdateNotifications(id, post) switch
            {
                UpdateResult.KeyAlreadyExists => this.Conflict(),
                UpdateResult.NotFound => this.NotFound(),
                UpdateResult.Ok => this.Ok(),
                _  => throw new Exception("Something went terribly wrong for you to be here.") 
            };
        
        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete(ulong id)
            => this.notificationService.RemoveNotifications(id) ? this.Ok() : this.NotFound();

        [HttpGet]
        public IActionResult GetAll()
            => this.Ok(this.notificationService.GetAllNotifications());

        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(ulong id)
        {
            var notification = this.notificationService.GetNotificationById(id);
            return notification is not null ? this.Ok(notification) : this.NotFound();
        }
    }
}
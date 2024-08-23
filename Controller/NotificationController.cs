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
    public class NotificationController
    (INotificationService notifService) 
    : ControllerBase
    {
        private readonly INotificationService notificationService = notifService;  

        [HttpPost]
        [Authorize( IsAdminPolicyName )]
        public IActionResult CreateNotification(Notification notif)
            => this.notificationService.AddNotification(notif) ? this.Ok(notif.Id) : this.Conflict();
        
        [Route("{id}")]
        [HttpPost]
        [Authorize( IsAdminPolicyName )]
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
        [Authorize( HasNotificationPolicyName )]
        public IActionResult Delete(ulong id)
            => this.notificationService.RemoveNotifications(id) ? this.Ok() : this.NotFound();

        [HttpGet]
        [Authorize( IsAdminPolicyName )]
        public IActionResult GetAll()
            => this.Ok(this.notificationService.GetAllNotifications());

        [Route("{id}")]
        [HttpGet]
        [Authorize( HasNotificationPolicyName )]
        public IActionResult Get(ulong id)
        {
            var notification = this.notificationService.GetNotificationById(id);
            return notification is not null ? this.Ok(notification) : this.NotFound();
        }

        [Route("my/{id}")]
        [HttpGet]
        [Authorize( Policy = HasIdEqualToIdParamPolicyName )]
        public IActionResult GetUsersNotifications(ulong id)
            => this.Ok(this.notificationService.GetNotificationsForUser(id));

        [Route("read/{id}")]
        [HttpGet]
        [Authorize( HasNotificationPolicyName )]
        public IActionResult MarkNotificationAsRead(ulong id)
            => this.notificationService.MarkNotificationAsRead(id) ? this.Ok() : this.NotFound();
    }
}
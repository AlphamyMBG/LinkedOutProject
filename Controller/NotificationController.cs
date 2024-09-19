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
        
        [Route("{id}")]
        [HttpDelete]
        [Authorize( HasNotificationPolicyName )]
        public IActionResult Delete(long id)
            => this.notificationService.RemoveNotifications(id) ? this.Ok() : this.NotFound();

        [HttpGet]
        [Authorize( IsAdminPolicyName )]
        public IActionResult GetAll()
            => this.Ok(this.notificationService.GetAllNotifications());

        [Route("{id}")]
        [HttpGet]
        [Authorize( HasNotificationPolicyName )]
        public IActionResult Get(long id)
        {
            var notification = this.notificationService.GetNotificationById(id);
            return notification is not null ? this.Ok(notification) : this.NotFound();
        }

        [Route("my/{id}")]
        [HttpGet]
        [Authorize( Policy = HasIdEqualToIdParamPolicyName )]
        public IActionResult GetUsersNotifications(long id)
            => this.Ok(this.notificationService.GetNotificationsForUser(id));

        [Route("read/{id}")]
        [HttpGet]
        [Authorize( HasNotificationPolicyName )]
        public IActionResult MarkNotificationAsRead(long id)
            => this.notificationService.MarkNotificationAsRead(id) ? this.Ok() : this.NotFound();
    }
}
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
    public class ConnectionController
    (IConnectionService connectionService, IRegularUserService userService) 
    : ControllerBase
    {
        private readonly IConnectionService connectionService = connectionService;  
        private readonly IRegularUserService userService = userService;  

        [HttpPost]
        [Authorize( IsAdminPolicyName )]
        public IActionResult CreateConnection(Connection Connection)
            => this.connectionService.AddConnection(Connection) ? this.Ok(Connection.Id) : this.Conflict();

        [Route("{id}")]
        [HttpPost]
        [Authorize( IsAdminPolicyName )]
        public IActionResult UpdateConnection(long id, Connection notification)
            => this.connectionService.UpdateConnection(id, notification) switch
            {
                UpdateResult.KeyAlreadyExists => this.Conflict(),
                UpdateResult.NotFound => this.NotFound(),
                UpdateResult.Ok => this.Ok(),
                _  => throw new Exception("Something went terribly wrong for you to be here.") 
            };

        [Route("{id}")]
        [HttpDelete]
        [Authorize( IsAdminPolicyName )] //TODO: Add apropriate filter
        public IActionResult Delete(long id)
            => this.connectionService.RemoveConnection(id) ? this.Ok() : this.NotFound();

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
            => this.Ok(this.connectionService.GetAllConnections());

        [Route("{id}")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get(long id)
        {
            var Connection = this.connectionService.GetConnectionById(id);
            return Connection is not null ? this.Ok(Connection) : this.NotFound();
        }

        [Route("send/{senderId}/{receipientId}")]
        [HttpPost]
        [Authorize( Policy = HasIdEqualToSenderIdPolicyName)]
        public IActionResult Send(uint senderId, uint receipientId)
        {
            return this.connectionService.SendConnectionRequest(senderId, receipientId) ? this.Ok() : this.NotFound();
        }
        
        [Route("accept/{id}")]
        [HttpPost]
        [Authorize( ReceivedConnectionRequestPolicyName )]
        public IActionResult Accept(uint id)
        {
            return this.connectionService.AcceptConnectionRequest(id) ? this.Ok() : this.NotFound();
        }

        [Route("decline/{id}")]
        [HttpPost]
        [Authorize( ReceivedConnectionRequestPolicyName )]
        public IActionResult Decline(uint id, RegularUser connectionReceipient)
        {
            return this.connectionService.DeclineConnectionRequest(connectionReceipient, id) ? this.Ok() : this.NotFound();
        }

        [Route("network/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetUsersNetwork(uint id)
        {
            if(this.userService.GetUserById(id) is not RegularUser user) return this.NotFound("User not found.");
            return this.Ok(this.connectionService.GetUsersConnectedTo(user));
        }
    
    }
}
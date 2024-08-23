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
    (IConnectionService connectionService) 
    : ControllerBase
    {
        private readonly IConnectionService ConnectionService = connectionService;  

        [HttpPost]
        [Authorize( IsAdminPolicyName )]
        public IActionResult CreateConnection(Connection Connection)
            => this.ConnectionService.AddConnection(Connection) ? this.Ok(Connection.Id) : this.Conflict();

        [Route("{id}")]
        [HttpPost]
        [Authorize( IsAdminPolicyName )]
        public IActionResult UpdateConnection(ulong id, Connection notification)
            => this.ConnectionService.UpdateConnection(id, notification) switch
            {
                UpdateResult.KeyAlreadyExists => this.Conflict(),
                UpdateResult.NotFound => this.NotFound(),
                UpdateResult.Ok => this.Ok(),
                _  => throw new Exception("Something went terribly wrong for you to be here.") 
            };

        [Route("{id}")]
        [HttpDelete]
        [Authorize( IsAdminPolicyName )] //TODO: Add apropriate filter
        public IActionResult Delete(ulong id)
            => this.ConnectionService.RemoveConnection(id) ? this.Ok() : this.NotFound();

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
            => this.Ok(this.ConnectionService.GetAllConnections());

        [Route("{id}")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get(ulong id)
        {
            var Connection = this.ConnectionService.GetConnectionById(id);
            return Connection is not null ? this.Ok(Connection) : this.NotFound();
        }

        [Route("send/{senderId}/{receipientId}")]
        [HttpPost]
        [Authorize( Policy = HasIdEqualToSenderIdPolicyName)]
        public IActionResult Send(uint senderId, uint receipientId)
        {
            return this.ConnectionService.SendConnectionRequest(senderId, receipientId) ? this.Ok() : this.NotFound();
        }
        [Route("accept/{id}")]
        [HttpPost]
        [Authorize( ReceivedConnectionRequestPolicyName )]
        public IActionResult Accept(uint id, RegularUser connectionReceipient)
        {
            return this.ConnectionService.AcceptConnectionRequest(connectionReceipient, id) ? this.Ok() : this.NotFound();
        }
        [Route("decline/{id}")]
        [HttpPost]
        [Authorize( ReceivedConnectionRequestPolicyName )]
        public IActionResult Decline(uint id, RegularUser connectionReceipient)
        {
            return this.ConnectionService.DeclineConnectionRequest(connectionReceipient, id) ? this.Ok() : this.NotFound();
        }
    
    }
}
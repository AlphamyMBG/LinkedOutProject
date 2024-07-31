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
    public class ConnectionController
    (IConnectionService connectionService) 
    : ControllerBase
    {
        private readonly IConnectionService ConnectionService = connectionService;  

        [HttpPost]
        public IActionResult CreateConnection(Connection Connection)
            => this.ConnectionService.AddConnection(Connection) ? this.Ok(Connection.Id) : this.Conflict();

        [Route("{id}")]
        [HttpPost]
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
        public IActionResult Delete(ulong id)
            => this.ConnectionService.RemoveConnection(id) ? this.Ok() : this.NotFound();

        [HttpGet]
        public IActionResult GetAll()
            => this.Ok(this.ConnectionService.GetAllConnections());

        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(ulong id)
        {
            var Connection = this.ConnectionService.GetConnectionById(id);
            return Connection is not null ? this.Ok(Connection) : this.NotFound();
        }

        [Route("send/{senderId}/{receipientId}")]
        [HttpPost]
        public IActionResult Send(uint senderId, uint receipientId)
        {
            return this.ConnectionService.SendConnectionRequest(senderId, receipientId) ? this.Ok() : this.NotFound();
        }
        [Route("accept/{conId}")]
        [HttpPost]
        public IActionResult Accept(uint conId, RegularUser connectionReceipient)
        {
            return this.ConnectionService.AcceptConnectionRequest(connectionReceipient, conId) ? this.Ok() : this.NotFound();
        }
        [Route("decline/{conId}")]
        [HttpPost]
        public IActionResult Decline(uint conId, RegularUser connectionReceipient)
        {
            return this.ConnectionService.DeclineConnectionRequest(connectionReceipient, conId) ? this.Ok() : this.NotFound();
        }
    
    }
}
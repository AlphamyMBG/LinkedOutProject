using BackendApp.Data;
using BackendApp.Model;
using BackendApp.Model.Enums;

namespace BackendApp.Service{
    public interface IConnectionService {
        public Connection? GetConnectionById(ulong id);
        public Connection[] GetAllConnections();
        public Connection[] GetAllConnectionsSentBy(ulong userId);
        public Connection[] GetAllConnectionsSentTo(ulong userId);
        public bool AddConnection(Connection Connection);
        public bool RemoveConnection(ulong id);
        public UpdateResult UpdateConnection(ulong id, Connection Connection);
        public bool SendConnectionRequest(RegularUser from, RegularUser to);
        public bool SendConnectionRequest(uint from, uint to);
        public bool DeclineConnectionRequest(RegularUser user, uint connectionId);
        public bool AcceptConnectionRequest(RegularUser user, uint connectionId);
    }

    public class ConnectionService(ApiContext context, IRegularUserService userService) : IConnectionService
    {
        private readonly ApiContext context = context;
        private readonly IRegularUserService userService = userService;
        public bool AddConnection(Connection Connection)
        {
            if(this.GetConnectionById(Connection.Id) is not null) return false;
            this.context.Connections.Add(Connection);
            this.context.SaveChanges();
            return true;
        }

        public Connection[] GetAllConnections()
            => [.. this.context.Connections];
        
        public Connection[] GetAllConnectionsSentBy(ulong userId)
            => this.context.Connections
                .Where(Connection => Connection.SentBy.Id == userId)
                .OrderBy(Connection => Connection.Timestamp)
                .ToArray();
        public Connection[] GetAllConnectionsSentTo(ulong userId)
            => this.context.Connections
                .Where(Connection => Connection.SentTo.Id == userId)
                .OrderBy(Connection => Connection.Timestamp)
                .ToArray();
        
        public bool ConnectionHasBeenRequested(RegularUser by, RegularUser to)
        {
            return this.GetAllConnectionsSentBy(by.Id)
                .FirstOrDefault(user => user.Id == to.Id) is not null;
        }

        public bool AreConnected(RegularUser userA, RegularUser userB)
        {
            return this.context.Connections
                .Where(
                    con => con.IsBetween(userA, userB) && con.Accepted
                )
                .Any();
        }
        public Connection? GetConnectionById(ulong id)
            => this.context.Connections.FirstOrDefault( 
                Connection => Connection.Id == id 
            );

        public bool RemoveConnection(ulong id){
            Connection? Connection = this.GetConnectionById(id);
            if( Connection is null ) return false;

            this.context.Connections.Remove(Connection);
            this.context.SaveChanges();
            return true;
        }

        public UpdateResult UpdateConnection(ulong id, Connection Connection)
        {
            //Check if user exists
            Connection? ConnectionInDb = this.GetConnectionById(id);
            if(ConnectionInDb is null) return UpdateResult.NotFound;

            //Save new data
            ConnectionInDb.Update(Connection);
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }

        public bool SendConnectionRequest(RegularUser from, RegularUser to)
        {   
            if(
                this.ConnectionHasBeenRequested(from, to)
                || this.ConnectionHasBeenRequested(to, from)
            ) return false;
            var Connection = new Connection
            (
                sentBy: from,
                sentTo: to,
                accepted: false,
                timestamp: DateTime.Now
            );
            this.context.Connections.Add(Connection);
            this.context.SaveChanges();
            return true;
        }
        public bool SendConnectionRequest(uint fromId, uint toId)
        {
            var from = this.userService.GetUserById(fromId);
            if(from is null) return false;
            var to = this.userService.GetUserById(toId);
            if(to is null) return false;
            return this.SendConnectionRequest(from, to);
        }

        public bool AcceptConnectionRequest(RegularUser user, uint connectionId)
        {
            Connection? connection = this.GetConnectionById(connectionId);
            if(connection is null || connection.SentBy.Id != user.Id) return false;
            connection.Accepted = true;
            this.context.SaveChanges();
            return true;
        }

        public bool DeclineConnectionRequest(RegularUser user, uint connectionId)
        {
            Connection? connection = this.GetConnectionById(connectionId);
            if(connection is null || connection.SentBy.Id != user.Id) return false;
            this.RemoveConnection(connection.Id);
            return true;
        }
        public bool AcceptConnectionRequest(uint userId, uint connectionId)
        {
            var to = this.userService.GetUserById(userId);
            if(to is null) return false;
            return this.AcceptConnectionRequest(to, connectionId);
        }

        public bool DeclineConnectionRequest(uint userId, uint connectionId)
        {
            var to = this.userService.GetUserById(userId);
            if(to is null) return false;
            return this.DeclineConnectionRequest(to, connectionId);
        }

    }
}
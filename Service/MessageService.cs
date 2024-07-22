using BackendApp.Data;
using BackendApp.Model;
using BackendApp.Model.Enums;

namespace BackendApp.Service{
    public interface IMessageService {
        public Message? GetMessageById(ulong id);
        public Message[] GetAllMessages();
        public bool AddMessage(Message message);
        public bool RemoveMessage(ulong id);
        public UpdateResult UpdateMessage(ulong id, Message message);
    }

    public class MessageService(ApiContext context) : IMessageService
    {
        private readonly ApiContext context = context;
        public bool AddMessage(Message message)
        {
            if(this.GetMessageById(message.Id) is not null) return false;
            this.context.Messages.Add(message);
            this.context.SaveChanges();
            return true;
        }

        public Message[] GetAllMessages()
            => [.. this.context.Messages];
        public Message? GetMessageById(ulong id)
            => this.context.Messages.FirstOrDefault( 
                message => message.Id == id 
            );

        public bool RemoveMessage(ulong id){
            Message? message = this.GetMessageById(id);
            if( message is null ) return false;

            this.context.Messages.Remove(message);
            this.context.SaveChanges();
            return true;
        }

        public UpdateResult UpdateMessage(ulong id, Message message)
        {
            //Check if user exists
            Message? messageInDb = this.GetMessageById(id);
            if(messageInDb is null) return UpdateResult.NotFound;

            //Save new data
            messageInDb.Update(message);
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }
    }
}
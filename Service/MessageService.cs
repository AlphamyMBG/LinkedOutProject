using BackendApp.Data;
using BackendApp.Model;
using BackendApp.Model.Enums;

namespace BackendApp.Service{
    public interface IMessageService {
        public Message? GetMessageById(ulong id);
        public Message[] GetAllMessages();
        public Message[] GetAllMessagesSentBy(uint userId);
        public Message[] GetAllMessagesSentTo(uint userId);
        public Message[] GetConversationBetween(uint userA, uint userB);
        public bool AddMessage(Message message);
        public bool RemoveMessage(ulong id);
        public UpdateResult UpdateMessage(ulong id, Message message);
        public bool SendMessage(RegularUser from, RegularUser to, string content);
        public bool SendMessage(uint from, uint to, string content);
        public Message[] GetRangeOfConversationBetween(uint userAId, uint userBId, int startAt, int endAfter);
    }

    public class MessageService(ApiContext context, IRegularUserService userService) : IMessageService
    {
        private readonly ApiContext context = context;
        private readonly IRegularUserService userService = userService;
        public bool AddMessage(Message message)
        {
            if(this.GetMessageById(message.Id) is not null) return false;
            this.context.Messages.Add(message);
            this.context.SaveChanges();
            return true;
        }

        public Message[] GetAllMessages()
            => [.. this.context.Messages];
        
        public Message[] GetAllMessagesSentBy(uint userId)
            => this.context.Messages
                .Where(message => message.SentBy.Id == userId)
                .OrderBy(message => message.Timestamp)
                .ToArray();
        public Message[] GetAllMessagesSentTo(uint userId)
            => this.context.Messages
                .Where(message => message.SentTo.Id == userId)
                .OrderBy(message => message.Timestamp)
                .ToArray();
        
        public Message[] GetConversationBetween(uint userAId, uint userBId)
            => this.context.Messages
                .Where(
                    message => 
                        (message.SentBy.Id == userAId && message.SentTo.Id == userBId)
                        || (message.SentBy.Id == userBId && message.SentTo.Id == userAId)
                )
                .OrderBy(message => message.Timestamp)
                .ToArray();

        public Message[] GetRangeOfConversationBetween(uint userAId, uint userBId, int startAt, int endAfter)
        {
            if(startAt < 0 || endAfter < 0) return [];
            return this.context.Messages
                .Where(
                    message => 
                        (message.SentBy.Id == userAId && message.SentTo.Id == userBId)
                        || (message.SentBy.Id == userBId && message.SentTo.Id == userAId)
                )
                .OrderBy(message => message.Timestamp)
                .Take(startAt + endAfter)
                .TakeLast(endAfter)
                .OrderBy(message => message.Timestamp)
                .ToArray();
        }
        

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

        public bool SendMessage(RegularUser from, RegularUser to, string content)
        {
            var message = new Message
            (
                content: content,
                sentBy: from,
                sentTo: to,
                DateTime.Now //I'd rather kill myself than manage time for different timezones thank you
            );
            this.context.Messages.Add(message);
            this.context.SaveChanges();
            return true;
        }

        public bool SendMessage(uint fromId, uint toId, string content)
        {
            var from = this.userService.GetUserById(fromId);
            if(from is null) return false;
            var to = this.userService.GetUserById(toId);
            if(to is null) return false;
            return this.SendMessage(from, to, content);
        }
    }
}
namespace Backend.Model
{
public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string SenderId { get; set; }
        public string ChatId {get; set; }
        public string Content {get; set;}
        public DateTime SentAt {get; set;}
        
        public Message(string senderId, string chatId, string content, DateTime sentAt ) {
            SenderId = senderId ;
            ChatId = chatId ;
            Content = content;
            SentAt = sentAt;
        }


            
    }}
    
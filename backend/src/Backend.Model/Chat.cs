namespace Backend.Model
{
public class Chat
    {
        public (string,string) ParticipantId { get; set; }
        public string Id {get; set; }
        
        public Chat(string doctorId,string patientId) {
            ParticipantId = (doctorId,patientId);
            Id = doctorId + patientId;
        }


            
    }}
    
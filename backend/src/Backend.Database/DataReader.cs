using Backend.Interfaces;
using Backend.Model;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Backend.Database
{
    public class DataReader : IDataReader
    {
        public MongoClient Client { get; set; }
        public DataReader(MongoClient client)
        {
            Client = client;
        }
        public Identity GetIdentity(string id)
        {
            return Client.GetDatabase("Identities").GetCollection<Identity>("Identities").Find(x => x.Id == id).FirstOrDefault();
        }

        public List<Identity> GetPendingIdentities()
        {
            return Client.GetDatabase("Identities").GetCollection<Identity>("Identities").Find(x => x.Role == Role.PendingPatient || x.Role == Role.PendingDoctor).ToList();
        }

        public List<Identity> GetAllDoctors()
        {
            var db = Client.GetDatabase("Identities").GetCollection<Identity>("Identities");
            return db.Find(x => x.Role == Role.Doctor).ToList();
        }
        public Identity GetDoctorForPatient(string patientId)
        {
            var db = Client.GetDatabase("Identities").GetCollection<Identity>("Identities");
            return db.Find(x => x.Role == Role.Doctor && x.Patients.Contains(patientId)).First();
        }
        public List<Identity> GetAllPatients()
        {
            return Client.GetDatabase("Identities").GetCollection<Identity>("Identities").Find(x => x.Role == Role.Patient).ToList();
        }

        public List<Chat> GetChats(string id)
        {
            return Client.GetDatabase("Identities").GetCollection<Chat>("Chat").Find(x => x.ParticipantId.Item1 == id || x.ParticipantId.Item2 == id).ToList();
        }

        public Chat GetChat(string id)
        {
            return Client.GetDatabase("Identities").GetCollection<Chat>("Chat").Find(x => x.Id == id).First();
        }

        public List<Message> GetMessages(string id)
        {
            var messages =  Client.GetDatabase("Identities").GetCollection<Message>("Message").Find(x => x.ChatId == id)?.ToList();
            return messages;

        }
    }
}
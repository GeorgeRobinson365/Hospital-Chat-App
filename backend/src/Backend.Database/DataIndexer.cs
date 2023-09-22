using Backend.Interfaces;
using Backend.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Database
{
    public class DataIndexer : IDataIndexer
    {
        public MongoClient Client { get; set; }
        public DataIndexer(MongoClient client)
        {
            Client = client;
        }
        public Task IndexIdentity(Identity identity)
        {
            //Add model to database
            return Client.GetDatabase("Identities").GetCollection<Identity>("Identities").InsertOneAsync(identity);
        }

        public Task SubmitApproval(string id, Role newRole)
        {
            return Client.GetDatabase("Identities").GetCollection<Identity>("Identities").UpdateOneAsync(x=>x.Id == id, Builders<Identity>.Update.Set(x=>x.Role, newRole));
        }
        public Task SubmitPatientAccess(string doctorId, Identity updatedIdentity)
        {
            return Client.GetDatabase("Identities").GetCollection<Identity>("Identities").UpdateOneAsync(x => x.Id == doctorId, Builders<Identity>.Update.Set(x => x.Patients, updatedIdentity.Patients));
        }

        public Task CreateChat(Chat chat)
        {
            return Client.GetDatabase("Identities").GetCollection<Chat>("Chat").InsertOneAsync(chat);
        }
        public Task SendMessage(Message message)
        {
            return Client.GetDatabase("Identities").GetCollection<Message>("Message").InsertOneAsync(message);
        }

        public DeleteResult DeleteIdentity(string uid)
        {
            return Client.GetDatabase("Identities").GetCollection<Identity>("Identities").DeleteOne(x=>x.Id == uid);

        }
    }
}

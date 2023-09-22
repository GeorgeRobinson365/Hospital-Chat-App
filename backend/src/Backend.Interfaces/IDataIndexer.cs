using Backend.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IDataIndexer
    {
        public Task IndexIdentity(Identity identity);
        public Task SubmitApproval(string id, Role newRole);
        public Task SubmitPatientAccess(string doctorId, Identity updatedIdentity);
        public Task CreateChat(Chat chat);
        public Task SendMessage(Message message);
        public DeleteResult DeleteIdentity(string uid);
    }
}

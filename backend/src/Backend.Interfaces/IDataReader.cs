using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Model;
namespace Backend.Interfaces
{
    public interface IDataReader
    {
        public Identity GetIdentity(string id);
        public List<Identity> GetPendingIdentities();
        public Identity GetDoctorForPatient(string patientId);
        public List<Identity> GetAllDoctors();
        public List<Identity> GetAllPatients();
        public List<Chat> GetChats(string id);
        public Chat GetChat(string chatId);
        public List<Message> GetMessages(string id);

    }
}

using Backend.Interfaces;
using Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.API.Tests.Fakes
{
    internal class FakeDataReader : IDataReader
    {
        public List<Identity> GetAllDoctors()
        {
            var baseIdentity1 = new Identity
            {
                Id="123",
                Patients = null,
                Role = Role.Doctor
            };
            var baseIdentity2 = new Identity
            {
                Id = "456",
                Patients = new string[] {"123"},
                Role = Role.Doctor
            };
            return new List<Identity>
            {
                {baseIdentity1 },
                {baseIdentity2 } 
            };
        }

        public Identity GetIdentity(string id)
        {
            throw new NotImplementedException();
        }

        public List<Identity> GetPendingIdentities()
        {
            throw new NotImplementedException();
        }

        List<Identity> IDataReader.GetAllPatients()
        {
            throw new NotImplementedException();
        }

        Chat IDataReader.GetChat(string chatId)
        {
            throw new NotImplementedException();
        }

        List<Chat> IDataReader.GetChats(string id)
        {
            throw new NotImplementedException();
        }

        Identity IDataReader.GetDoctorForPatient(string patientId)
        {
            throw new NotImplementedException();
        }

        List<Message> IDataReader.GetMessages(string id)
        {
            throw new NotImplementedException();
        }
    }
}

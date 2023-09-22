using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend.Interfaces;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;

namespace Backend.API.Tests.Fakes
{
    internal class FirebaseManagerFake : IFirebaseManager
    {
        public string VerifyIdToken(string token)
        {
            if(token =="12") return "12";
            return "";
        }

        Task IFirebaseManager.DeleteUser(string uid)
        {
            throw new NotImplementedException();
        }

        string IFirebaseManager.GetDisplayName(string uid)
        {
            throw new NotImplementedException();
        }

        UserRecord IFirebaseManager.GetUser(string uid)
        {
            throw new NotImplementedException();
        }
    }
}

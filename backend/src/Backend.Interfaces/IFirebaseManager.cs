using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IFirebaseManager
    {
        public string VerifyIdToken(string token);
        public string GetDisplayName(string uid);
        public Task DeleteUser(string uid);
        public UserRecord GetUser(string uid);
    }
}

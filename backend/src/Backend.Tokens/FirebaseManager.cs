using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.Model;
using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Backend.Tokens
{
    public class FirebaseManager : IFirebaseManager
    {
        private readonly Settings _settings;
        public FirebaseManager(IConfiguration config)
        {
            _settings = config.Get<Settings>();
            var connJson = JsonConvert.SerializeObject(_settings.FirebaseConnection);
            connJson = Regex.Replace(connJson, @"/\\n/g", @"\n");
            connJson = connJson.Replace(@"\n", "\n");
            connJson = Regex.Unescape(connJson);
            try
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(connJson)
                });
            }
            catch (Exception ex)
            {
                // Default firebase app already exists
                // swallow this ex
            }
        }

        public string GetDisplayName(string uid)
        {
            return FirebaseAuth.DefaultInstance.GetUserAsync(uid).Result.DisplayName;
        }

        public UserRecord GetUser(string uid)
        {
            return FirebaseAuth.DefaultInstance.GetUserAsync(uid).Result;
        }

        public string VerifyIdToken(string token)
        {
            try
            {
                var res = FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token).Result;
                return res.Uid;
            } catch (FirebaseAuthException e)
            {
                throw new Exception("Firebase Token is invalid");
            }
        }
        public Task DeleteUser(string uid)
        {
            return FirebaseAuth.DefaultInstance.DeleteUserAsync(uid);
        }
    }
}

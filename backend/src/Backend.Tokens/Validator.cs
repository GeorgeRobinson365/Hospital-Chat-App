using Microsoft.AspNetCore.Http;
using JWT.Algorithms;
using JWT.Builder;
using System.Security.Claims;
using Backend.Model;
using Newtonsoft.Json.Linq;
using Backend.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

namespace Backend.Tokens
{
    public class Validator : IValidator
    {
        private JwtBuilder _builder;
        private string _secret;
        private IFirebaseManager _firebaseManager;
        public Validator(IConfiguration config, IFirebaseManager firebaseManager)
        {
            _secret = config.Get<Settings>().JWTSecret;
            _builder = JwtBuilder.Create().WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_secret)
                .MustVerifySignature();
            _firebaseManager = firebaseManager;
        }
        public async Task<bool> ValidatePatientDataRequest(HttpRequest req, string idOfPatientRequested)
        {
            var token = TokenExtractor.TryGet(req);
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            Identity claims = null;
            try
            {
                claims = _builder.Decode<Identity>(token);
            } catch(Exception e)
            {
                return false;
            }
            if (claims.Role == Role.Doctor || (claims.Role==Role.Patient && claims.Id == idOfPatientRequested))
                return true;
            return false;
        }

        public async Task<bool> ValidateIdentityRequest(HttpRequest req, string id)
        {
            var token = TokenExtractor.TryGet(req);
            var uid = _firebaseManager.VerifyIdToken(token);
            return id == uid;
        }

        public async Task<bool> ValidateAdminRequest(HttpRequest req)
        {
            var token = TokenExtractor.TryGet(req);
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            Identity claims = null;
            try
            {
                claims = _builder.Decode<Identity>(token);
            }
            catch (Exception e)
            {
                return false;
            }
            if (claims.Role == Role.Admin)
                return true;
            return false;
        }

        public async Task<bool> ValidateChatRequest(HttpRequest req, string id)
        {
            var token = TokenExtractor.TryGet(req);
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            Identity claims = null;
            try
            {
                claims = _builder.Decode<Identity>(token);
            }
            catch (Exception e)
            {
                return false;
            }
            if(claims.Id !=  id)
            {
                return false;
            }
            if (claims.Role == Role.Doctor || claims.Role == Role.Patient)
                return true;
            return false;
        }
        public async Task<bool> ValidateDeleteRequest(HttpRequest req, string id)
        {
            var token = TokenExtractor.TryGet(req);
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            Identity claims = null;
            try
            {
                claims = _builder.Decode<Identity>(token);
            }
            catch (Exception e)
            {
                return false;
            }
            if (claims.Role == Role.Admin || claims.Id == id)
                return true;
            return false;
        }

        public async Task<Identity> GetClaims(HttpRequest req)
        {
            var token = TokenExtractor.TryGet(req);
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            Identity claims = null;
            try
            {
                claims = _builder.Decode<Identity>(token);
                return claims;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}

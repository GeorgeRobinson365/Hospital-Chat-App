using Backend.Model;
using Microsoft.AspNetCore.Http;

namespace Backend.Interfaces
{
    public interface IValidator
    {
        public Task<bool> ValidatePatientDataRequest(HttpRequest req, string idOfPatientRequested);
        public Task<bool> ValidateIdentityRequest(HttpRequest req, string id);
        public Task<bool> ValidateAdminRequest(HttpRequest req);
        public Task<Identity> GetClaims(HttpRequest req);
        public Task<bool> ValidateChatRequest(HttpRequest req, string id);
        public Task<bool> ValidateDeleteRequest(HttpRequest req, string id);

    }
}
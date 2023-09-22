using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Net.Http.Headers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using System.Runtime.CompilerServices;
using Backend.Model;
using Backend.Interfaces;
using Backend.Tokens;
using Backend.Database;
using Backend.Commands;
using MongoDB.Driver;
using JWT.Builder;
using JWT.Algorithms;
using System.Xml.Linq;
using FirebaseAdmin.Auth;
using Newtonsoft.Json;
using ZstdSharp.Unsafe;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.API;

[ApiController]
public class Servicing
{
    private readonly Settings _settings;
    private readonly IValidator _validator;
    private readonly IFirebaseManager _firebaseManager;
    private readonly IDataReader _dataReader;
    private readonly IDataIndexer _dataIndexer;
    private readonly MongoClient _mongoClient;
    private readonly IFileWriter _writer;

    [ActivatorUtilitiesConstructor]
    public Servicing(IFirebaseManager firebaseManager, IValidator validator, IConfiguration config, IFileWriter writer)
    {
        _settings = config.Get<Settings>();
        _firebaseManager = firebaseManager;
        _validator = validator;
        _mongoClient = new MongoClient(_settings.MongoConnectionString);
        _dataReader = new DataReader(_mongoClient);
        _dataIndexer = new DataIndexer(_mongoClient);
        _writer = writer;
    }

    public Servicing(IFirebaseManager firebaseManager, IValidator validator, IConfiguration config, IDataIndexer dataIndexer, IFileWriter writer)
    {
        _settings = config.Get<Settings>();
        _firebaseManager = firebaseManager;
        _validator = validator;
        _mongoClient = new MongoClient(_settings.MongoConnectionString);
        _dataReader = new DataReader(_mongoClient);
        _dataIndexer = dataIndexer;
        _writer = writer;
    }

    
    public Servicing(IFirebaseManager firebaseManager, IValidator validator, IConfiguration config, IDataIndexer dataIndexer, IDataReader dataReader, IFileWriter writer)
    {
        _settings = config.Get<Settings>();
        _firebaseManager = firebaseManager;
        _validator = validator;
        _dataReader = dataReader;
        _dataIndexer = dataIndexer;
        _writer = writer;
    }

    [FunctionName("CreateIdentity")]
    [OpenApiOperation(operationId: "CreateIdentity", tags: new[] { "writes" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CloudRequest<CreateIdentity>), Description = "Parameter", Required = true)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.Accepted, contentType: "text/plain", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> CreateIdentity(
        [HttpTrigger(AuthorizationLevel.Function, methods:new []{ "post", "put"}, Route = "createidentity")]
        [FromBody][Required] CloudRequest<CreateIdentity> cloudRequest, HttpRequest req)
    {
        if (!_validator.ValidateIdentityRequest(req, cloudRequest.Data.Id).Result)
            return new UnauthorizedResult();
        try
        {
            var id = cloudRequest.Data.Id;
            var identity = new Identity(cloudRequest.Data.Id, cloudRequest.Data.Role);
            await _dataIndexer.IndexIdentity(identity);
            return new AcceptedResult(new Uri($"Get/{id}", UriKind.Relative), new { id = id });
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    [FunctionName("SubmitPatientAccess")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "writes" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CloudRequest<SubmitPatientAccess>), Description = "Parameter", Required = true)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.Accepted, contentType: "text/plain", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> SubmitPatientAccess(
    [HttpTrigger(AuthorizationLevel.Function, methods:new []{ "post", "put"}, Route = "submitPatientAccess")]
        [FromBody][Required] CloudRequest<SubmitPatientAccess> cloudRequest, HttpRequest req)
    {
        if (!_validator.ValidateAdminRequest(req).Result)
            return new UnauthorizedResult();
        try
        {
            var patientId = cloudRequest.Data.PatientId;
            var doctorId = cloudRequest.Data.DoctorId;
            //Get doctor and patient
            var allDoctors = _dataReader.GetAllDoctors();
            var patient = _dataReader.GetIdentity(patientId);
            var doctor = allDoctors.Find(x=>x.Id == cloudRequest.Data.DoctorId);
            //Check doctor does not already contain that patient
            if (doctor.Patients == null)
                doctor.Patients = new string[]
                {
                    patientId
                };
            else if (doctor.Patients.Any() && !doctor.Patients.Contains(patientId))
                doctor.Patients = doctor.Patients.Append(patientId).ToArray();
            else if (allDoctors.Where(x => x.Patients.Contains(patientId)).Count() > 0)
                return new BadRequestObjectResult("A different doctor already contains patient");
            else
                return new BadRequestObjectResult("Doctor already contains patient");
            //Check no other doctors do not already contain that patient

            await _dataIndexer.SubmitPatientAccess(cloudRequest.Data.DoctorId, doctor);
            return new AcceptedResult(new Uri($"Get/{doctorId}", UriKind.Relative), new { id = doctorId });
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    [FunctionName("CreateChat")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "writes" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    //[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CloudRequest<SubmitPatientAccess>), Description = "Parameter", Required = true)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.Accepted, contentType: "text/plain", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> CreateChat(
[HttpTrigger(AuthorizationLevel.Function, methods:new []{ "post", "put"}, Route = "createChat")]
         [FromBody] CloudRequest<CreateChat> cmd, HttpRequest req)
    {
        //var participantId = cloudRequest.Data.ParticipantId;

        var participantId = new Tuple<string, string>(cmd.Data.ParticipantId[0], cmd.Data.ParticipantId[1]);
        if (!_validator.ValidateChatRequest(req, participantId.Item1).Result)
            return new UnauthorizedResult();
        try
        {
            var chat = new Chat(participantId.Item1, participantId.Item2);
            await _dataIndexer.CreateChat(chat);
            return new AcceptedResult($"{ chat}", _dataReader.GetChat(chat.Id));
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    [FunctionName("SendMessage")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "writes" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    //[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CloudRequest<SubmitPatientAccess>), Description = "Parameter", Required = true)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.Accepted, contentType: "text/plain", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> SendMessage(
[HttpTrigger(AuthorizationLevel.Function, methods:new []{ "post", "put"}, Route = "sendMessage")]
         [FromBody] CloudRequest<SendMessage> cmd, HttpRequest req)
    {
        //var participantId = cloudRequest.Data.ParticipantId;

        if (!_validator.ValidateChatRequest(req, cmd.Data.SenderId).Result)
            return new UnauthorizedResult();
        try
        {
            var message = new Message(cmd.Data.SenderId, cmd.Data.ChatId, cmd.Data.Content, cmd.Time);
            await _dataIndexer.SendMessage(message);
            return new AcceptedResult($"{message}", message);
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }


    [FunctionName("GetPendingIdentities")]
    [OpenApiOperation(operationId: "GetPendingIdentities", tags: new[] { "reads" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "idOfPatient", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "the id of the requested price")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> GetPendingIdentities(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "identities/pending")]
        HttpRequest req)
    {
        if (!_validator.ValidateAdminRequest(req).Result)
            return new UnauthorizedResult();
        try
        {
            //Get data from database and return
            var identities = _dataReader.GetPendingIdentities();
            identities = identities.ConvertAll((x)=> {
                x.FullName = _firebaseManager.GetDisplayName(x.Id);
                return x;
            });
            return new ObjectResult(identities);
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    [FunctionName("SubmitPendingApproval")]
    [OpenApiOperation(operationId: "SubmitPendingApproval", tags: new[] { "reads" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "idOfPatient", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "the id of the requested price")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> SubmitPendingApproval(
[HttpTrigger(AuthorizationLevel.Function, "get", Route = "submitapproval/{id}")]
        HttpRequest req, string id)
    {
        if (!_validator.ValidateAdminRequest(req).Result)
            return new UnauthorizedResult();
        try
        {
            Role oldRole = _dataReader.GetIdentity(id).Role;
            Role newRole;
            if (oldRole == Role.PendingPatient) newRole = Role.Patient;
            else if (oldRole == Role.PendingDoctor) newRole = Role.Doctor;
            else return new BadRequestErrorMessageResult("Invalid user");

            await _dataIndexer.SubmitApproval(id, newRole);
            //Get data from database and return
            var identities = _dataReader.GetPendingIdentities();
            identities = identities.ConvertAll((x) => {
                x.FullName = _firebaseManager.GetDisplayName(x.Id);
                return x;
            });
            return new ObjectResult(identities);
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    [FunctionName("GetPatientData")]
    [OpenApiOperation(operationId: "GetPatientData", tags: new[] { "reads" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "idOfPatient", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "the id of the requested price")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> GetPatientData(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "patient/{id}")]
        HttpRequest req, string id)
    {
        if (!_validator.ValidatePatientDataRequest(req, id).Result)
            return new UnauthorizedResult();
        try
        {
            //Get data from database and return
            return new ObjectResult(null);
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    [FunctionName("GetAllPatients")]
    [OpenApiOperation(operationId: "GetAllPatients", tags: new[] { "reads" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> GetAllPatients(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "patients")]
        HttpRequest req)
    {
        if (!_validator.ValidateAdminRequest(req).Result)
            return new UnauthorizedResult();
        try
        {
            var users = _dataReader.GetAllPatients().ConvertAll(x =>
            {
                try
                {
                    var user = _firebaseManager.GetUser(x.Id);
                    return new Patient
                    {
                        Id = x.Id,
                        Email = user.Email,
                        FullName = user.DisplayName
                    };
                }
                catch(Exception ex)
                {
                    return new Patient
                    {
                        Id = x.Id,
                        Email = "Deleted User",
                        FullName = "Deleted User"
                    };
                }
            });
            return new ObjectResult(users);
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    [FunctionName("GetAllDoctors")]
    [OpenApiOperation(operationId: "GetAllDoctors", tags: new[] { "reads" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> GetAllDoctors(
[HttpTrigger(AuthorizationLevel.Function, "get", Route = "doctors")]
        HttpRequest req)
    {
        if (!_validator.ValidateAdminRequest(req).Result)
            return new UnauthorizedResult();
        try
        {
            var users = _dataReader.GetAllDoctors().ConvertAll(x=>
            {
                x.FullName = _firebaseManager.GetDisplayName(x.Id);
                return x;
            });
            return new ObjectResult(users);
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    //This endpoint is for when someone wants to get their identity so it can be used on the frontend. As we haven't issued them a token yet, the way we validate a request is with a firebase token
    [FunctionName("GetIdentity")]
    [OpenApiOperation(operationId: "GetIdentity", tags: new[] { "reads" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "idOfPatient", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "the id of the requested price")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> GetIdentity(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getidentity/{id}")]
        HttpRequest req, string id)
    {
        var token = TokenExtractor.TryGet(req);
        if (_firebaseManager.VerifyIdToken(token).IsNullOrWhiteSpace())
            return new UnauthorizedResult();
        try
        {
            Identity identity = _dataReader.GetIdentity(id);
            if (identity == null)
                throw new Exception("No identity found");    
            var issuer = new TokenIssuer(_settings.JWTSecret).IssueTokenForUser(identity);
            identity.APIToken = issuer;
            identity.FullName = _firebaseManager.GetDisplayName(identity.Id);
            return new ObjectResult(identity);
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    [FunctionName("GetChats")]
    [OpenApiOperation(operationId: "GetChats", tags: new[] { "reads" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "the id of the requested price")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> GetChats(
[HttpTrigger(AuthorizationLevel.Function, "get", Route = "chats/{id}")]
        HttpRequest req, string id)
    {
        if (await _validator.ValidateChatRequest(req, id) == false)
            return new UnauthorizedResult();
        try
        {
            var chats = _dataReader.GetChats(id).ConvertAll((Converter<Chat, ChatView>)(x =>
            {
                return new ChatView
                {
                    ChatId = x.Id,
                    ParticipantId = x.ParticipantId,
                    Name = _firebaseManager.GetDisplayName(x.ParticipantId.Item1 == id ? x.ParticipantId.Item2 : x.ParticipantId.Item1)
                };
            }));
            return new ObjectResult(chats);
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    [FunctionName("GetMessages")]
    [OpenApiOperation(operationId: "GetMessages", tags: new[] { "reads" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "the id of the requested price")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> GetMessages(
[HttpTrigger(AuthorizationLevel.Function, "get", Route = "messages/{userId}")]
        HttpRequest req, string userId)
    {
        if (await _validator.ValidateChatRequest(req, userId) == false)
            return new UnauthorizedResult();
        try
        {
            var messages = new List<Message>();
            var chats = _dataReader.GetChats(userId);
            chats.ForEach((x =>
            {
                try
                {
                    var msg = _dataReader.GetMessages((string)x.Id);
                    msg.ForEach(y => messages.Add(y));
                } catch(Exception ex)
                {
                    Console.WriteLine("No messages for chat");
                }
            }));
            return new ObjectResult(messages);
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    [FunctionName("GetDoctorForPatient")]
    [OpenApiOperation(operationId: "GetDoctorForPatient", tags: new[] { "reads" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "patientId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "the id of the requested price")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> GetDoctorForPatient(
[HttpTrigger(AuthorizationLevel.Function, "get", Route = "doctors/{patientId}")]
        HttpRequest req, string patientId)
    {
        //if (await _validator.ValidateChatRequest(req, id) == false)
        //    return new UnauthorizedResult();
        try
        {
            
            return new ObjectResult(_dataReader.GetDoctorForPatient(patientId));
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    [FunctionName("DeleteAccount")]
    [OpenApiOperation(operationId: "DeleteAccount", tags: new[] { "reads" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "the id of the requested price")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> DeleteAccount(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "deleteAccount/{userId}")]
        HttpRequest req, string userId)
    {
        if (await _validator.ValidateDeleteRequest(req, userId) == false)
            return new UnauthorizedResult();
        try
        {
            if(_dataIndexer.DeleteIdentity(userId).IsAcknowledged != true)
                throw new Exception("Cannot delete identity");
            await _firebaseManager.DeleteUser(userId);
            return new ObjectResult($"User {userId} is deleted");
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }

    [FunctionName("ExportAccount")]
    [OpenApiOperation(operationId: "ExportAccount", tags: new[] { "reads" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "the id of the requested price")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/pdf", bodyType: typeof(byte[]), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> ExportAccount(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "exportAccount/{userId}")]
        HttpRequest req, string userId)
    {
        if (await _validator.ValidateDeleteRequest(req, userId) == false)
            return new UnauthorizedResult();
        try
        {
            var user = _firebaseManager.GetUser(userId);
            var identity = _dataReader.GetIdentity(userId);

            return new ObjectResult(_writer.GeneratePDFBytes(identity, user));
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }
}

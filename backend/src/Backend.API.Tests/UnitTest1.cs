using Backend.API.Tests.Fakes;
using Backend.Commands;
using Backend.Database;
using Backend.Interfaces;
using Backend.Model;
using Backend.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;

namespace Backend.API.Tests
{
    public class Tests
    {
        private Servicing _servicing;
        private IValidator _validator;
        private IConfiguration _configuration;
        private IFirebaseManager _firebaseManager;
        [SetUp]
        public void Setup()
        {
            // Initialize the fake Firebase manager
            _firebaseManager = new FirebaseManagerFake();

            // In-memory configuration for testing purposes
            var myConfiguration = new Dictionary<string, string>
            {
                {"dontcare", "dontcare" },
                {"MongoConnectionString", "mongodb+srv://Hospitaladmin:<password>@hospitalcluster.vovnion.mongodb.net/?retryWrites=true&w=majority"},
                {"JWTSecret", "foo" },
            };

            // Build the configuration object
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            // Initialize validator and servicing with the configurations
            _validator = new Validator(_configuration, _firebaseManager);
            _servicing = new Servicing(_firebaseManager, _validator, _configuration, null);
        }

        [Test]
        public void Test_SignUp_Process_When_Id_On_FirebaseToken_Does_Not_Match_json()
        {
            // Load a fake request from a JSON file
            var json = Helpers.LoadJson("./FakeRequests/CreateIdentity-1234.json");
            var req = Helpers.BuildHttpRequest(json);

            // Deserialize the request to a cloud request object
            var cloudRequest = JsonConvert.DeserializeObject<CloudRequest<CreateIdentity>>(json);

            // Mock the Firebase manager to return a specific ID
            var mockFirebaseManager = new Mock<IFirebaseManager>();
            mockFirebaseManager.Setup(x => x.VerifyIdToken(It.IsAny<string>())).Returns("123");

            // Initialize services with mocked dependencies
            var validator = new Validator(_configuration, mockFirebaseManager.Object);
            var servicing = new Servicing(mockFirebaseManager.Object, validator, _configuration, null);

            // Test the identity creation process
            var res = servicing.CreateIdentity(cloudRequest, req).Result;

            // Assert the result to ensure unauthorized response
            Assert.IsInstanceOf<UnauthorizedResult>(res);
        }

        [Test]
        public void Test_SignUp_Process_When_Id_On_FirebaseToken_Does_Match_json()
        {
            // Load a fake request from a JSON file
            var json = Helpers.LoadJson("./FakeRequests/CreateIdentity-123.json");
            var req = Helpers.BuildHttpRequest(json);
            var cloudRequest = JsonConvert.DeserializeObject<CloudRequest<CreateIdentity>>(json);

            // Mock the Firebase manager and database indexer with specific behaviors
            var mockFirebaseManager = new Mock<IFirebaseManager>();
            mockFirebaseManager.Setup(x => x.VerifyIdToken(It.IsAny<string>())).Returns("123");
            var mockDbIndexer = new Mock<IDataIndexer>();
            mockDbIndexer.Setup(x => x.IndexIdentity(It.IsAny<Identity>())).Returns(Task.CompletedTask);

            // Initialize services with mocked dependencies
            var validator = new Validator(_configuration, mockFirebaseManager.Object);
            var servicing = new Servicing(mockFirebaseManager.Object, validator, _configuration, mockDbIndexer.Object, null);

            // Test the identity creation process
            var res = servicing.CreateIdentity(cloudRequest, req).Result;

            // Assert the result to ensure the request is accepted
            Assert.IsInstanceOf<AcceptedResult>(res);
        }



        [Test]
        public void Test_SimulateLogin_For_User_123_DoctorRole()
        {
            // Setting up the user ID and role for the simulation
            var userId = "123";
            var role = Role.Doctor;

            // Building the HTTP request with a JWT token header
            var req = Helpers.BuildHttpRequest();
            req.Headers.Add("JWT", Helpers.BuildToken(userId, role));

            // Mocking the Firebase Manager to return the simulated user ID
            var mockFirebaseManager = new Mock<IFirebaseManager>();
            mockFirebaseManager.Setup(x => x.VerifyIdToken(It.IsAny<string>())).Returns(userId);

            // Mocking the FileWriter
            var mockFileWriter = new Mock<IFileWriter>();

            // Mocking the Database Indexer to return a completed task when indexing an identity
            var mockDbIndexer = new Mock<IDataIndexer>();
            mockDbIndexer.Setup(x => x.IndexIdentity(It.IsAny<Identity>())).Returns(Task.CompletedTask);

            // Mocking the Data Reader to return the simulated user identity
            var mockDataReader = new Mock<IDataReader>();
            var mockIdentity = new Identity
            {
                Id = userId,
                Role = role
            };
            mockDataReader.Setup(x => x.GetIdentity(It.IsAny<string>())).Returns(mockIdentity);

            // Setting up the validator and servicing objects with the above mocks
            var validator = new Validator(_configuration, mockFirebaseManager.Object);
            var servicing = new Servicing(mockFirebaseManager.Object, validator, _configuration, mockDbIndexer.Object, mockDataReader.Object, mockFileWriter.Object);

            // Calling the GetIdentity method and storing the result
            var res = servicing.GetIdentity(req, userId).Result;

            // Assertions to ensure that the login simulation was successful
            Assert.IsInstanceOf<ObjectResult>(res);
            var identity = (Identity)((ObjectResult)res).Value;
            Assert.IsInstanceOf<Identity>(identity);
        }


        [Test]
        public void Test_SimulateLogin_For_InvalidUser()
        {
            // Setting up an invalid user ID and role for the simulation
            var invalidUserId = "invalid_456";
            var role = Role.Doctor;

            // Building the HTTP request with a JWT token header
            var req = Helpers.BuildHttpRequest();
            req.Headers.Add("JWT", Helpers.BuildToken(invalidUserId, role));

            // Mocking the Firebase Manager to return null
            var mockFirebaseManager = new Mock<IFirebaseManager>();
            mockFirebaseManager.Setup(x => x.VerifyIdToken(It.IsAny<string>())).Returns((string)null); // Token verification fails

            // Mocking the Data Reader to return null
            var mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.GetIdentity(It.IsAny<string>())).Returns((Identity)null);

            // Mocking the FileWriter
            var mockFileWriter = new Mock<IFileWriter>();

            // Setting up the validator and servicing objects with the above mocks 
            var validator = new Validator(_configuration, mockFirebaseManager.Object);
            var servicing = new Servicing(mockFirebaseManager.Object, validator, _configuration, null, mockFileWriter.Object);

            // Calling the GetIdentity method and storing the result
            var res = servicing.GetIdentity(req, invalidUserId).Result;

            // Assertion to ensure that the login attempt for an invalid user returns an Unauthorized result
            Assert.IsInstanceOf<UnauthorizedResult>(res);
        }




        [Test]
        public void Test_SendMessage_With_ValidId()
        {
            // Setup variables for simulation
            var userId = "123";
            var chatId = "chat_001";
            var content = "Hello, this is a test message.";

            // Mocking the Firebase Manager to verify a valid user token
            var mockFirebaseManager = new Mock<IFirebaseManager>();
            mockFirebaseManager.Setup(x => x.VerifyIdToken(It.IsAny<string>())).Returns(userId);

            // Mocking the validator to confirm that the chat request is valid
            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(v => v.ValidateChatRequest(It.IsAny<HttpRequest>(), userId)).ReturnsAsync(true);

            // Mocking other dependencies
            var mockDataIndexer = new Mock<IDataIndexer>();
            var mockDataReader = new Mock<IDataReader>();
            var mockFileWriter = new Mock<IFileWriter>();

            // Initializing the servicing object with the mocked dependencies
            var servicing = new Servicing(mockFirebaseManager.Object, mockValidator.Object, _configuration, mockDataIndexer.Object, mockDataReader.Object, mockFileWriter.Object);

            // Creating a command to simulate the sending of a message
            var cmd = new CloudRequest<SendMessage>
            {
                Data = new SendMessage
                {
                    SenderId = userId,
                    ChatId = chatId,
                    Content = content,
                }
            };
            var req = new Mock<HttpRequest>();

            // Calling the SendMessage method and storing the result
            var result = servicing.SendMessage(cmd, req.Object).Result;

            // Assertion to ensure that the message sending is successful for a valid user ID
            Assert.IsInstanceOf<AcceptedResult>(result);
        }



        [Test]
        public void Test_SendMessage_With_InvalidId()
        {
            // Setup variables for simulation
            var userId = "invalid123";
            var chatId = "chat_001";
            var content = "Hello, this is a test message.";

            // Mocking the Firebase Manager to return null for an invalid user token
            var mockFirebaseManager = new Mock<IFirebaseManager>();
            mockFirebaseManager.Setup(x => x.VerifyIdToken(It.IsAny<string>())).Returns((string)null);

            // Mocking the validator to confirm that the chat request is invalid
            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(v => v.ValidateChatRequest(It.IsAny<HttpRequest>(), userId)).ReturnsAsync(false);

            // Mocking other dependencies
            var mockDataIndexer = new Mock<IDataIndexer>();
            var mockDataReader = new Mock<IDataReader>();
            var mockFileWriter = new Mock<IFileWriter>();

            // Initializing the servicing object with the mocked dependencies
            var servicing = new Servicing(mockFirebaseManager.Object, mockValidator.Object, _configuration, mockDataIndexer.Object, mockDataReader.Object, mockFileWriter.Object);

            // Creating a command to simulate the sending of a message
            var cmd = new CloudRequest<SendMessage>
            {
                Data = new SendMessage
                {
                    SenderId = userId,
                    ChatId = chatId,
                    Content = content,
                }
            };
            var req = new Mock<HttpRequest>();

            // Calling the SendMessage method and storing the result
            var result = servicing.SendMessage(cmd, req.Object).Result;

            // Assertion to ensure that the message sending attempt returns an Unauthorized result for an invalid user ID
            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }


        [Test]
        public void Test_FetchChatsForUser_With_ValidToken()
        {
            // Setup variables
            var userId = "123";
            var role = Role.Doctor;

            // Building the HTTP request for login simulation
            var req = Helpers.BuildHttpRequest();
            req.Headers.Add("JWT", Helpers.BuildToken(userId, role));

            // Mocking the Firebase Manager to verify the user token
            var mockFirebaseManager = new Mock<IFirebaseManager>();
            mockFirebaseManager.Setup(x => x.VerifyIdToken(It.IsAny<string>())).Returns(userId);

            // Mocking other dependencies
            var mockDbIndexer = new Mock<IDataIndexer>();
            mockDbIndexer.Setup(x => x.IndexIdentity(It.IsAny<Identity>())).Returns(Task.CompletedTask);

            var mockDbReader = new Mock<IDataReader>();
            mockDbReader.Setup(x => x.GetIdentity(userId)).Returns(new Identity
            {
                Role = role,
                Id = userId
            });

            mockDbReader.Setup(dr => dr.GetChats(It.IsAny<string>())).Returns(new List<Chat>());  // Return a list of dummy chats or whatever fits your scenario.

            //Mock validate request so that ID is valid
            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(v => v.ValidateChatRequest(It.IsAny<HttpRequest>(), It.IsAny<string>())).ReturnsAsync(true);

            // Initializing the service with mocked dependencies

            var servicing = new Servicing(mockFirebaseManager.Object, mockValidator.Object, _configuration, mockDbIndexer.Object, mockDbReader.Object, null);

            // Calling the GetIdentity method to simulate user login
            var loginRes = servicing.GetIdentity(req, userId).Result;
            var identity = (Identity)((ObjectResult)loginRes).Value;

            // Building the HTTP request to fetch chats using the token obtained from the simulated login
            var getRequest = Helpers.BuildHttpRequest();
            getRequest.Headers.Add("JWT", identity.APIToken);

            // Calling the GetChats method to fetch chats
            var res = servicing.GetChats(getRequest, "123").Result;

            // Assertion to ensure the expected response for fetching chats with a valid token
            Assert.That(res, Is.TypeOf<ObjectResult>());
        }

        [Test]
        public void Test_FetchChatsForUser_With_InvalidToken()
        {
            // Setup variables
            var userId = "123";
            var invalidToken = "InvalidTokenXYZ";

            // Building the HTTP request with an invalid token
            var req = Helpers.BuildHttpRequest();
            req.Headers.Add("JWT", invalidToken);

            // Mocking the Firebase Manager to return null indicating the token is invalid
            var mockFirebaseManager = new Mock<IFirebaseManager>();
            mockFirebaseManager.Setup(x => x.VerifyIdToken(It.IsAny<string>())).Returns((string)null);

            // Mocking other dependencies
            var mockDbIndexer = new Mock<IDataIndexer>();
            mockDbIndexer.Setup(x => x.IndexIdentity(It.IsAny<Identity>())).Returns(Task.CompletedTask);

            var mockDbReader = new Mock<IDataReader>();

            // Initializing the service with mocked dependencies
            var validator = new Validator(_configuration, mockFirebaseManager.Object);
            var servicing = new Servicing(mockFirebaseManager.Object, validator, _configuration, mockDbIndexer.Object, mockDbReader.Object, null);

            // Building the HTTP request to fetch chats using the invalid token
            var getRequest = Helpers.BuildHttpRequest();
            getRequest.Headers.Add("JWT", invalidToken);

            // Calling the GetChats method to attempt fetching chats with the invalid token
            var res = servicing.GetChats(getRequest, userId).Result;

            // Assertion to ensure the expected Unauthorized response for the invalid token
            Assert.That(res, Is.TypeOf<UnauthorizedResult>());
        }
    }
}
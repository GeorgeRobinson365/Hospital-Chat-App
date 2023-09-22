
using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;
using Newtonsoft.Json;
using Backend.Model;


namespace Backend.API.Tests
{
    
    internal class Helpers
    {
        // Build an HTTP request
        public static HttpRequest BuildHttpRequest(string body = "")
        {
            // Serialize the provided body to JSON
            var json = JsonConvert.SerializeObject(body);

            // Convert the serialized JSON string to a memory stream
            var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));

            // Create a new default HTTP context
            var context = new DefaultHttpContext();

            // Retrieve the request from the context
            var request = context.Request;

            // Assign the memory stream to the request body and set its content type to JSON
            request.Body = memoryStream;
            request.ContentType = "application/json";

            // Return the constructed request
            return request;
        }

        // Overload method to build a token based on user ID and role
        public static string BuildToken(string id, Role role)
        {
            return BuildToken(
                new Dictionary<string, object>()
                {
                    {"uid", id },
                    {"roles", role }
                }
            );
        }

        // Method to build a JWT token based on the provided claims dictionary
        public static string BuildToken(IDictionary<string, object> claims)
        {
            
            var secret = "foofoofoofoofoo!!!@@@111";

            // Create the necessary JWT components for encoding
            var algorithm = new HMACSHA256Algorithm();
            var serializer = new JsonNetSerializer();
            var base64Encoder = new JwtBase64UrlEncoder();
            var jwtEncoder = new JwtEncoder(algorithm, serializer, base64Encoder);

            // Encode and return the JWT token
            return jwtEncoder.Encode(claims, secret);
        }

        // Add the JWT token to the HTTP request header
        public static HttpRequest AddTokenToHttpRequest(HttpRequest request, string token)
        {
            request.Headers.Add("JWT", token);
            return request;
        }

        // Load a JSON file from the specified path
        public static string LoadJson(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return json;
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Model
{
    public class Settings
    {
        public string JWTSecret { get; set; }
        public FirebaseConnection FirebaseConnection { get; set; }
        public string DatabaseConnectionString { get; set; }
        public string MongoConnectionString { get; set; }
    }
    public class FirebaseConnection
    {

        public string type { get; set; } //= Environment.GetEnvironmentVariable("FirebaseConnection__type");
        public string project_id { get; set; }// = Environment.GetEnvironmentVariable("FirebaseConnection__project_id");
        public string private_key_id { get; set; }// = Environment.GetEnvironmentVariable("FirebaseConnection__private_key_id");
        public string private_key { get; set; }// = Environment.GetEnvironmentVariable("FirebaseConnection__private_key");
        public string client_email { get; set; }// = Environment.GetEnvironmentVariable("FirebaseConnection__client_email");
        public string client_id { get; set; } //= Environment.GetEnvironmentVariable("FirebaseConnection__client_id");
        public string auth_uri { get; set; } //= Environment.GetEnvironmentVariable("FirebaseConnection__auth_uri");
        public string token_uri { get; set; } //= Environment.GetEnvironmentVariable("FirebaseConnection__token_uri");
        public string auth_provider_x509_cert_url { get; set; }// = Environment.GetEnvironmentVariable("FirebaseConnection__auth_provider_x509_cert_url");
        public string client_x509_cert_url { get; set; }// = Environment.GetEnvironmentVariable("FirebaseConnection__client_x509_cert_url");
    }
}

﻿namespace CosmosDb.Model.Configuration
{
    public class CosmosDbSettings
    {
        public string EndpointUri { get; set; } = "https://localhost:8081";
        public string PrimaryKey { get; set; } = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        public bool IgnoreSslServerCertificateValidation = false;
    }
}
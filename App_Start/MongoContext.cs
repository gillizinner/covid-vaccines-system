using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidVaccinationSystem.App_Start
{
    public class MongoContext
    {
        MongoClient _client;
        MongoServer _server;
        public MongoDatabase _database;


        public MongoContext()        //constructor   
        {

            var MongoDatabaseName = "CovidVaccinesSystem";   
            var MongoPort = 27017;
            var MongoHost = "localhost";

            // Creating MongoClientSettings  
            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(MongoHost, Convert.ToInt32(MongoPort))
            };
            _client = new MongoClient(settings);
            _server = _client.GetServer();
            _database = _server.GetDatabase(MongoDatabaseName);
        }
    }
}


using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using RunTracker.Model;

namespace RunTracker.Services
{
    class DatabaseService
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public DatabaseService(IConfiguration configuration)
        {
            // Hämta connection string från User Secrets via IConfiguration
            var connectionString = configuration.GetConnectionString("MongoDb");
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("SofiaNordström");
        }

        public IMongoCollection<RunningSession> RunningSessions => _database.GetCollection<RunningSession>("RunningSessions");
        public IMongoCollection<RunType> RunTypes => _database.GetCollection<RunType>("RunTypes");

        public IMongoDatabase Database => _database;
    }
}

using MongoDB.Bson;
using MongoDB.Driver;
using RunTracker.Model;

namespace RunTracker.Repository
{
    public class RunningSessionRepository : IRunningSessionRepository
    {

        private readonly IMongoCollection<RunningSession> _runningSessionsCollection;

        public RunningSessionRepository(IMongoDatabase database)
        {
            _runningSessionsCollection = database.GetCollection<RunningSession>("RunningSessions");
        }

        public async Task<List<RunningSession>> GetAllAsync()
        {
            return await _runningSessionsCollection.Find(r => true).ToListAsync();
        }

        public async Task AddAsync(RunningSession runningSession)
        {
            await _runningSessionsCollection.InsertOneAsync(runningSession);
        }

        public async Task UpdateAsync(RunningSession runningSession)
        {
            var filter = Builders<RunningSession>.Filter.Eq(r => r.Id, runningSession.Id);
            await _runningSessionsCollection.ReplaceOneAsync(filter, runningSession);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            var filter = Builders<RunningSession>.Filter.Eq(r => r.Id, id);
            await _runningSessionsCollection.DeleteOneAsync(filter);
        }
    }
}

using MongoDB.Bson;
using MongoDB.Driver;
using RunTracker.Model;

namespace RunTracker.Repository
{
    internal class RunTypeRepository : IRunTypeRepository
    {
        private readonly IMongoCollection<RunType> _runTypesCollection;

        public RunTypeRepository(IMongoDatabase database)
        {
            _runTypesCollection = database.GetCollection<RunType>("RunTypes");
        }

        public async Task<List<RunType>> GetAllAsync()
        {
            return await _runTypesCollection.Find(r => true).ToListAsync();
        }

        public async Task AddAsync(RunType runType)
        {
            await _runTypesCollection.InsertOneAsync(runType);
        }

        public async Task UpdateAsync(RunType runType)
        {
            var filter = Builders<RunType>.Filter.Eq(r => r.Id, runType.Id);
            await _runTypesCollection.ReplaceOneAsync(filter, runType);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            var filter = Builders<RunType>.Filter.Eq(r => r.Id, id);
            await _runTypesCollection.DeleteOneAsync(filter);
        }
    }
}

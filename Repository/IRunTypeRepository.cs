using MongoDB.Bson;
using RunTracker.Model;

namespace RunTracker.Repository
{
    public interface IRunTypeRepository
    {
        Task<List<RunType>> GetAllAsync();
        Task AddAsync(RunType runType);
        Task UpdateAsync(RunType runType);
        Task DeleteAsync(ObjectId id);
    }
}

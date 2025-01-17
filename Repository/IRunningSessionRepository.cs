using MongoDB.Bson;
using RunTracker.Model;

namespace RunTracker.Repository
{
    public interface IRunningSessionRepository
    {
        Task<List<RunningSession>> GetAllAsync();
        Task AddAsync(RunningSession runningSession);
        Task UpdateAsync(RunningSession runningSession);
        Task DeleteAsync(ObjectId id);
    }
}

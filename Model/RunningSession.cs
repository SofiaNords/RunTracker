using MongoDB.Bson;

namespace RunTracker.Model
{
    public class RunningSession
    {
        public ObjectId Id { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Distance { get; set; }
        public TimeSpan? Time { get; set; }
        public RunType? RunType { get; set; }
    }
}

using MongoDB.Bson;

namespace RunTracker.Model
{
    public class RunningSession
    {
        public ObjectId Id { get; set; }
        public DateTime Date { get; set; }
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }
        public string RunType { get; set; }
    }
}

using MongoDB.Bson;

namespace RunTracker.Model
{
    public class RunType
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}

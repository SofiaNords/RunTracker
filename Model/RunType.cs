using MongoDB.Bson;

namespace RunTracker.Model
{
    public class RunType
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is RunType other)
            {
                return Id == other.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

}

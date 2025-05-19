using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CourseManagement.Models
{
    public class Image
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("courseId")]
        public string CourseId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("data")]
        public byte[] Data { get; set; }
    }
}
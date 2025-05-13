using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public class Course
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("instructor")]
        public string Instructor { get; set; }

        [BsonElement("lessons")]
        public List<Lesson> Lessons { get; set; }

        [BsonElement("grades")]
        public List<Grade> Grades { get; set; }
    }

    public class Lesson
    {
        [BsonElement("lessonId")]
        public int LessonId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("duration")]
        public int Duration { get; set; }

        [BsonElement("materials")]
        public List<Material> Materials { get; set; }
    }

    public class Material
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("url")]
        public string Url { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }
    }

    public class Grade
    {
        [BsonElement("studentId")]
        public string StudentId { get; set; }

        [BsonElement("grade")]
        public double GradeValue { get; set; }

        [BsonElement("comment")]
        public string Comment { get; set; }
    }
}
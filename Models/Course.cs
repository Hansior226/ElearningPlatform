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

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("instructor")]
        public string Instructor { get; set; }

        [BsonElement("lessons")]
        public List<Lesson> Lessons { get; set; } = new List<Lesson>(); // Inicjalizacja jako pusta lista

        [BsonElement("grades")]
        public List<Grade> Grades { get; set; } = new List<Grade>(); // Inicjalizacja jako pusta lista

        // Pole do przechowywania listy materiałów (zgodne z wcześniejszym kodem widoku)
        [BsonElement("materials")]
        public List<Material> Materials { get; set; } = new List<Material>(); // Inicjalizacja jako pusta lista
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
        public List<Material> Materials { get; set; } = new List<Material>(); // Inicjalizacja jako pusta lista
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
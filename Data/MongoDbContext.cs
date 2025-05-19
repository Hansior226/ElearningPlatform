using MongoDB.Driver;
using CourseManagement.Models;

namespace CourseManagement.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("CourseDB");
        }

        public IMongoCollection<Course> Courses => _database.GetCollection<Course>("courses");
        public IMongoCollection<Image> Images => _database.GetCollection<Image>("images");
    }
}
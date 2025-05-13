using CourseManagement.Data;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CourseManagement.Controllers
{
    public class CoursesController : Controller
    {
        private readonly MongoDbContext _context;

        public CoursesController()
        {
            _context = new MongoDbContext();
        }

        public async Task<ActionResult> Index()
        {
            var courses = await _context.Courses.Find(_ => true).ToListAsync();
            return View(courses);
        }

        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var course = await _context.Courses
                .Find(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (course == null)
            {
                return HttpNotFound();
            }

            return View(course);
        }
    }
}
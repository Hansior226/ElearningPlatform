using CourseManagement.Data;
using CourseManagement.Models;
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

        // GET: Courses
        public async Task<ActionResult> Index()
        {
            var courses = await _context.Courses.Find(_ => true).ToListAsync();
            return View(courses);
        }

        // GET: Courses/Details/5
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

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                await _context.Courses.InsertOneAsync(course);
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var course = await _context.Courses.Find(c => c.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return HttpNotFound();
            }

            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Course course)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                var existingCourse = await _context.Courses.Find(c => c.Id == id).FirstOrDefaultAsync();
                if (existingCourse == null)
                {
                    return HttpNotFound();
                }

                existingCourse.Name = course.Name; // Zmieniono Title na Name
                existingCourse.Description = course.Description;
                existingCourse.Instructor = course.Instructor;
                existingCourse.Lessons = course.Lessons;
                existingCourse.Grades = course.Grades;
                existingCourse.Materials = course.Materials;

                await _context.Courses.ReplaceOneAsync(c => c.Id == id, existingCourse);
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var course = await _context.Courses.Find(c => c.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return HttpNotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            await _context.Courses.DeleteOneAsync(c => c.Id == id);
            return RedirectToAction("Index");
        }
    }
}
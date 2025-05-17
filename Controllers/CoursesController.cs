using CourseManagement.Data;
using CourseManagement.Models;
using MongoDB.Driver;
using System.Collections.Generic;
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
                course.Lessons = new List<Lesson>();
                course.Grades = new List<Grade>();
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

            var course = await _context.Courses
                .Find(c => c.Id == id)
                .FirstOrDefaultAsync();

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
            if (ModelState.IsValid)
            {
                var filter = Builders<Course>.Filter.Eq(c => c.Id, id);
                var update = Builders<Course>.Update
                    .Set(c => c.Title, course.Title)
                    .Set(c => c.Description, course.Description)
                    .Set(c => c.Instructor, course.Instructor);

                await _context.Courses.UpdateOneAsync(filter, update);
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

            var course = await _context.Courses
                .Find(c => c.Id == id)
                .FirstOrDefaultAsync();

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
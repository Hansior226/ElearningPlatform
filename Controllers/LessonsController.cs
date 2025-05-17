using CourseManagement.Data;
using CourseManagement.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CourseManagement.Controllers
{
    public class LessonsController : Controller
    {
        private readonly MongoDbContext _context;

        public LessonsController()
        {
            _context = new MongoDbContext();
        }

        // GET: Lessons
        public async Task<ActionResult> Index(string courseId)
        {
            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null) return HttpNotFound();
            ViewBag.CourseId = courseId;
            return View(course.Lessons);
        }

        // GET: Lessons/Create
        public ActionResult Create(string courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        // POST: Lessons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string courseId, Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
                if (course == null) return HttpNotFound();

                lesson.Materials = new System.Collections.Generic.List<Material>();
                lesson.LessonId = course.Lessons.Count + 1;
                course.Lessons.Add(lesson);

                await _context.Courses.ReplaceOneAsync(c => c.Id == courseId, course);
                return RedirectToAction("Index", new { courseId });
            }
            ViewBag.CourseId = courseId;
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public async Task<ActionResult> Edit(string courseId, int lessonId)
        {
            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null) return HttpNotFound();

            var lesson = course.Lessons.Find(l => l.LessonId == lessonId);
            if (lesson == null) return HttpNotFound();

            ViewBag.CourseId = courseId;
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string courseId, Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
                if (course == null) return HttpNotFound();

                var existingLesson = course.Lessons.Find(l => l.LessonId == lesson.LessonId);
                if (existingLesson == null) return HttpNotFound();

                existingLesson.Title = lesson.Title;
                existingLesson.Duration = lesson.Duration;

                await _context.Courses.ReplaceOneAsync(c => c.Id == courseId, course);
                return RedirectToAction("Index", new { courseId });
            }
            ViewBag.CourseId = courseId;
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public async Task<ActionResult> Delete(string courseId, int lessonId)
        {
            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null) return HttpNotFound();

            var lesson = course.Lessons.Find(l => l.LessonId == lessonId);
            if (lesson == null) return HttpNotFound();

            ViewBag.CourseId = courseId;
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string courseId, int lessonId)
        {
            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null) return HttpNotFound();

            course.Lessons.RemoveAll(l => l.LessonId == lessonId);
            await _context.Courses.ReplaceOneAsync(c => c.Id == courseId, course);
            return RedirectToAction("Index", new { courseId });
        }
    }
}
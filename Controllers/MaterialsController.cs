using CourseManagement.Data;
using CourseManagement.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CourseManagement.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly MongoDbContext _context;

        public MaterialsController()
        {
            _context = new MongoDbContext();
        }

        // GET: Materials
        public async Task<ActionResult> Index(string courseId, int lessonId)
        {
            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null) return HttpNotFound();

            var lesson = course.Lessons.Find(l => l.LessonId == lessonId);
            if (lesson == null) return HttpNotFound();

            ViewBag.CourseId = courseId;
            ViewBag.LessonId = lessonId;
            return View(lesson.Materials);
        }

        // GET: Materials/Create
        public ActionResult Create(string courseId, int lessonId)
        {
            ViewBag.CourseId = courseId;
            ViewBag.LessonId = lessonId;
            return View();
        }

        // POST: Materials/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string courseId, int lessonId, Material material)
        {
            if (ModelState.IsValid)
            {
                var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
                if (course == null) return HttpNotFound();

                var lesson = course.Lessons.Find(l => l.LessonId == lessonId);
                if (lesson == null) return HttpNotFound();

                lesson.Materials.Add(material);
                await _context.Courses.ReplaceOneAsync(c => c.Id == courseId, course);
                return RedirectToAction("Index", new { courseId, lessonId });
            }
            ViewBag.CourseId = courseId;
            ViewBag.LessonId = lessonId;
            return View(material);
        }

        // GET: Materials/Edit/5
        public async Task<ActionResult> Edit(string courseId, int lessonId, int materialIndex)
        {
            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null) return HttpNotFound();

            var lesson = course.Lessons.Find(l => l.LessonId == lessonId);
            if (lesson == null || materialIndex >= lesson.Materials.Count) return HttpNotFound();

            ViewBag.CourseId = courseId;
            ViewBag.LessonId = lessonId;
            ViewBag.MaterialIndex = materialIndex;
            return View(lesson.Materials[materialIndex]);
        }

        // POST: Materials/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string courseId, int lessonId, int materialIndex, Material material)
        {
            if (ModelState.IsValid)
            {
                var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
                if (course == null) return HttpNotFound();

                var lesson = course.Lessons.Find(l => l.LessonId == lessonId);
                if (lesson == null || materialIndex >= lesson.Materials.Count) return HttpNotFound();

                lesson.Materials[materialIndex] = material;
                await _context.Courses.ReplaceOneAsync(c => c.Id == courseId, course);
                return RedirectToAction("Index", new { courseId, lessonId });
            }
            ViewBag.CourseId = courseId;
            ViewBag.LessonId = lessonId;
            ViewBag.MaterialIndex = materialIndex;
            return View(material);
        }

        // GET: Materials/Delete/5
        public async Task<ActionResult> Delete(string courseId, int lessonId, int materialIndex)
        {
            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null) return HttpNotFound();

            var lesson = course.Lessons.Find(l => l.LessonId == lessonId);
            if (lesson == null || materialIndex >= lesson.Materials.Count) return HttpNotFound();

            ViewBag.CourseId = courseId;
            ViewBag.LessonId = lessonId;
            ViewBag.MaterialIndex = materialIndex;
            return View(lesson.Materials[materialIndex]);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string courseId, int lessonId, int materialIndex)
        {
            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null) return HttpNotFound();

            var lesson = course.Lessons.Find(l => l.LessonId == lessonId);
            if (lesson == null || materialIndex >= lesson.Materials.Count) return HttpNotFound();

            lesson.Materials.RemoveAt(materialIndex);
            await _context.Courses.ReplaceOneAsync(c => c.Id == courseId, course);
            return RedirectToAction("Index", new { courseId, lessonId });
        }
    }
}
using CourseManagement.Data;
using CourseManagement.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CourseManagement.Controllers
{
    public class GradesController : Controller
    {
        private readonly MongoDbContext _context;

        public GradesController()
        {
            _context = new MongoDbContext();
        }

        // GET: Grades
        public async Task<ActionResult> Index(string courseId)
        {
            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null) return HttpNotFound();

            ViewBag.CourseId = courseId;
            return View(course.Grades);
        }

        // GET: Grades/Create
        public ActionResult Create(string courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        // POST: Grades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string courseId, Grade grade)
        {
            if (ModelState.IsValid)
            {
                var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
                if (course == null) return HttpNotFound();

                course.Grades.Add(grade);
                await _context.Courses.ReplaceOneAsync(c => c.Id == courseId, course);
                return RedirectToAction("Index", new { courseId });
            }
            ViewBag.CourseId = courseId;
            return View(grade);
        }

        // GET: Grades/Edit/5
        public async Task<ActionResult> Edit(string courseId, string studentId)
        {
            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null) return HttpNotFound();

            var grade = course.Grades.Find(g => g.StudentId == studentId);
            if (grade == null) return HttpNotFound();

            ViewBag.CourseId = courseId;
            return View(grade) {
                if (ModelState.IsValid)
                {
                    var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
                    if (course == null) return HttpNotFound();

                    var existingGrade = course.Grades.Find(g => g.StudentId == grade.StudentId);
                    if (existingGrade == null) return HttpNotFound();

                    existingGrade.GradeValue = grade.GradeValue;
                    existingGrade.Comment = grade.Comment;

                    await _context.Courses.ReplaceOneAsync(c => c.Id == courseId, course);
                    return RedirectToAction("Index", new { courseId });
                }
                ViewBag.CourseId = courseId;
                return View(grade);
            }

        // GET: Grades/Delete/5
        public async Task<ActionResult> Delete(string courseId, string studentId)
        {
            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null) return HttpNotFound();

            var grade = course.Grades.Find(g => g.StudentId == studentId);
            if (grade == null) return HttpNotFound();

            ViewBag.CourseId = courseId;
            return View(grade);
        }

        // POST: Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string courseId, string studentId)
        {
            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null) return HttpNotFound();

            course.Grades.RemoveAll(g => g.StudentId == studentId);
            await _context.Courses.ReplaceOneAsync(c => c.Id == courseId, course);
            return RedirectToAction("Index", new { courseId });
        }
    }
}
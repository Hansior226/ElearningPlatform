using CourseManagement.Data;
using CourseManagement.Models;
using MongoDB.Driver;
using System.Collections.Generic;
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
            if (string.IsNullOrEmpty(courseId))
            {
                return HttpNotFound();
            }

            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null)
            {
                return HttpNotFound();
            }

            ViewBag.CourseId = courseId;
            return View(course.Grades);
        }

        // GET: Grades/Create
        public ActionResult Create(string courseId)
        {
            if (string.IsNullOrEmpty(courseId))
            {
                return HttpNotFound();
            }

            ViewBag.CourseId = courseId;
            return View();
        }

        // POST: Grades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string courseId, Grade grade)
        {
            if (string.IsNullOrEmpty(courseId))
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
                if (course == null)
                {
                    return HttpNotFound();
                }

                if (course.Grades == null)
                {
                    course.Grades = new List<Grade>();
                }

                course.Grades.Add(grade);
                await _context.Courses.ReplaceOneAsync(c => c.Id == courseId, course);
                return RedirectToAction("Index", new { courseId });
            }

            ViewBag.CourseId = courseId;
            return View(grade);
        }

        // GET: Grades/Edit
        public async Task<ActionResult> Edit(string courseId, string studentId)
        {
            if (string.IsNullOrEmpty(courseId) || string.IsNullOrEmpty(studentId))
            {
                return HttpNotFound();
            }

            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null)
            {
                return HttpNotFound();
            }

            var grade = course.Grades?.Find(g => g.StudentId == studentId);
            if (grade == null)
            {
                return HttpNotFound();
            }

            ViewBag.CourseId = courseId;
            return View(grade);
        }

        // POST: Grades/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string courseId, Grade grade)
        {
            if (string.IsNullOrEmpty(courseId) || string.IsNullOrEmpty(grade.StudentId))
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
                if (course == null)
                {
                    return HttpNotFound();
                }

                var existingGrade = course.Grades?.Find(g => g.StudentId == grade.StudentId);
                if (existingGrade == null)
                {
                    return HttpNotFound();
                }

                existingGrade.GradeValue = grade.GradeValue;
                existingGrade.Comment = grade.Comment;

                await _context.Courses.ReplaceOneAsync(c => c.Id == courseId, course);
                return RedirectToAction("Index", new { courseId });
            }

            ViewBag.CourseId = courseId;
            return View(grade);
        }

        // GET: Grades/Delete
        public async Task<ActionResult> Delete(string courseId, string studentId)
        {
            if (string.IsNullOrEmpty(courseId) || string.IsNullOrEmpty(studentId))
            {
                return HttpNotFound();
            }

            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null)
            {
                return HttpNotFound();
            }

            var grade = course.Grades?.Find(g => g.StudentId == studentId);
            if (grade == null)
            {
                return HttpNotFound();
            }

            ViewBag.CourseId = courseId;
            return View(grade);
        }

        // POST: Grades/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string courseId, string studentId)
        {
            if (string.IsNullOrEmpty(courseId) || string.IsNullOrEmpty(studentId))
            {
                return HttpNotFound();
            }

            var course = await _context.Courses.Find(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course == null)
            {
                return HttpNotFound();
            }

            course.Grades?.RemoveAll(g => g.StudentId == studentId);
            await _context.Courses.ReplaceOneAsync(c => c.Id == courseId, course);
            return RedirectToAction("Index", new { courseId });
        }
    }
}
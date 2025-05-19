using CourseManagement.Data;
using CourseManagement.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.IO;
using System.Web;
using System;

namespace CourseManagement.Controllers
{
    public class ImagesController : Controller
    {
        private readonly MongoDbContext _context;

        public ImagesController()
        {
            _context = new MongoDbContext();
        }

        // GET: Images
        public async Task<ActionResult> Index(string courseId)
        {
            if (string.IsNullOrEmpty(courseId))
            {
                return HttpNotFound();
            }

            var images = await _context.Images.Find(i => i.CourseId == courseId).ToListAsync();
            ViewBag.CourseId = courseId;
            return View(images);
        }

        // GET: Images/Create
        public ActionResult Create(string courseId)
        {
            if (string.IsNullOrEmpty(courseId))
            {
                return HttpNotFound();
            }

            ViewBag.CourseId = courseId;
            return View();
        }

        // POST: Images/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string courseId, Image image, HttpPostedFileBase file)
        {
            if (string.IsNullOrEmpty(courseId) || file == null || file.ContentLength == 0)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.InputStream.CopyTo(memoryStream);
                    image.Data = memoryStream.ToArray();
                }

                image.CourseId = courseId;
                image.Name = file.FileName;

                await _context.Images.InsertOneAsync(image);
                return RedirectToAction("Index", new { courseId });
            }

            ViewBag.CourseId = courseId;
            return View(image);
        }

        // GET: Images/Edit/5
        public async Task<ActionResult> Edit(string id, string courseId)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(courseId))
            {
                return HttpNotFound();
            }

            var image = await _context.Images.Find(i => i.Id == id).FirstOrDefaultAsync();
            if (image == null || image.CourseId != courseId)
            {
                return HttpNotFound();
            }

            ViewBag.CourseId = courseId;
            return View(image);
        }

        // POST: Images/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, string courseId, HttpPostedFileBase file)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(courseId))
            {
                return HttpNotFound();
            }

            var image = await _context.Images.Find(i => i.Id == id).FirstOrDefaultAsync();
            if (image == null || image.CourseId != courseId)
            {
                return HttpNotFound();
            }

            if (file != null && file.ContentLength > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.InputStream.CopyTo(memoryStream);
                    image.Data = memoryStream.ToArray();
                }
                image.Name = file.FileName;
                await _context.Images.ReplaceOneAsync(i => i.Id == id, image);
            }

            return RedirectToAction("Index", new { courseId });
        }

        // GET: Images/Delete/5
        public async Task<ActionResult> Delete(string id, string courseId)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(courseId))
            {
                return HttpNotFound();
            }

            var image = await _context.Images.Find(i => i.Id == id).FirstOrDefaultAsync();
            if (image == null || image.CourseId != courseId)
            {
                return HttpNotFound();
            }

            ViewBag.CourseId = courseId;
            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string courseId)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(courseId))
            {
                return HttpNotFound();
            }

            await _context.Images.DeleteOneAsync(i => i.Id == id);
            return RedirectToAction("Index", new { courseId });
        }

        // GET: Images/Display/5
        public async Task<ActionResult> Display(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var image = await _context.Images.Find(i => i.Id == id).FirstOrDefaultAsync();
            if (image == null)
            {
                return HttpNotFound();
            }

            // Ustal typ MIME na podstawie rozszerzenia pliku
            string mimeType = "image/jpeg"; // Domyślny
            if (image.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                mimeType = "image/png";
            }
            else if (image.Name.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
            {
                mimeType = "image/gif";
            }

            return File(image.Data, mimeType);
        }
    }
}
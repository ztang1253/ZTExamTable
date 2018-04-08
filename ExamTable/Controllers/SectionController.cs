using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExamTable.Models;

namespace ExamTable.Controllers
{
    public class SectionController : Controller
    {
        private ExamContext db = new ExamContext();

        // GET: Section
        public ActionResult Index()
        {
            var sections = db.sections.Where(c => c.is_deleted == false)
                .Include(s => s.course)
                .Include(s => s.faculty)
                .Include(s => s.program);
            return View(sections.ToList());
        }

        // GET: Section/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            section section = db.sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // GET: Section/Create
        public ActionResult Create()
        {
            ViewBag.course_id = new SelectList(db.courses, "id", "title");
            ViewBag.faculty_id = new SelectList(db.faculties, "id", "last_name");
            ViewBag.program_id = new SelectList(db.programs, "id", "title");
            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,section_number,program_id,course_id,student_enrolled,faculty_id,is_deleted,created_by,created_on,modified_by,modified_on")] section section)
        {
            if (ModelState.IsValid)
            {
                section.is_deleted = false;
                section.student_enrolled = 30;
                db.sections.Add(section);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.course_id = new SelectList(db.courses, "id", "title", section.course_id);
            ViewBag.faculty_id = new SelectList(db.faculties, "id", "last_name", section.faculty_id);
            ViewBag.program_id = new SelectList(db.programs, "id", "title", section.program_id);
            return View(section);
        }

        // GET: Section/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            section section = db.sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            ViewBag.course_id = new SelectList(db.courses, "id", "title", section.course_id);
            ViewBag.faculty_id = new SelectList(db.faculties, "id", "last_name", section.faculty_id);
            ViewBag.program_id = new SelectList(db.programs, "id", "title", section.program_id);
            return View(section);
        }

        // POST: Section/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,section_number,program_id,course_id,student_enrolled,faculty_id,is_deleted,created_by,created_on,modified_by,modified_on")] section section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.course_id = new SelectList(db.courses, "id", "title", section.course_id);
            ViewBag.faculty_id = new SelectList(db.faculties, "id", "last_name", section.faculty_id);
            ViewBag.program_id = new SelectList(db.programs, "id", "title", section.program_id);
            return View(section);
        }

        // GET: Section/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            section section = db.sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            section section = db.sections.Find(id);
            db.sections.Remove(section);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

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
            var sections = db.sections
                .Include(s => s.course)
                .Include(s => s.faculty)
                .Include(s => s.program)
                .OrderBy(o => o.is_deleted)
                .ThenBy(l => l.course.hours)
                .ThenBy(t => t.course.code)
                .ThenBy(i => i.id)
                .ThenBy(s => s.section_number);
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
            ViewBag.course_id = new SelectList(db.courses.OrderBy(o => o.code), "id", "courseDropdown");
            ViewBag.faculty_id = new SelectList(db.faculties.OrderBy(o => o.first_name), "id", "fullName");
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
                section.created_on = DateTime.Now;
                db.sections.Add(section);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.course_id = new SelectList(db.courses.OrderBy(o => o.code), "id", "courseDropdown", section.course_id);
            ViewBag.faculty_id = new SelectList(db.faculties.OrderBy(o => o.first_name), "id", "fullName", section.faculty_id);
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

            ViewBag.course_id = new SelectList(db.courses.OrderBy(o => o.code), "id", "courseDropdown", section.course_id);
            ViewBag.faculty_id = new SelectList(db.faculties.OrderBy(o => o.first_name), "id", "fullName", section.faculty_id);
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

                if (section.is_deleted == false)
                {
                    // set related course deleted = false;
                    var tempC = db.courses.Where(e => e.id == section.course_id);
                    if (tempC.ToList().Count > 0)
                    {
                        foreach (var item in tempC)
                        {
                            item.is_deleted = false;
                        }
                    }

                    // set related course - exam deleted = false;
                    var tempCE = db.course_exam.Where(e => e.course_id == section.course_id);
                    if (tempCE.ToList().Count > 0)
                    {
                        foreach (var item in tempCE)
                        {
                            item.is_deleted = false;
                        }
                    }
                }

                section.modified_on = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.course_id = new SelectList(db.courses.OrderBy(o => o.code), "id", "courseDropdown", section.course_id);
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

            //// set related section deleted = true;
            //var tempS = db.sections.Where(e => e.course_id == section.course_id);
            //if (tempS.ToList().Count > 0)
            //{
            //    foreach (var item in tempS)
            //    {
            //        item.is_deleted = true;
            //    }
            //}

            //// set related course deleted = true;
            //var tempC = db.courses.Where(e => e.id == section.course_id);
            //if (tempC.ToList().Count > 0)
            //{
            //    foreach (var item in tempC)
            //    {
            //        item.is_deleted = true;
            //    }
            //}

            //// set related course - exam deleted = true;
            //var tempCE = db.course_exam.Where(e => e.course_id == section.course_id);
            //if (tempCE.ToList().Count > 0)
            //{
            //    foreach (var item in tempCE)
            //    {
            //        item.is_deleted = true;
            //    }
            //}

            section.is_deleted = true;
            section.modified_on = DateTime.Now;
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

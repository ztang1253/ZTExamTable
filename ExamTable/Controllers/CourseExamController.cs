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
    public class CourseExamController : Controller
    {
        private ExamContext db = new ExamContext();

        // GET: CourseExam
        public ActionResult Index()
        {
            var course_exam = db.course_exam
                    .Include(e=>e.course)
                    .Include(c => c.room_type);
            return View(course_exam.ToList());
        }

        // GET: CourseExam/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            course_exam course_exam = db.course_exam.Find(id);
            if (course_exam == null)
            {
                return HttpNotFound();
            }
            return View(course_exam);
        }

        // GET: CourseExam/Create
        public ActionResult Create()
        {
            ViewBag.required_room_type_id = new SelectList(db.room_type, "id", "type");
            ViewBag.course_id = new SelectList(db.courses, "id", "title");
            return View();
        }

        // POST: CourseExam/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,course_id,have_final_exam,exam_length,required_room_type_id,final_exam_note,is_deleted,created_by,created_on,modified_by,modified_on")] course_exam course_exam)
        {
            if (ModelState.IsValid)
            {
                course_exam.is_deleted = false;
                db.course_exam.Add(course_exam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.required_room_type_id = new SelectList(db.room_type, "id", "type", course_exam.required_room_type_id);
            ViewBag.course_id = new SelectList(db.courses, "id", "title", course_exam.course_id);
            return View(course_exam);
        }

        // GET: CourseExam/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            course_exam course_exam = db.course_exam.Find(id);
            if (course_exam == null)
            {
                return HttpNotFound();
            }
            ViewBag.required_room_type_id = new SelectList(db.room_type, "id", "type", course_exam.required_room_type_id);
            return View(course_exam);
        }

        // POST: CourseExam/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,course_id,have_final_exam,exam_length,required_room_type_id,final_exam_note,is_deleted,created_by,created_on,modified_by,modified_on")] course_exam course_exam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course_exam).State = EntityState.Modified;

                if (course_exam.is_deleted == true)
                {
                    // set all related course to deleted = true
                    var tempC = db.courses.Where(e => e.id == course_exam.course_id);
                    if (tempC.ToList().Count > 0)
                    {
                        foreach (var item in tempC)
                        {
                            item.is_deleted = true;
                        }
                    }

                    // set all related sections deleted = true
                    var tempS = db.sections.Where(e => e.course_id == course_exam.course_id);
                    if (tempS.ToList().Count > 0)
                    {
                        foreach (var item in tempS)
                        {
                            item.is_deleted = true;
                        }
                    }
                }
                else
                {
                    // set all related course to deleted = false
                    var tempC = db.courses.Where(e => e.id == course_exam.course_id);
                    if (tempC.ToList().Count > 0)
                    {
                        foreach (var item in tempC)
                        {
                            item.is_deleted = false;
                        }
                    }

                    // set all related sections deleted = false
                    var tempS = db.sections.Where(e => e.course_id == course_exam.course_id);
                    if (tempS.ToList().Count > 0)
                    {
                        foreach (var item in tempS)
                        {
                            item.is_deleted = false;
                        }
                    }
                }







                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.required_room_type_id = new SelectList(db.room_type, "id", "type", course_exam.required_room_type_id);
            return View(course_exam);
        }

        // GET: CourseExam/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            course_exam course_exam = db.course_exam.Find(id);
            if (course_exam == null)
            {
                return HttpNotFound();
            }
            return View(course_exam);
        }

        // POST: CourseExam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            course_exam course_exam = db.course_exam.Find(id);
            course_exam.is_deleted = true;

            // set all related course to deleted = true
            var tempC = db.courses.Where(e => e.id == course_exam.course_id);
            if (tempC.ToList().Count > 0)
            {
                foreach (var item in tempC)
                {
                    item.is_deleted = true;
                }
            }

            // set all related sections deleted = true
            var tempS = db.sections.Where(e => e.course_id == course_exam.course_id);
            if (tempS.ToList().Count > 0)
            {
                foreach (var item in tempS)
                {
                    item.is_deleted = true;
                }
            }

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

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
    public class SpecialArrangementController : Controller
    {
        private ExamContext db = new ExamContext();

        // GET: SpecialArrangement
        public ActionResult Index()
        {
            return View(db.special_arrangement.Where(c => c.is_deleted == false).ToList());
        }

        // GET: SpecialArrangement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            special_arrangement special_arrangement = db.special_arrangement.Find(id);
            if (special_arrangement == null)
            {
                return HttpNotFound();
            }
            return View(special_arrangement);
        }

        // GET: SpecialArrangement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SpecialArrangement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,program_code,program_title,course_code,course_title,course_hours,section_number,have_final_exam,final_exam_note,room_request,exam_length,weekday,time,room,teacher_name,proctor,is_deleted,created_by,created_on,modified_by,modified_on")] special_arrangement special_arrangement)
        {
            if (ModelState.IsValid)
            {
                db.special_arrangement.Add(special_arrangement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(special_arrangement);
        }

        // GET: SpecialArrangement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            special_arrangement special_arrangement = db.special_arrangement.Find(id);
            if (special_arrangement == null)
            {
                return HttpNotFound();
            }
            return View(special_arrangement);
        }

        // POST: SpecialArrangement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,program_code,program_title,course_code,course_title,course_hours,section_number,have_final_exam,final_exam_note,room_request,exam_length,weekday,time,room,teacher_name,proctor,is_deleted,created_by,created_on,modified_by,modified_on")] special_arrangement special_arrangement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(special_arrangement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(special_arrangement);
        }

        // GET: SpecialArrangement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            special_arrangement special_arrangement = db.special_arrangement.Find(id);
            if (special_arrangement == null)
            {
                return HttpNotFound();
            }
            return View(special_arrangement);
        }

        // POST: SpecialArrangement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            special_arrangement special_arrangement = db.special_arrangement.Find(id);
            db.special_arrangement.Remove(special_arrangement);
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

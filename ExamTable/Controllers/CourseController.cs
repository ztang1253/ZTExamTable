﻿using System;
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
    public class CourseController : Controller
    {
        private ExamContext db = new ExamContext();

        // GET: Course
        public ActionResult Index()
        {
            var courses = db.courses.Where(c=>c.is_deleted == false)
                    .Include(c => c.room_type).Include(c => c.room_type1);
            return View(courses.ToList());
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            course course = db.courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            ViewBag.required_room1_type_id = new SelectList(db.room_type, "id", "type");
            ViewBag.required_room2_type_id = new SelectList(db.room_type, "id", "type");
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,code,title,hours,required_room1_type_id,required_room2_type_id,is_deleted,created_by,created_on,modified_by,modified_on")] course course)
        {
            if (ModelState.IsValid)
            {
                course.is_deleted = false;
                db.courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.required_room1_type_id = new SelectList(db.room_type, "id", "type", course.required_room1_type_id);
            ViewBag.required_room2_type_id = new SelectList(db.room_type, "id", "type", course.required_room2_type_id);
            return View(course);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            course course = db.courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.required_room1_type_id = new SelectList(db.room_type, "id", "type", course.required_room1_type_id);
            ViewBag.required_room2_type_id = new SelectList(db.room_type, "id", "type", course.required_room2_type_id);
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,code,title,hours,required_room1_type_id,required_room2_type_id,is_deleted,created_by,created_on,modified_by,modified_on")] course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.required_room1_type_id = new SelectList(db.room_type, "id", "type", course.required_room1_type_id);
            ViewBag.required_room2_type_id = new SelectList(db.room_type, "id", "type", course.required_room2_type_id);
            return View(course);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            course course = db.courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            course course = db.courses.Find(id);
            db.courses.Remove(course);
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
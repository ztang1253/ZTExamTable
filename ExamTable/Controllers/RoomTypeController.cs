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
    public class RoomTypeController : Controller
    {
        private ExamContext db = new ExamContext();

        // GET: RoomType
        public ActionResult Index()
        {
            return View(db.room_type.OrderBy(d=>d.is_deleted).ThenBy(o => o.type).ToList());
        }

        // GET: RoomType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            room_type room_type = db.room_type.Find(id);
            if (room_type == null)
            {
                return HttpNotFound();
            }
            return View(room_type);
        }

        // GET: RoomType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,type,is_deleted,created_by,created_on,modified_by,modified_on")] room_type room_type)
        {
            if (ModelState.IsValid)
            {
                room_type.is_deleted = false;
                room_type.created_on = DateTime.Now;
                db.room_type.Add(room_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(room_type);
        }

        // GET: RoomType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            room_type room_type = db.room_type.Find(id);
            if (room_type == null)
            {
                return HttpNotFound();
            }
            return View(room_type);
        }

        // POST: RoomType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,type,is_deleted,created_by,created_on,modified_by,modified_on")] room_type room_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room_type).State = EntityState.Modified;
                room_type.modified_on = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room_type);
        }

        // GET: RoomType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            room_type room_type = db.room_type.Find(id);
            if (room_type == null)
            {
                return HttpNotFound();
            }
            return View(room_type);
        }

        // POST: RoomType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            room_type room_type = db.room_type.Find(id);
            room_type.is_deleted = true;
            room_type.modified_on = DateTime.Now;
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using ExamTable.Controllers.algorithm;
using System.Web.Mvc;
using ExamTable.Models;

namespace ExamTable.Controllers
{
    public class ExamTimetableController : Controller
    {
        private ExamContext db = new ExamContext();

        // GET: ExamTimetable
        public ActionResult Index()
        {
            int ver = (int)db.exam_timetable.OrderByDescending(e => e.id).First().version_number;
            return View(db.exam_timetable.Where(e => e.version_number == ver)
                    .OrderByDescending(e=>e.version_number)
                    .ThenBy(w => w.weekday)
                    .ThenBy(a=>a.course_code)
                    .ThenBy(c=>c.section_number)
                    .ToList());
        }

        // GET: ExamTimetable/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            exam_timetable exam_timetable = db.exam_timetable.Find(id);
            if (exam_timetable == null)
            {
                return HttpNotFound();
            }
            return View(exam_timetable);
        }

        // GET: ExamTimetable/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExamTimetable/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version_number,program_code,program_title,course_code,course_title,course_hours,section_number,have_final_exam,final_exam_note,room_request,exam_length,weekday,time,room,teacher_name,proctor,is_deleted,created_by,created_on,modified_by,modified_on")] exam_timetable exam_timetable)
        {
            if (ModelState.IsValid)
            {
                GreedyAlgo algo = new GreedyAlgo();
                ExamTables table = algo.arrangeExam();
                int? versionNo;
                if (!(db.exam_timetable.Count() > 0))
                {
                    versionNo = 0;
                }
                else
                {
                    versionNo = db.exam_timetable.OrderByDescending(e => e.id).First().version_number;
                    versionNo++;
                }

                foreach (var item in table.solution)
                {
                    foreach (var section in item.sessionIds)
                    {
                        exam_timetable.version_number = versionNo;
                        exam_timetable.program_code = db.programs.Find(db.sections.Find(section).program_id).program_code;
                        exam_timetable.program_title = db.programs.Find(db.sections.Find(section).program_id).title;
                        exam_timetable.course_code = db.courses.Find(item.courseId).code;
                        exam_timetable.course_title = db.courses.Find(item.courseId).title;
                        exam_timetable.course_hours = db.courses.Find(item.courseId).hours;
                        exam_timetable.section_number = db.sections.Find(section).section_number;
                        exam_timetable.have_final_exam = db.course_exam.Where(ce => ce.course_id == item.courseId).First().have_final_exam;
                        exam_timetable.final_exam_note = db.course_exam.Where(ce => ce.course_id == item.courseId).First().final_exam_note;
                        exam_timetable.room_request = db.room_type.Find(item.roomType).type;
                        exam_timetable.exam_length = db.course_exam.Where(ce => ce.course_id == item.courseId).First().exam_length.ToString();
                        exam_timetable.weekday = getWeekday(item.weekDay);
                        exam_timetable.time = getTime(item.startHour, item.endHour, double.Parse(exam_timetable.exam_length));
                        exam_timetable.room = db.rooms.Find(item.roomId).name;
                        var sectionEntity = db.sections.Find(section);
                        if (sectionEntity != null && db.faculties.Find(sectionEntity.faculty_id) != null) // the function to get teacher name is not right here
                        {
                            exam_timetable.teacher_name = db.faculties.Find(sectionEntity.faculty_id).first_name + " " +
                                db.faculties.Find(sectionEntity.faculty_id).last_name;
                        }
                        exam_timetable.proctor = db.faculties.Find(item.protorId).first_name + " " +
                                db.faculties.Find(item.protorId).last_name;
                        exam_timetable.created_by = "Liz";
                        exam_timetable.is_deleted = false;
                        db.exam_timetable.Add(exam_timetable);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }

            return View(exam_timetable);
        }

        private string getTime(int start, int end, double length)
        {
            string tempTime = "";
            if (length%1 != 0)
            {
                tempTime = start.ToString() + ":00 - " + end.ToString() + ":30";
            }
            else
            {
                tempTime = start.ToString() + ":00 - " + end.ToString() + ":00";
            }

            return tempTime;
        }

        private string getWeekday(int weekday)
        {
            string tempDay = "";
            switch (weekday)
            {
                case 0:
                    tempDay = "Monday";
                    break;
                case 1:
                    tempDay = "Tuesday";
                    break;
                case 2:
                    tempDay = "Wednesday";
                    break;
                case 3:
                    tempDay = "Thursday";
                    break;
                case 4:
                    tempDay = "Friday";
                    break;
                default:
                    break;
            }
            return tempDay;
        }

        // GET: ExamTimetable/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            exam_timetable exam_timetable = db.exam_timetable.Find(id);
            if (exam_timetable == null)
            {
                return HttpNotFound();
            }
            return View(exam_timetable);
        }

        // POST: ExamTimetable/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version_number,program_code,program_title,course_code,course_title,course_hours,section_number,have_final_exam,final_exam_note,room_request,exam_length,weekday,time,room,teacher_name,proctor,is_deleted,created_by,created_on,modified_by,modified_on")] exam_timetable exam_timetable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exam_timetable).State = EntityState.Modified;
                if (exam_timetable.is_deleted != false)
                {
                    var tempSchedule = db.exam_timetable.Where(e => e.version_number == exam_timetable.version_number);
                    foreach (var exam in tempSchedule)
                    {
                        exam.is_deleted = true;
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(exam_timetable);
        }

        // GET: ExamTimetable/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            exam_timetable exam_timetable = db.exam_timetable.Find(id);
            if (exam_timetable == null)
            {
                return HttpNotFound();
            }
            return View(exam_timetable);
        }

        // POST: ExamTimetable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            exam_timetable exam_timetable = db.exam_timetable.Find(id);
            db.exam_timetable.Remove(exam_timetable);
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

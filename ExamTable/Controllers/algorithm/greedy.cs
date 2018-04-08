using System;
using System.Collections.Generic;
using ExamTable.Models;

namespace ExamTable.Controllers.algorithm
{
    public class ExamEntity
    {
        public int protorId;
        public int courseId;
        public List<int> sessionIds = new List<int>();
        public int roomId;
        public int roomType;
        public int weekDay; // 0 for monday
        public int startHour;
        public int startMinute;
        public int endHour;
        public int endMinute;
    }

    public class CourseEntity
    {
        public int courseId;
        public int level;
        public List<int> sessionIds = new List<int>();
        public int requiredRoomType;
        public double duration;
    }

    public class SpecialEntity
    {
        public int courseId;
        public int roomId;
        public int sessionId;
        public int protorId;
        public TimeValueInterval timeData;

        public SpecialEntity(int course, int room, int session, int protor, int week, int hour, double d)
        {
            courseId = course;
            roomId = room;
            sessionId = session;
            protorId = protor;
            timeData = new TimeValueInterval(week * 10000 + hour * 100, d);
        }

        public SpecialEntity(string course, string room, int session, string protor, string week, string hour, string d)
        {
            courseId = string2Int(course);
            roomId = string2Int(room);
            sessionId = session;
            protorId = string2Int(protor);
            int weekDay = string2Int(week);
            double startHour = Convert.ToDouble(hour);
            double duration = Convert.ToDouble(d);
            timeData = new TimeValueInterval (weekDay * 10000 + TimeValueInterval.hoursStandardValue(startHour), duration);
        }

        private static int string2Int(string value)
        {
            if (value.Length < 1 || value.ToLower().Equals("null"))
                return 0;

            return Convert.ToInt32(value);
        }
    }

    public class TimeValueInterval
    {
        public int startValue = 0; // weekday * 10000 + hour * 100 + min
        public int endValue = 0;

        public TimeValueInterval(int s, double duration)
        {
            initial(s, duration);
        }

        public void initial(int s, double duration)
        {
            startValue = s;
            endValue = startValue + hoursStandardValue(duration);
        }

        public static int hoursStandardValue(double hourInFloat)
        {
            hourInFloat = Math.Abs(hourInFloat);
            int hour = (int)hourInFloat;
            int min = (int)((hourInFloat - hour) * 60);
            return hour * 100 + min;
        }

        public bool SameWith(TimeValueInterval another)
        {
            return startValue == another.startValue && endValue == another.endValue;
        }

        public bool isConflict(TimeValueInterval another)
        {
            if (startValue < another.startValue)
            {
                return endValue > another.startValue;
            }

            return startValue < another.endValue;
        }

        public bool isValid()
        {
            int startHour = startValue % 10000;
            int endHour = endValue % 10000;
            return isHourValid(startHour) && isHourValid(endHour);
        }

        public static int value2Weekday(int value)
        {
            return value / 10000;
        }

        public static int value2Hour(int value)
        {
            return (value % 10000) / 100;
        }

        public static int value2Min(int value)
        {
            return value % 100;
        }

        private static bool isHourValid(int hour)
        {
            return (hour >= GreedyAlgo.MIN_AM_HOUR && hour <= GreedyAlgo.MAX_AM_HOUR) || (hour >= GreedyAlgo.MIN_PM_HOUR && hour <= GreedyAlgo.MAX_PM_HOUR);
        }
    }
    public class ProtorEntity
    {
        public int protorId;
        public int protTimes;
        public List<TimeValueInterval> occupied;

        public ProtorEntity(int protor)
        {
            protorId = protor;
            protTimes = 0;
            occupied = new List<TimeValueInterval>();
        }

        public void arrangeExam(TimeValueInterval data)
        {
            bool contain = false;
            foreach (TimeValueInterval tv in occupied)
            {
                if (tv.SameWith(data))
                {
                    contain = true;
                    break;
                }
            }

            if (!contain) occupied.Add(data);
            ++protTimes;
        }
    }

    public class RoomEntity
    {
        public int roomId;
        public int roomType;
        public int roomCapacity;
        public List<TimeValueInterval> occupied;

        public RoomEntity(int room, int type, int capacity)
        {
            roomId = room;
            roomType = type;
            roomCapacity = capacity;
            occupied = new List<TimeValueInterval>();
        }

        public void arrangeExam(TimeValueInterval data)
        {
            bool contain = false;
            foreach (TimeValueInterval tv in occupied)
            {
                if (tv.SameWith(data))
                {
                    contain = true;
                    break;
                }
            }

            if (!contain)  occupied.Add(data);
        }
    }

    public class ExamTables
    {
        public List<ExamEntity> solution;
        public ExamTables()
        {
            solution = new List<ExamEntity>();
        }

        public bool addExam(ExamEntity entity)
        {
            if (entity == null)
                return false;

            solution.Add(entity);
            return true;
        }
    }

    // Main function class enterance
    public class GreedyAlgo
    {
        public const int MIN_AM_HOUR = 900; // 9 o'clock
        public const int MAX_AM_HOUR = 1200; // 12 o'clock
        public const int MIN_PM_HOUR = 1300; // 13 o'clock
        public const int MAX_PM_HOUR = 1700; // 17 o'clock

        private const int SESSION_CAPACITY = 30;

        private ExamContext context = new ExamContext();
        private List<CourseEntity> allExamCourse = new List<CourseEntity>();
        private List<ProtorEntity> allProtor = new List<ProtorEntity>();
        private List<RoomEntity> allRoom = new List<RoomEntity>();
        private List<SpecialEntity> allSpecial = new List<SpecialEntity>();
        private List<TimeValueInterval>[] allLevelTimeLine;
        private int maxLevel;

        private static int adjustStartValue(int value)
        {
            int startHour = value % 10000;
            int weekDay = value / 10000;
            if (startHour < MIN_AM_HOUR)
                startHour = MIN_AM_HOUR;
            else if (startHour > MAX_AM_HOUR && startHour < MIN_PM_HOUR)
                startHour = MIN_PM_HOUR;
            else if (startHour > MAX_PM_HOUR)
            {
                ++weekDay;
                startHour = MIN_AM_HOUR;
            }

           return weekDay * 10000 + startHour;
        }

        private static TimeValueInterval getConflictTimeData(List<TimeValueInterval> occupied, TimeValueInterval one)
        {
            foreach (TimeValueInterval ti in occupied)
            {
                if (ti != null && ti.isConflict(one))
                    return ti;
            }

            return null;
        }

        private static int getValidStartTimeValue(int startValue, double duration)
        {
            startValue = adjustStartValue(startValue);
            int weekDay = startValue / 10000;
            int startHour = startValue % 10000;
            int standardDuation = TimeValueInterval.hoursStandardValue(duration);
            int startNow = startHour + standardDuation;
            int startPM = MIN_PM_HOUR + standardDuation;
            int startAM = MIN_AM_HOUR + standardDuation;
            if (startHour >= MIN_AM_HOUR && startNow <= MAX_AM_HOUR)
            {
                return startValue;
            }
            else if (startHour >= MIN_PM_HOUR && startNow <= MAX_PM_HOUR)
            {
                return startValue;
            }
            else if (startHour < MIN_PM_HOUR && startPM < MAX_PM_HOUR)
            {
                return weekDay * 10000 + MIN_PM_HOUR;
            }
            else if (startAM < MAX_AM_HOUR)
            {
                return (weekDay + 1) * 10000 + MIN_AM_HOUR;
            }
            else if (startPM < MAX_PM_HOUR)
            {
                return (weekDay + 1) * 10000 + MIN_PM_HOUR;
            }

            Console.WriteLine("Error!!!!!! Duration is too long!!!!!!!!");
            return -1;
        }

        // return a nonnull value except duration of one exam is too long
        private static TimeValueInterval getEarliest(List<TimeValueInterval> occupied, double duration)
        {
            int startValue = 0;
            startValue = getValidStartTimeValue(startValue, duration);
            if (startValue < 0)
                return null;

            TimeValueInterval t = new TimeValueInterval(startValue, duration);
            while (true)
            {
                TimeValueInterval tmp = getConflictTimeData(occupied, t);
                if (tmp == null)
                    break;

                startValue = tmp.endValue;
                startValue = getValidStartTimeValue(startValue, duration);
                t.initial(startValue, duration);
            }

            return t;
        }

        private TimeValueInterval getEarlistTimeData(CourseEntity ce)
        {
            TimeValueInterval earlist = getEarliest(allLevelTimeLine[ce.level], ce.duration);
            TimeValueInterval byRoom = null, tmp = null, byProtor = null;
            foreach (RoomEntity re in allRoom)
            {
                if (re.roomType == ce.requiredRoomType)
                {
                    tmp = getEarliest(re.occupied, ce.duration);
                    if (byRoom == null || byRoom.startValue > tmp.startValue)
                        byRoom = tmp;
                }
            }

            foreach (ProtorEntity pe in allProtor)
            {
                tmp = getEarliest(pe.occupied, ce.duration);
                if (byProtor == null || byProtor.startValue > tmp.startValue)
                    byProtor = tmp;
            }

            earlist = earlist.startValue > byRoom.startValue ? earlist : byRoom; // get the shortest brick
            earlist = earlist.startValue > byProtor.startValue ? earlist : byProtor;
            return earlist;
        }

        private void getProtorRoomAndSpecial()
        {
            allProtor.Clear();
            allRoom.Clear();
            allSpecial.Clear();

            foreach (faculty f in context.faculties)
            {
                if (f.is_deleted == false)
                {
                    allProtor.Add(new ProtorEntity(f.id));
                }
            }

            foreach (room r in context.rooms)
            {
                if (r.room_type_id != null && r.capacity != null && r.is_deleted == false)
                {
                    allRoom.Add(new RoomEntity(r.id, r.room_type_id ?? 0, r.capacity ?? 0));
                }
            }

            foreach (special_arrangement s in context.special_arrangement)
            {
                SpecialEntity se = new SpecialEntity(s.course_code, s.room, s.section_number ?? 0, s.proctor, s.weekday, s.time, s.exam_length);
                if (se.timeData.isValid()) // special arrangement is valid only time is set
                {
                    allSpecial.Add(se);
                }
            }


            allSpecial.Sort(
                delegate (SpecialEntity s1, SpecialEntity s2)
                {
                    return s1.courseId - s2.courseId;
                });
        }

        private void sortExamCourse()
        {
            foreach (course_exam ce in context.course_exam)
            {
                if (context.courses.Find(ce.course_id).is_deleted == false)
                {
                    if (ce.have_final_exam.ToUpper().Equals("YES") && ce.course_id != null && ce.required_room_type_id != null && ce.exam_length != null && ce.course != null)
                    {
                        CourseEntity course = new CourseEntity();
                        course.courseId = ce.course_id ?? 0;
                        course.requiredRoomType = ce.required_room_type_id ?? 0;
                        course.duration = ce.exam_length ?? 0;
                        course.level = ce.course.hours ?? -1;
                        maxLevel = Math.Max(maxLevel, course.level);
                        if (course.level == 9) course.level = 5;
                        else if (course.level == 10) course.level = 6;
                        allExamCourse.Add(course);
                    }
                }
            }

            foreach (section sessions in context.sections)
            {
                int courseId = sessions.course_id ?? 0;
                int sId = sessions.id;
                foreach (CourseEntity ce in allExamCourse)
                {
                    if (courseId == ce.courseId)
                    {
                        ce.sessionIds.Add(sId);
                    }
                }
            }

            // sort by session count of courses, descending (course with more sessions in first)
            allExamCourse.Sort(
                delegate (CourseEntity c1, CourseEntity c2)
                {
                    int i = c2.sessionIds.Count - c1.sessionIds.Count;
                    if (i == 0)
                        return c1.courseId - c2.courseId;
                    return i;
                });
        }

        private void sortProtorAndRoom()
        {
            allProtor.Sort(
                delegate (ProtorEntity c1, ProtorEntity c2)
                {
                    return c1.protTimes - c2.protTimes;
                });

            allRoom.Sort(
                delegate (RoomEntity c1, RoomEntity c2)
                {
                    return c1.occupied.Count - c2.occupied.Count;
                });
        }

        private List<SpecialEntity> checkInSpecial(int courseId)
        {
            List<SpecialEntity> ret = new List<SpecialEntity>();
            foreach (SpecialEntity s in allSpecial)
            {
                if (s.courseId == courseId)
                    ret.Add(s);
            }

            return ret;
        }

        private ExamEntity specialArrange(CourseEntity course, SpecialEntity special, TimeValueInterval timeData, List<int> remainSessions)
        {
            int sessionCt = remainSessions.Count;
            int sessionIndex = 0;
            RoomEntity room = null;

            foreach (RoomEntity r in allRoom)
            {
                if (special.roomId > 0)
                {
                    if (r.roomId == special.roomId)
                    {
                        room = r;
                        break;
                    }
                }
                else
                {
                    if (r.roomType == course.requiredRoomType && getConflictTimeData(r.occupied, timeData) == null)
                    {
                        room = r;
                        break;
                    }
                }
            }

            if (room == null)
                return null;

            bool hasSpecialProtor = special.protorId > 0;
            ExamEntity exam = new ExamEntity();
            exam.courseId = special.courseId;
            exam.roomId = special.roomId;
            exam.roomType = room.roomType;
            exam.startMinute = TimeValueInterval.value2Min(timeData.startValue);
            exam.startHour = TimeValueInterval.value2Hour(timeData.startValue);
            exam.weekDay = TimeValueInterval.value2Weekday(timeData.startValue);

            exam.endMinute = TimeValueInterval.value2Min(timeData.endValue);
            exam.endHour = TimeValueInterval.value2Hour(timeData.endValue);

            int sessionsThisRoom = room.roomCapacity / SESSION_CAPACITY;
            if (special.sessionId > 0)
            {
                exam.sessionIds.Add(special.sessionId);
                remainSessions.Remove(special.sessionId);
                --sessionsThisRoom;
            }

            for (int i = 0; i < sessionsThisRoom; ++i, ++sessionIndex)
            {
                if (sessionIndex >= remainSessions.Count)
                    break;

                exam.sessionIds.Add(remainSessions[sessionIndex]);
            }

            remainSessions.RemoveRange(0, sessionIndex);

            if (hasSpecialProtor)
            {
                exam.protorId = special.protorId;
                ProtorEntity pe = null;
                foreach (ProtorEntity p in allProtor)
                {
                    if (p.protorId == special.protorId)
                    {
                        pe = p;
                        break;
                    }
                }

                if (pe != null)
                {
                    pe.arrangeExam(timeData);
                }
            }
            else
            {
                foreach (ProtorEntity pe in allProtor)
                {
                    if (getConflictTimeData(pe.occupied, timeData) == null && pe.protorId != special.protorId)
                    {
                        exam.protorId = pe.protorId;
                        pe.arrangeExam(timeData);
                        break;
                    }
                }
            }

            room.arrangeExam(timeData);
            allLevelTimeLine[course.level].Add(timeData);
            return exam;
        }

        private List<ExamEntity> normalArrange(CourseEntity ce, List<int> remainSessions, TimeValueInterval timeData)
        {
            List<ExamEntity> ret = new List<ExamEntity>();
            int sessionCt = remainSessions.Count;
            int sessionIndex = 0, roomIndex = 0, protorIndex = 0;

            foreach (RoomEntity r in allRoom)
            {
                if (r.roomType == ce.requiredRoomType && getConflictTimeData(r.occupied, timeData) == null)
                {
                    ExamEntity entity = new ExamEntity();
                    entity.courseId = ce.courseId;
                    entity.roomId = r.roomId;
                    entity.roomType = r.roomType;
                    entity.startMinute = TimeValueInterval.value2Min(timeData.startValue);
                    entity.startHour = TimeValueInterval.value2Hour(timeData.startValue);
                    entity.weekDay = TimeValueInterval.value2Weekday(timeData.startValue);

                    entity.endMinute = TimeValueInterval.value2Min(timeData.endValue);
                    entity.endHour = TimeValueInterval.value2Hour(timeData.endValue);

                    r.arrangeExam(timeData);
                    allLevelTimeLine[ce.level].Add(timeData);

                    int sessionsThisRoom = r.roomCapacity / SESSION_CAPACITY;
                    for (int i = 0; i < sessionsThisRoom; ++i, ++sessionIndex)
                    {
                        if (sessionIndex >= remainSessions.Count)
                            break;

                        entity.sessionIds.Add(remainSessions[sessionIndex]);
                    }

                    foreach (ProtorEntity pe in allProtor)
                    {
                        if (getConflictTimeData(pe.occupied, timeData) == null)
                        {
                            entity.protorId = pe.protorId;
                            pe.arrangeExam(timeData);
                            break;
                        }
                    }

                    ++roomIndex;
                    ++protorIndex;
                    ret.Add(entity);
                }

                if (sessionCt <= sessionIndex)
                    break;
            }

            remainSessions.Clear();
            return ret;
        }

        public ExamTables arrangeExam()
        {
            ExamTables ret = new ExamTables();
            maxLevel = -1;
            sortExamCourse();
            getProtorRoomAndSpecial();
            allLevelTimeLine = new List<TimeValueInterval>[maxLevel + 1];
            for (int i = 0; i < maxLevel; ++i)
            {
                allLevelTimeLine[i] = new List<TimeValueInterval>();
            }

            while (allSpecial.Count > 0)
            {
                int courseId = allSpecial[0].courseId;
                List<SpecialEntity> s = checkInSpecial(courseId);
                CourseEntity ce = null;
                foreach (CourseEntity c in allExamCourse)
                {
                    if (c.courseId == courseId)
                    {
                        ce = c;
                        break;
                    }
                }

                if (ce == null)
                {
                    foreach (SpecialEntity se in s)
                    {
                        allSpecial.Remove(se);
                    }
                    continue;
                }

                List<int> remainSessions = new List<int>(ce.sessionIds);
                TimeValueInterval timeData = allSpecial[0].timeData;

                if (s.Count > 0) // special arrangement(course with a specified room an start time)
                {
                    foreach (SpecialEntity se in s)
                    {
                        ExamEntity e = specialArrange(ce, se, timeData, remainSessions);
                        ret.addExam(e);
                    }
                }

                if (remainSessions.Count > 0)
                {
                    List<ExamEntity> el = normalArrange(ce, remainSessions, timeData);
                    foreach (ExamEntity exam in el)
                    {
                        ret.addExam(exam);
                    }
                }

                foreach (SpecialEntity se in s)
                {
                    allSpecial.Remove(se);
                }

                allExamCourse.Remove(ce);
            }

            while (allExamCourse.Count > 0)
            {
                CourseEntity ce = allExamCourse[0];
                List<int> remainSessions = new List<int>(ce.sessionIds);
                TimeValueInterval timeData = getEarlistTimeData(ce);
                if (remainSessions.Count > 0)
                {
                    List<ExamEntity> el = normalArrange(ce, remainSessions, timeData);
                    foreach (ExamEntity exam in el)
                    {
                        ret.addExam(exam);
                    }
                }

                allExamCourse.Remove(ce);
                sortProtorAndRoom();
            }

            return ret;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimetablePlus_API.Entity;

namespace TimetablePlus_API.Response
{
    public class Timetable
    {
        public List<List<Lesson>> timetable;

        public Timetable(List<int> lessons)
        {
            if (lessons == null)
                return;
            timetable = new List<List<Lesson>>();
            for (int i = 0; i < 7; i++)
            {
                List<Lesson> temp = new List<Lesson>();
                timetable.Add(temp);
            }
            using (DataContext mContext = new DataContext())
                try
                {
                    foreach (int id in lessons)
                    {
                        var lesson = mContext.lesson.Where(p => p.id == id).FirstOrDefault();
                        if (lesson != null)
                        {
                            timetable[lesson.dayofweek].Add(lesson);
                        }
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
        }
    }
}

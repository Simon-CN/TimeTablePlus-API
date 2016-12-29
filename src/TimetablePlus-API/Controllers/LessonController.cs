using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimetablePlus_API.Response;
using TimetablePlus_API.Entity;
using TimetablePlus_API.Utility;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimetablePlus_API.Controllers
{
    [Route("api/[controller]")]
    public class LessonController : Controller
    {
        DataContext mContext = new DataContext();

        [HttpGet]
        [Route("timetable/{token}")]
        public BaseResponse<List<List<Lesson>>> GetTimetable(string token)
        {
            BaseResponse<List<List<Lesson>>> timetable = new BaseResponse<List<List<Lesson>>>();
            try
            {
                var lessonList = mContext.lesson_map.Where(p => p.uid == TokenUtil.GetUserId(token)).Select(p => p.lid).ToList();
                if (lessonList != null)
                {
                    Timetable t = new Timetable(lessonList);
                    timetable.setContent(t.timetable);
                }
            }
            catch (Exception e)
            {
                timetable.setFailed(e.Message);
            }

            return timetable;
        }

        [HttpPost]
        [Route("timetable/add")]
        public BaseResponse<Object> AddToTimetable(int id, string token)
        {
            BaseResponse<Object> rsp = new BaseResponse<Object>();
            try
            {
                int uid = TokenUtil.GetUserId(token);
                var lsn = mContext.lesson_map.Where(p => p.uid == uid && p.lid == id).ToList();
                if (lsn.Count == 0)
                {
                    mContext.lesson_map.Add(new LessonMap(uid, id));
                    mContext.SaveChanges();
                }
                else
                {
                    rsp.setFailed("课程已存在");
                }
            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }

            return rsp;
        }

        [HttpPost]
        [Route("create")]
        public BaseResponse<Object> CreateLesson(string name, string classroom, string teacher, int dayofweek,
            int start_time, int end_time, int start_week, int end_week, string token)
        {
            BaseResponse<Object> rsp = new BaseResponse<object>();
            Lesson newLesson = new Lesson();
            newLesson.name = name;
            newLesson.classroom = classroom;
            newLesson.teacher = teacher;
            newLesson.dayofweek = dayofweek;
            newLesson.startTime = start_time;
            newLesson.endTime = end_time;
            newLesson.startWeek = start_week;
            newLesson.endWeek = end_week;
            try
            {
                mContext.lesson.Add(newLesson);
                mContext.SaveChanges();
                int lid = mContext.lesson.Where(p => p.name == name).Select(p => p.id).FirstOrDefault();
                mContext.lesson_map.Add(new LessonMap(TokenUtil.GetUserId(token), lid));
                mContext.SaveChanges();
            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }

            return rsp;
        }

        [HttpGet]
        [Route("search/{str}")]
        public BaseResponse<List<Lesson>> GetSearchResult(string str)
        {
            BaseResponse<List<Lesson>> rsp = new BaseResponse<List<Lesson>>();
            try
            {
                var lessonList = mContext.lesson.Where(p => p.name.Contains(str)).ToList();
                rsp.setContent(lessonList);
            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }
            return rsp;
        }

        [HttpGet]
        [Route("list/{token}")]
        public BaseResponse<List<Lesson>> GetUserLessons(string token)
        {
            BaseResponse<List<Lesson>> rsp = new BaseResponse<List<Lesson>>();
            try
            {
                var lessons = mContext.lesson_map.Where(p => p.uid == TokenUtil.GetUserId(token)).Select(p => p.lid).ToList();
                if (lessons.Count != 0)
                {
                    List<Lesson> temp = new List<Lesson>();
                    foreach (int id in lessons)
                    {
                        temp.Add(mContext.lesson.Where(p => p.id == id).FirstOrDefault());
                    }
                    rsp.setContent(temp);
                }
            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }
            return rsp;
        }

        [HttpPost]
        [Route("timetable/remove")]
        public BaseResponse<Object> RemoveLessonFromTimetable(string token, int id)
        {
            BaseResponse<Object> rsp = new BaseResponse<Object>();
            try
            {
                var lm = mContext.lesson_map.Where(p => p.uid == TokenUtil.GetUserId(token) && p.lid == id).FirstOrDefault();
                if (lm != null)
                {
                    mContext.lesson_map.Remove(lm);
                    mContext.SaveChanges();
                }
            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }
            return rsp;
        }
    }
}

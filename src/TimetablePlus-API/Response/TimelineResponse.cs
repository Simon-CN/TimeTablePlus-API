using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimetablePlus_API.Entity;
using TimetablePlus_API.Utility;

namespace TimetablePlus_API.Response
{
    public class TimelineResponse
    {
        public int id { get; set; }

        public string content { get; set; }

        public long createTime { get; set; }

        public string lessonName { get; set; }

        public string userName { get; set; }

        public string portrait { get; set; }

        public string pictures { get; set; }

        public string location { get; set; }

        public TimelineResponse() { }
        public TimelineResponse(Timeline t)
        {
            id = t.id;
            content = t.content;
            pictures = t.pictures;
            location = t.location;
            createTime = DatetimeUtil.ConvertToTimestamp(t.createTime);

            try
            {
                DataContext mContext = new DataContext();
                var userInfo = mContext.user.Where(p => p.id == t.userId).FirstOrDefault();
                if (userInfo != null)
                {
                    userName = userInfo.name;
                    portrait = userInfo.portrait;
                }
                else
                {
                    throw new ArgumentNullException();
                }
                var lessonInfo = mContext.lesson.Where(p => p.id == t.lessonId).Select(p => p.name).FirstOrDefault();
                if (lessonInfo != null)
                {
                    lessonName = lessonInfo;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

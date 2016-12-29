using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimetablePlus_API.Response;
using TimetablePlus_API.Entity;
using TimetablePlus_API.Utility;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimetablePlus_API.Controllers
{
    [Route("api/[controller]")]
    public class TimelineController : Controller
    {
        DataContext mContext = new DataContext();
        
        [HttpPost]
        [Route("user")]
        public BaseResponse<List<TimelineResponse>> GetUserTimeline(string token, int page, int size)
        {
            BaseResponse<List<TimelineResponse>> rsp = new BaseResponse<List<TimelineResponse>>();
            try
            {
                var tl = mContext.timeline.Where(p => p.userId == TokenUtil.GetUserId(token)).OrderByDescending(p => p.id).Skip((page - 1) * size).Take(size).ToList();
                if (tl.Count > 0)
                {
                    List<TimelineResponse> content = new List<TimelineResponse>();
                    foreach (Timeline t in tl)
                    {
                        try
                        {
                            TimelineResponse tr = new TimelineResponse(t);
                            content.Add(tr);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    rsp.setContent(content);
                }


            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }
            return rsp;
        }

        [HttpPost]
        [Route("lesson")]
        public BaseResponse<List<TimelineResponse>> GetLessonTimeline(int id, int page, int size)
        {
            BaseResponse<List<TimelineResponse>> rsp = new BaseResponse<List<TimelineResponse>>();
            try
            {
                var tl = mContext.timeline.Where(p => p.lessonId == id).OrderByDescending(p => p.id).Skip((page - 1) * size).Take(size).ToList();
                if (tl.Count > 0)
                {
                    List<TimelineResponse> content = new List<TimelineResponse>();
                    foreach (Timeline t in tl)
                    {
                        try
                        {
                            TimelineResponse tr = new TimelineResponse(t);
                            content.Add(tr);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    rsp.setContent(content);
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


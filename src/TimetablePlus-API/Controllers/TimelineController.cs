using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimetablePlus_API.Response;
using TimetablePlus_API.Entity;
using TimetablePlus_API.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimetablePlus_API.Controllers
{
    [Route("api/[controller]")]
    public class TimelineController : Controller
    {
        DataContext mContext = new DataContext();
        private static readonly string PicutreUrl = "http://10.205.27.210:8080/uploads/";

        private IHostingEnvironment _environment;

        public TimelineController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

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

        [HttpPost]
        [Route("create")]
        public BaseResponse<Object> CreateTimeline(IFormFile[] files, string token, int lesson_id, string location, string content)
        {
            BaseResponse<Object> rsp = new BaseResponse<object>();
            int uid = TokenUtil.GetUserId(token);
            Timeline tl = new Timeline();
            tl.userId = uid;
            tl.content = content;
            tl.lessonId = lesson_id;
            tl.location = location;
            try
            {
                if (files != null && files.Length > 0)
                {
                    if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                    {
                        _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    }
                    var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                    List<string> pictures = new List<string>();
                    foreach (IFormFile file in files)
                    {
                        if (file != null)
                        {
                            string fileName = uid + "timeline" + file.FileName;
                            string path = Path.Combine(uploads, fileName);
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }
                            pictures.Add(PicutreUrl + fileName);
                        }
                    }
                    tl.pictures = ConvertPictureList(pictures);
                }

                mContext.timeline.Add(tl);
                mContext.SaveChanges();
            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }
            return rsp;

        }

        private string ConvertPictureList(List<string> ls)
        {
            string s = null;
            for (int i = 0; i < ls.Count; i++)
            {
                if (i == 0)
                {
                    s += ls[i];
                }
                else
                {
                    s +=( "," + ls[i]);
                }
            }
            return s;
        }
    }
}


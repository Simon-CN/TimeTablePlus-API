using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TimetablePlus_API.Response;
using TimetablePlus_API.Entity;
using TimetablePlus_API.Utility;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimetablePlus_API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IHostingEnvironment _environment;

        public HomeController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        private DataContext context = new DataContext();
        [HttpGet]
        [Route("value")]
        public BaseResponse<string> value()
        {
            BaseResponse<string> s = new BaseResponse<string>();

            s.setContent(context.user.Select(p => p.name).ToList()[0]);
            s.setSuccess();
            return s;
        }

        [HttpPost]
        [Route("login")]
        public BaseResponse<UserResponse> Login(string username, string password)
        {
            BaseResponse<UserResponse> rsp = new BaseResponse<UserResponse>();
            try
            {
                var uif = context.user.Where(p => p.name.Equals(username) && p.password.Equals(password)).ToList()[0];
                if (uif != null)
                {
                    UserResponse ur = new UserResponse();
                    ur.id = uif.id;
                    ur.name = uif.name;
                    ur.protrait = string.IsNullOrEmpty(uif.portrait) ? null : uif.portrait;
                    ur.schoolId = uif.schoolId;
                    ur.description = uif.desc;
                    ur.background = string.IsNullOrEmpty(uif.background) ? null : uif.background;
                    ur.token = TokenUtil.CreateToken(ur.id);
                    rsp.setContent(ur);
                }
            }
            catch (Exception e)
            {
                rsp.setContent(null);
                rsp.setFailed(e.Message);
            }

            return rsp;
        }

        [HttpPost]
        [Route("register")]
        public BaseResponse<Object> Register(string username, string password)
        {
            BaseResponse<Object> rsp = new BaseResponse<object>();
            try
            {
                User u = new User();
                u.name = username;
                u.password = password;
                context.user.Add(u);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }

            return rsp;
        }

        [HttpPost]
        [Route("editPassword")]
        public BaseResponse<Object> EditPassword(string password, string token)
        {
            BaseResponse<Object> rsp = new BaseResponse<object>();
            try
            {
                int uid = TokenUtil.GetUserId(token);
                var uif = context.user.Where(p => p.id == uid).FirstOrDefault();
                if (uif != null)
                {
                    uif.password = password;
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }
            return rsp;
        }

        [HttpPost]
        [Route("editDescription")]
        public BaseResponse<Object> EditDescription(string token, string desc)
        {
            BaseResponse<Object> rsp = new BaseResponse<object>();
            try
            {
                int uid = TokenUtil.GetUserId(token);
                var uif = context.user.Where(p => p.id == uid).FirstOrDefault();
                if (uif != null)
                {
                    uif.desc = desc;
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }

            return rsp;
        }

        [HttpPost]
        [Route("portrait")]
        public BaseResponse<string> UploadPortrait(IFormFile file)
        {
            BaseResponse<string> rsp = new BaseResponse<string>();
            try
            {
                string path = null;
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                if (uploads != null)
                {
                    path = Path.Combine(uploads, file.FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                rsp.setContent(path);
            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }
            return rsp;
        }

    }
}

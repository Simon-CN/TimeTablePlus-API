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
        private static readonly string PictureUrl = "http://10.205.27.210:8080/uploads/";

        private IHostingEnvironment _environment;

        public UserController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        private DataContext context = new DataContext();

        [HttpPost]
        [Route("info")]
        public BaseResponse<Object> GetUserInfo(string token)
        {
            BaseResponse<Object> s = new BaseResponse<Object>();
            try
            {
                int uid = TokenUtil.GetUserId(token);
                var uif = context.user.Where(p => p.id == uid).Select(p => new { screenName = p.name, desc = p.desc, portrait = p.portrait, background = p.background }).FirstOrDefault();
                s.setContent(uif);
            }
            catch (Exception e)
            {
                s.setFailed(e.Message);
            }

            return s;
        }

        [HttpPost]
        [Route("login")]
        public BaseResponse<string> Login(string username, string password)
        {
            BaseResponse<string> rsp = new BaseResponse<string>();
            try
            {
                var uif = context.user.Where(p => p.name.Equals(username) && p.password.Equals(password)).ToList()[0];
                if (uif != null)
                {
                    rsp.setContent(TokenUtil.CreateToken(uif.id));
                }
            }
            catch (Exception e)
            {
                rsp.setContent(null);
                rsp.setFailed("用户名或密码错误");
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
                if (context.user.Where(p => p.name == username).FirstOrDefault() != null)
                {
                    rsp.setFailed("用户名已存在");
                }
                else
                {
                    User u = new User();
                    u.name = username;
                    u.password = password;
                    context.user.Add(u);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }

            return rsp;
        }

        [HttpPost]
        [Route("editPassword")]
        public BaseResponse<Object> EditPassword(string password, string oldpassword, string token)
        {
            BaseResponse<Object> rsp = new BaseResponse<object>();
            try
            {
                int uid = TokenUtil.GetUserId(token);
                var uif = context.user.Where(p => p.id == uid && p.password == oldpassword).FirstOrDefault();
                if (uif == null)
                {
                    rsp.setFailed("密码错误");
                }
                else
                {
                    uif.password = password;
                    context.SaveChanges();
                }
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
        public BaseResponse<string> UploadPortrait(IFormFile file, string token)
        {
            BaseResponse<string> rsp = new BaseResponse<string>();
            try
            {
                var ui = context.user.Where(p => p.id == TokenUtil.GetUserId(token)).FirstOrDefault();
                string path = null;
                string fileName = null;
                if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                {
                    _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                if (uploads != null)
                {
                    fileName = ui.id + "portrait" + file.FileName;
                    path = Path.Combine(uploads, fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                ui.portrait = PictureUrl + fileName;
                context.SaveChanges();
                rsp.setContent(path);
            }
            catch (Exception e)
            {
                rsp.setFailed(e.Message);
            }
            return rsp;
        }

        [HttpPost]
        [Route("background")]
        public BaseResponse<string> UploadBackground(IFormFile file, string token)
        {
            BaseResponse<string> rsp = new BaseResponse<string>();
            try
            {
                var ui = context.user.Where(p => p.id == TokenUtil.GetUserId(token)).FirstOrDefault();
                string path = null;
                string fileName = null;
                if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                {
                    _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                if (uploads != null)
                {
                    fileName = ui.id + "background" + file.FileName;
                    path = Path.Combine(uploads, fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                ui.background = PictureUrl + fileName;
                context.SaveChanges();
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

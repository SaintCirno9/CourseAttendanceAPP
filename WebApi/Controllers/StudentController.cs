using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonShared.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Authentication;
using WebApi.DbContext;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> logger;
        private readonly JwtManager jwtManager;
        private readonly AppDbContext context;
        private readonly MailService mailService;
        private readonly DuplicateCheckingService _duplicateCheckingService;

        public StudentController(ILogger<StudentController> logger, JwtManager jwtManager,
            AppDbContext context, MailService mailService, DuplicateCheckingService duplicateCheckingService)
        {
            this.logger = logger;
            this.jwtManager = jwtManager;
            this.context = context;
            this.mailService = mailService;
            _duplicateCheckingService = duplicateCheckingService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] Student student)
        {
            logger.LogInformation("Student {Name} registered with id {Id}", student.Name, student.Id);
            if (!_duplicateCheckingService.CheckEmail(student.Email))
            {
                return BadRequest("邮箱已存在");
            }

            if (!_duplicateCheckingService.CheckPhone(student.PhoneNum))
            {
                return BadRequest("电话号码已存在");
            }

            context.Students.Add(student);
            context.SaveChanges();
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] Dictionary<string, string> loginData)
        {
            logger.LogInformation("Student with id {Id} wants to login", loginData["id"]);
            var student = context.Students.Find(loginData["id"]);
            if (student == null)
            {
                return Unauthorized("用户名不存在");
            }

            if (student.Password != loginData["password"])
            {
                return Unauthorized("密码错误");
            }

            var token = jwtManager.Authenticate(student);
            return Ok(new
            {
                token,
                face = student.FaceData
            });
        }
        
        [HttpGet("GetInfo")]
        public IActionResult GetInfo()
        {
            var student = context.Students.Find(jwtManager.GetId(Request));
            return Ok(student);
        }

        [HttpPost("SaveFaceData")]
        public IActionResult SaveFaceData([FromBody]string faceData)
        {
            var student = context.Students.Find(jwtManager.GetId(Request));
            logger.LogInformation("Student with id {Id} wants to save face data", student.Id);
            student.FaceData = faceData;
            context.SaveChanges();
            return Ok();
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody] Dictionary<string, string> data)
        {
            var code = data["code"];
            var newPassword = data["newPassword"];
            var student = context.Students.Find(jwtManager.GetId(Request));
            if (student is null)
            {
                return BadRequest("用户不存在");
            }

            if (!mailService.VerifyCode(student.Email, code))
            {
                return BadRequest("验证码错误或过期");
            }

            student.Password = newPassword;
            context.SaveChanges();
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword([FromBody] Dictionary<string, string> data)
        {
            var code = data["code"];
            var password = data["password"];
            var email = data["email"];
            var student = context.Students.First(student1 => student1.Email == email);
            if (student is null)
            {
                return BadRequest("用户不存在");
            }

            if (!mailService.VerifyCode(email, code))
            {
                return BadRequest("验证码错误或过期");
            }

            student.Password = password;
            context.SaveChanges();
            return Ok();
        }

        [HttpPost("ChangeEmail")]
        public IActionResult ChangeEmail([FromBody] Dictionary<string, string> data)
        {
            var newEmail = data["newEmail"];
            var code = data["code"];
            if (!_duplicateCheckingService.CheckEmail(newEmail))
            {
                return BadRequest("邮箱已存在");
            }

            var student = context.Students.Find(jwtManager.GetId(Request));
            logger.LogInformation("Student with id {Id} wants to change it's email to {NewEmail}", student.Id,
                newEmail);

            if (!mailService.VerifyCode(newEmail, code))
            {
                return BadRequest("验证码错误或过期");
            }

            student.Email = newEmail;
            context.SaveChanges();
            return Ok();
        }
    }
}
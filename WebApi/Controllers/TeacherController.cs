using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text.Json;
using CommonShared.DataModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Authentication;
using WebApi.DbContext;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly ILogger<TeacherController> logger;
        private readonly JwtManager jwtManager;
        private readonly AppDbContext context;
        private readonly MailService mailService;
        private readonly DuplicateCheckingService _duplicateCheckingService;

        public TeacherController(ILogger<TeacherController> logger, JwtManager jwtManager,
            AppDbContext context, MailService mailService, DuplicateCheckingService duplicateCheckingService)
        {
            this.logger = logger;
            this.jwtManager = jwtManager;
            this.context = context;
            this.mailService = mailService;
            _duplicateCheckingService = duplicateCheckingService;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] Teacher teacher)
        {
            logger.LogInformation("Teacher {Name} registered with id {Id}", teacher.Name, teacher.Id);
            if (!_duplicateCheckingService.CheckEmail(teacher.Email))
            {
                return Ok("邮箱已存在");
            }

            if (!_duplicateCheckingService.CheckPhone(teacher.PhoneNum))
            {
                return Ok("电话号码已存在");
            }

            context.Teachers.Add(teacher);
            context.SaveChanges();
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] Dictionary<string, string> loginData)
        {
            logger.LogInformation("Teacher with id {Id} wants to login", loginData["id"]);
            var teacher = context.Teachers.Find(loginData["id"]);
            if (teacher == null)
            {
                return Unauthorized("用户名不存在");
            }

            if (teacher.Password != loginData["password"])
            {
                return Unauthorized("密码错误");
            }

            var token = jwtManager.Authenticate(teacher);
            return Ok(token);
        }

        [HttpGet("GetInfo")]
        public IActionResult GetInfo()
        {
            var teacher = context.Teachers.Find(jwtManager.GetId(Request));
            return Ok(teacher);
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody] Dictionary<string, string> data)
        {
            var code = data["code"];
            var newPassword = data["newPassword"];
            var teacher = context.Teachers.Find(jwtManager.GetId(Request));
            if (teacher is null)
            {
                return BadRequest("用户不存在");
            }

            if (!mailService.VerifyCode(teacher.Email, code))
            {
                return BadRequest("验证码错误或过期");
            }

            teacher.Password = newPassword;
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
            var teacher = context.Teachers.First(teacher1 => teacher1.Email == email);
            if (teacher is null)
            {
                return BadRequest("用户不存在");
            }

            if (!mailService.VerifyCode(email, code))
            {
                return BadRequest("验证码错误或过期");
            }

            teacher.Password = password;
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

            var teacher = context.Teachers.Find(jwtManager.GetId(Request));
            logger.LogInformation("Teacher with id {Id} wants to change it's email to {NewEmail}", teacher.Id,
                newEmail);

            if (!mailService.VerifyCode(newEmail, code))
            {
                return BadRequest("验证码错误或过期");
            }

            teacher.Email = newEmail;
            context.SaveChanges();
            return Ok();
        }
    }
}
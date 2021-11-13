using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using WebApi.Authentication;
using WebApi.DbContext;
using WebApi.Services;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private readonly ILogger<MailController> logger;
        private readonly JwtManager jwtManager;
        private readonly MailService mailService;
        private readonly AppDbContext _context;

        public MailController(ILogger<MailController> logger, JwtManager jwtManager,
            MailService mailService, AppDbContext context)
        {
            this.logger = logger;
            this.jwtManager = jwtManager;
            this.mailService = mailService;
            _context = context;
        }
        
        [HttpGet("ResetPasswordMail/{endType}/{mailAddress}")]
        public ActionResult ResetPasswordMail(string endType ,string mailAddress)
        {
            switch (endType)
            {
                case "StudentEnd" when !_context.Students.Select(student => student.Email).Contains(mailAddress):
                    return BadRequest("邮箱不存在");
                case "TeacherEnd" when !_context.Teachers.Select(teacher => teacher.Email).Contains(mailAddress):
                    return BadRequest("邮箱不存在");
            }
            
            mailService.SendCode(mailAddress);
            return Ok();
        }

        [HttpGet("SendCode/{mailAddress}")]
        public ActionResult SendCode(string mailAddress)
        {
            mailService.SendCode(mailAddress);
            return Ok();
        }

        [Authorize]
        [HttpGet("SendCode")]
        public ActionResult SendCode()
        {
            var userId = jwtManager.GetId(Request);
            var mailAddress = _context.Teachers.Select(teacher => new {id = teacher.Id, email = teacher.Email})
                .Concat(_context.Students.Select(student => new {id = student.Id, email = student.Email}))
                .FirstOrDefault(arg => arg.id == userId)
                ?.email;
            if (mailAddress is null)
            {
                return BadRequest();
            }

            mailService.SendCode(mailAddress);
            return Ok();
        }
    }
}
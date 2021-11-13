using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace WebApi.Services
{
    public class MailService
    {
        private readonly IConfiguration configuration;
        private readonly HashSet<Code> CodeSet = new();

        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendCode(string mailAddress)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(configuration["MailSetting:Mail"]));
            email.To.Add(MailboxAddress.Parse(mailAddress));
            var code = new Code
            {
                Value = new Random().Next(100000, 999999).ToString(),
                MailAddress = mailAddress,
            };
            email.Subject = "验证码";
            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = code.Value
            };
            var smtp = new SmtpClient();
            smtp.Connect(configuration["MailSetting:Host"], int.Parse(configuration["MailSetting:Port"]));
            

            smtp.Authenticate(configuration["MailSetting:Mail"],
                configuration["MailSetting:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);

            CodeSet.RemoveWhere(code1 => code1.MailAddress == code.MailAddress);
            CodeSet.Add(code);

        }

        public bool VerifyCode(string mailAddress, string value)
        {
            if (CodeSet.All(code1 => code1.MailAddress != mailAddress))
            {
                return false;
            }

            var code = CodeSet.First(code1 => code1.MailAddress == mailAddress);
            if (code.ExpireTime < DateTime.Now)
            {
                CodeSet.Remove(code);
                return false;
            }

            if (code.Value != value)
            {
                return false;
            }

            CodeSet.Remove(code);
            return true;
        }

        public class Code
        {
            public string Value { get; set; }
            public string MailAddress { get; set; }
            public DateTime ExpireTime { get; } = DateTime.Now.AddMinutes(15);
        }
    }
}
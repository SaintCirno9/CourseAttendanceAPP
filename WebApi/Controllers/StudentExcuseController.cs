using System;
using System.Linq;
using CommonShared.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Authentication;
using WebApi.DbContext;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("student/excuse")]
    [Authorize(Roles = "student")]
    public class StudentExcuseController : ControllerBase
    {
        private readonly ILogger<StudentExcuseController> logger;
        private readonly JwtManager jwtManager;
        private readonly AppDbContext context;

        public StudentExcuseController(ILogger<StudentExcuseController> logger, JwtManager jwtManager,
            AppDbContext context)
        {
            this.logger = logger;
            this.jwtManager = jwtManager;
            this.context = context;
        }


        [HttpPost("SaveExcuse")]
        public IActionResult SaveExcuse([FromBody] Excuse excuse)
        {
            var student = context.Students.Find(jwtManager.GetId(Request));
            logger.LogInformation("Student With Id {StudentId} Wants To Save Excuse With CourseId = {CourseId}",
                student.Id, excuse.CourseId);
            if (excuse.Id is 0)
            {
                if (!student.Courses.Select(course => course.Id).Contains(excuse.CourseId))
                {
                    return NoContent();
                }

                excuse.ExcuseState = ExcuseState.Unreviewed;
                student.Excuses.Add(excuse);
                context.SaveChanges();
                return Ok();
            }

            var targetExcuse = student.Excuses.Find(excuse1 => excuse1.Id == excuse.Id);
            if (targetExcuse is not {ExcuseState: ExcuseState.Unreviewed})
            {
                return NoContent();
            }

            targetExcuse.Reason = excuse.Reason;
            targetExcuse.Period = excuse.Period;
            targetExcuse.CourseId = excuse.CourseId;
            context.SaveChanges();
            return Ok();
        }

        [HttpGet("GetExcuses")]
        public IActionResult GetExcuses()
        {
            return Ok(context.Students.Find(jwtManager.GetId(Request))?.Excuses);
        }


        [HttpDelete("DeleteExcuse/{excuseId:int}")]
        public IActionResult DeleteExcuse(int excuseId)
        {
            var student = context.Students.Find(jwtManager.GetId(Request));
            if (student is null)
            {
                return NoContent();
            }

            var targetExcuse = student.Excuses.Find(excuse => excuse.Id == excuseId);
            if (targetExcuse is not {ExcuseState: ExcuseState.Unreviewed})
            {
                return NoContent();
            }

            context.Excuses.Remove(targetExcuse);
            context.SaveChanges();
            return Ok();
        }
    }
}
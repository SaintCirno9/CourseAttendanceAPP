using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CommonShared.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Authentication;
using WebApi.DbContext;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("teacher/course")]
    [Authorize(Roles = "teacher")]
    public class TeacherCourseController : ControllerBase
    {
        private readonly ILogger<TeacherCourseController> logger;
        private readonly JwtManager jwtManager;
        private readonly AppDbContext context;

        public TeacherCourseController(ILogger<TeacherCourseController> logger, JwtManager jwtManager,
            AppDbContext context)
        {
            this.logger = logger;
            this.jwtManager = jwtManager;
            this.context = context;
        }

        [HttpGet("GetCourses")]
        public IActionResult GetCourses()
        {
            var courses = context.Teachers.Find(jwtManager.GetId(Request)).Courses;
            return Ok(courses);
        }

        [HttpPost("SaveCourse")]
        public IActionResult SaveCourse([FromBody] Course course)
        {
            var teacherId = jwtManager.GetId(Request);
            if (course.Id == 0)
            {
                course.TeacherId = teacherId;
                context.Courses.Add(course);
                context.SaveChanges();
                return Ok();
            }

            var targetCourse = context.Courses.Find(course.Id);
            if (targetCourse != null && targetCourse.TeacherId == teacherId && course.TeacherId == teacherId)
            {
                targetCourse.Name = course.Name;
                targetCourse.Number = course.Number;
                targetCourse.Serial = course.Serial;
                targetCourse.Duration = course.Duration;
                context.SaveChanges();
                return Ok();
            }

            return NoContent();
        }

        [HttpDelete("DeleteCourse/{courseId:int}")]
        public IActionResult DeleteCourse(int courseId)
        {
            var targetCourse = context.Courses.Find(courseId);
            if (targetCourse != null)
            {
                context.Courses.Remove(targetCourse);
                context.SaveChanges();
                return Ok();
            }

            return NoContent();
        }
    }
}
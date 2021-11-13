using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Authentication;
using WebApi.DbContext;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("student/course")]
    [Authorize(Roles = "student")]
    public class StudentCourseController : ControllerBase
    {
        private readonly ILogger<StudentCourseController> _logger;
        private readonly JwtManager _jwtManager;
        private readonly AppDbContext _context;

        public StudentCourseController(ILogger<StudentCourseController> logger, JwtManager jwtManager,
            AppDbContext context)
        {
            _logger = logger;
            _jwtManager = jwtManager;
            _context = context;
        }

        [HttpGet("GetCourses")]
        public IActionResult GetCourses()
        {
            return Ok(_context.Courses.ToList().Where(course =>
                course.Students == null ||
                !course.Students.Exists(student => student.Id == _jwtManager.GetId(Request))));
        }

        [HttpGet("GetAttendedCourses")]
        public IActionResult GetAttendedCourses()
        {
            return Ok(_context.Students.Find(_jwtManager.GetId(Request))?.Courses);
        }

        [HttpGet("ChooseCourses/{courseId:int}")]
        public IActionResult ChooseCourses(int courseId)
        {
            var targetCourse = _context.Courses.Find(courseId);
            if (targetCourse == null)
            {
                return NoContent();
            }

            var student = _context.Students.Find(_jwtManager.GetId(Request));
            student?.Courses.Add(targetCourse);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("ExitCourses/{courseId:int}")]
        public IActionResult ExitCourses(int courseId)
        {
            var targetCourse = _context.Courses.Find(courseId);
            if (targetCourse == null)
            {
                return NoContent();
            }

            var student = _context.Students.Find(_jwtManager.GetId(Request));
            if (student is null)
            {
                return BadRequest();
            }

            student.Courses.Remove(targetCourse);
            var courseExcuses = student.Excuses.Where(excuse => excuse.CourseId == targetCourse.Id).ToList();
            _context.Excuses.RemoveRange(courseExcuses);

            var courseAttendanceRecords = targetCourse.Attendances
                .SelectMany(attendance => attendance.AttendanceRecords)
                .Where(record => record.StudentId == student.Id).ToList();
            _context.AttendanceRecords.RemoveRange(courseAttendanceRecords);
            
            _context.SaveChanges();
            return Ok();
        }
    }
}
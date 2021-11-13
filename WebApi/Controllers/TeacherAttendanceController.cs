using System;
using System.Collections.Generic;
using System.Linq;
using CommonShared.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Authentication;
using WebApi.DbContext;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("teacher/attendance")]
    [Authorize(Roles = "teacher")]
    public class TeacherAttendanceController : ControllerBase
    {
        private readonly ILogger<TeacherAttendanceController> _logger;
        private readonly JwtManager _jwtManager;
        private readonly AppDbContext _context;
        private readonly AttendanceService _attendanceService;

        public TeacherAttendanceController(ILogger<TeacherAttendanceController> logger, JwtManager jwtManager,
            AppDbContext context, AttendanceService attendanceService)
        {
            _logger = logger;
            _jwtManager = jwtManager;
            _context = context;
            _attendanceService = attendanceService;
        }

        [HttpGet("GetAttendances")]
        public IActionResult GetAttendances()
        {
            var teacherId = _jwtManager.GetId(Request);
            _logger.LogInformation("Teacher With Id {Id} Wants To Get Attendances", teacherId);
            _attendanceService.GenerateAttendanceRecords();
            var attendances = _context.Teachers.Find(teacherId)?.Courses.SelectMany(course => course.Attendances);
            return Ok(attendances);
        }

        [HttpPost("SaveAttendance")]
        public IActionResult SaveAttendance([FromBody] Attendance attendance)
        {
            var teacher = _context.Teachers.Find(_jwtManager.GetId(Request));
            _logger.LogInformation("Teacher With Id {Id} Wants To Get Attendances", teacher.Id);
            var targetCourse = teacher?.Courses.Find(course => course.Id == attendance.CourseId);
            if (targetCourse is null)
            {
                return NoContent();
            }

            targetCourse.Attendances.Add(attendance);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("DeleteAttendance/{attendanceId:int}")]
        public IActionResult DeleteAttendance(int attendanceId)
        {
            var teacher = _context.Teachers.Find(_jwtManager.GetId(Request));
            var targetAttendance = teacher?.Courses.SelectMany(course => course.Attendances)
                .First(attendance1 => attendance1.Id == attendanceId);
            if (targetAttendance == null)
            {
                return NoContent();
            }

            _context.Attendances.Remove(targetAttendance);
            _context.SaveChanges();
            return Ok();
        }
    }
}
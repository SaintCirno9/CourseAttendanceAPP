using System;
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
    [Route("student/attendance")]
    [Authorize(Roles = "student")]
    public class StudentAttendanceController : ControllerBase
    {
        private readonly ILogger<StudentAttendanceController> _logger;
        private readonly JwtManager _jwtManager;
        private readonly AppDbContext _context;
        private readonly AttendanceService _attendanceService;

        public StudentAttendanceController(ILogger<StudentAttendanceController> logger, JwtManager jwtManager,
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
            var student = _context.Students.Find(_jwtManager.GetId(Request));
            if (student is null)
            {
                return BadRequest();
            }

            _attendanceService.GenerateAttendanceRecords();
            var attendances = student.Courses
                .SelectMany(course => course.Attendances).Where(attendance =>
                    attendance.StartTime < DateTime.Now && attendance.DeadTime > DateTime.Now &&
                    !attendance.AttendanceRecords.Exists(record => record.StudentId == student.Id));
            _logger.LogInformation("Student with id {Id} Fetched {Count} attendances", student.Id, attendances.Count());
            return Ok(attendances);
        }

        [HttpGet("GetAttendanceRecords")]
        public IActionResult GetAttendanceRecords()
        {
            return Ok(
                _context.Students.Find(_jwtManager.GetId(Request))?.AttendanceRecords);
        }

        [HttpPost("ProcessAttendance")]
        public IActionResult ProcessAttendance([FromBody] AttendanceRecord attendanceRecord)
        {
            var student = _context.Students.Find(_jwtManager.GetId(Request));
            var targetAttendance = student?.Courses.SelectMany(course => course.Attendances)
                .First(attendance1 => attendance1.Id == attendanceRecord.AttendanceId);
            if (targetAttendance is null)
            {
                return NoContent();
            }

            student.AttendanceRecords.Add(attendanceRecord);
            _context.SaveChanges();
            return Ok();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using CommonShared.DataModels;
using WebApi.DbContext;

namespace WebApi.Services
{
    public class AttendanceService
    {
        private readonly AppDbContext _context;

        public AttendanceService(AppDbContext context)
        {
            _context = context;
        }

        public void GenerateAttendanceRecords()
        {
            var attendances = _context.Attendances.ToList();

            foreach (var attendance in attendances)
            {
                if (attendance.DeadTime > DateTime.Now)
                {
                    continue;
                }

                var records = attendance.AttendanceRecords;
                var students = attendance.Course.Students;
                if (records.Count == attendance.Course.Students.Count)
                {
                    continue;
                }

                var absentStudentIds = students.Select(student => student.Id)
                    .Except(records.Select(record => record.StudentId));
                foreach (var absentStudentId in absentStudentIds)
                {
                    _context.AttendanceRecords.Add(new AttendanceRecord
                    {
                        AttendanceId = attendance.Id,
                        Result = GetResult(attendance, absentStudentId),
                        StudentId = absentStudentId
                    });
                }
            }

            _context.SaveChanges();

            AttendanceResult GetResult(Attendance lastAttendance, string studentId)
            {
                var previousAttendance = attendances.LastOrDefault(attendance =>
                    attendance.CourseId == lastAttendance.CourseId && attendance.Id < lastAttendance.Id);
                if (previousAttendance is null)
                {
                    return AttendanceResult.Absent;
                }
                if (previousAttendance.StartTime.AddMinutes(previousAttendance.Course.Duration) <=
                    lastAttendance.StartTime)
                {
                    return AttendanceResult.Absent;
                }

                return previousAttendance.AttendanceRecords.First(record => record.StudentId == studentId).Result ==
                       AttendanceResult.Normal
                    ? AttendanceResult.LeaveEarly
                    : AttendanceResult.Absent;
            }
        }
    }
}
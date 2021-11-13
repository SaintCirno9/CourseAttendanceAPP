using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CommonShared.JsonConverters;

namespace CommonShared.DataModels
{
    public class Student
    {
        public Student()
        {
        }

        [Key] public string Id { get; set; }

        [JsonConverter(typeof(JsonIgnoreSerialize))]
        public string Password { get; set; }

        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }

        [JsonConverter(typeof(JsonIgnoreSerialize))]
        public string FaceData { get; set; }

        public virtual List<Course> Courses { get; set; }
        public virtual List<Excuse> Excuses { get; set; }
        public virtual List<AttendanceRecord> AttendanceRecords { get; set; }
    }

    public class Teacher
    {
        public Teacher()
        {
        }

        [Key] public string Id { get; set; }

        [JsonConverter(typeof(JsonIgnoreSerialize))]
        public string Password { get; set; }

        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }

        public virtual List<Course> Courses { get; set; }
    }

    public class Course
    {
        public Course()
        {
        }

        [Key] public int Id { get; set; }
        public string Number { get; set; }
        public int Serial { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }

        public virtual List<Student> Students { get; set; }
        public virtual List<Attendance> Attendances { get; set; }
        public virtual List<Excuse> Excuses { get; set; }

        public string TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
    }

    public enum ExcuseState
    {
        Unreviewed,
        Passed,
        Rejected
    }

    public class Excuse
    {
        public Excuse()
        {
        }

        [Key] public int Id { get; set; }
        public string Period { get; set; }
        public string Reason { get; set; }
        public ExcuseState ExcuseState { get; set; }

        public string StudentId { get; set; }
        public virtual Student Student { get; set; }
        [Required] public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }

    public enum AttendanceType
    {
        Location,
        Face,
        Both
    }

    public class Attendance
    {
        public Attendance()
        {
        }

        [Key] public int Id { get; set; }
        public string Location { get; set; }
        [Required] public AttendanceType Type { get; set; }
        [Required] public DateTime StartTime { get; set; }
        [Required] public DateTime EndTime { get; set; }
        [Required] public DateTime DeadTime { get; set; }

        public virtual List<AttendanceRecord> AttendanceRecords { get; set; }

        [Required] public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }

    public enum AttendanceResult
    {
        Normal,
        Absent,
        Late,
        LeaveEarly,
        Excuse
    }

    public class AttendanceRecord
    {
        public AttendanceRecord()
        {
        }

        [Key] public int Id { get; set; }
        public AttendanceResult Result { get; set; }

        public string StudentId { get; set; }
        public virtual Student Student { get; set; }
        [Required] public int AttendanceId { get; set; }
        public virtual Attendance Attendance { get; set; }
    }
}
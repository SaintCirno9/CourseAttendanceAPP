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
    [Route("teacher/excuse")]
    [Authorize(Roles = "teacher")]
    public class TeacherExcuseController : ControllerBase
    {
        private readonly ILogger<TeacherExcuseController> logger;
        private readonly JwtManager jwtManager;
        private readonly AppDbContext context;

        public TeacherExcuseController(ILogger<TeacherExcuseController> logger, JwtManager jwtManager,
            AppDbContext context)
        {
            this.logger = logger;
            this.jwtManager = jwtManager;
            this.context = context;
        }

        [HttpGet("GetExcuses")]
        public IActionResult GetExcuses()
        {
            return Ok(context.Teachers.Find(jwtManager.GetId(Request))?.Courses.SelectMany(course => course.Excuses));
        }

        [HttpGet("ReviewExcuse/{excuseId:int}/{excuseState}")]
        public IActionResult ReviewExcuse(int excuseId, ExcuseState excuseState)
        {
            logger.LogInformation("Change Excuse({Id}) To State {State}", excuseId, excuseState);
            var excuse = context.Teachers.Find(jwtManager.GetId(Request))?.Courses.SelectMany(course => course.Excuses)
                .First(excuse1 => excuse1.Id == excuseId);
            if (excuse is null)
            {
                return NoContent();
            }

            excuse.ExcuseState = excuseState;
            context.SaveChanges();
            return Ok();
        }
    }
}
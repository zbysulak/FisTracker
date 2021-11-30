using FisTracker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FisTracker.Controllers
{
    [Route("api/[controller]")]
    public class TestController : BaseController
    {
        private readonly AppDbContext _context;
        public TestController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("Start")]
        public IActionResult Start()
        {
            return Ok(new MessageResult { Message = $"API is running" });
        }

        [HttpGet("Auth")]
        public ActionResult<MessageResult> Test()
        {
            return Ok(new MessageResult { Message = $"You are logged in as {this.CurrentUser.UserId}" });
        }

        // GET: api/TimeInputs
        [HttpGet("Timesheet")]
        public async Task<ActionResult<TimeSheet>> GetTimeInputs(DateTime from, DateTime to)
        {
            var times = await _context.TimeInputs.Where(ti =>
                ti.UserId == this.CurrentUser.UserId &&
                ti.Date >= from && ti.Date <= to
            ).OrderBy(ti => ti.Date).ToListAsync();
            var result = new TimeSheet(from,
                to,
                times);
            return result;
        }

        // GET: api/TimeInputs
        [AllowAnonymous]
        [HttpGet("Exception")]
        public async Task<IActionResult> Exception()
        {
            int i = 0;
            return Ok(1/i);
        }
    }
}

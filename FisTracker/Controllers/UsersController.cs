using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FisTracker.Data;
using System.Web.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace FisTracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult<User> Register(RegisterRequest r)
        {
            var newUser = _context.Users.Add(new Data.User()
            {
                Name = r.Name,
                Password = Crypto.HashPassword(r.Password)
            });
            _context.SaveChanges();
            return Ok(newUser.Entity);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<User> Login(LoginRequest r)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Name == r.Name).Result;
            if (user == null)
            {
                return Unauthorized("User not found");
            }
            if (!Crypto.VerifyHashedPassword(user.Password, r.Password))
            {
                return Unauthorized("Wrong password");
            }
            var sessionId = this.HttpContext.Session.Id;
            this.HttpContext.Session.SetString("init", "1");
            _context.Sessions.Add(new Session()
            {
                State = SesstionState.Valid,
                UserId = user.Id,
                ValidTo = DateTime.Now.AddHours(1),
                Id = this.HttpContext.Session.Id
            });
            _context.SaveChanges();
            //this.Response.Cookies.Append("sessionId", )
            return Ok(user);
        }
    }
}

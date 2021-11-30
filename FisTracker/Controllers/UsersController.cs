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
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public ActionResult<LoginResult> Register(RegisterRequest r)
        {
            _context.Users.Add(new Data.User()
            {
                Name = r.Name,
                Password = Crypto.HashPassword(r.Password)
            });
            _context.SaveChanges();

            return ProcessLogin(new LoginRequest { Name = r.Name, Password = r.Password });
        }

        private ActionResult<LoginResult> ProcessLogin(LoginRequest r)
        {

            var user = _context.Users.FirstOrDefaultAsync(u => u.Name == r.Name).Result;
            if (user == null)
            {
                return Unauthorized(new MessageResult { Message = "User not found", IsError = true });
            }
            if (!Crypto.VerifyHashedPassword(user.Password, r.Password))
            {
                return Unauthorized(new MessageResult { Message = "Wrong password", IsError = true });
            }
            var sessionId = this.HttpContext.Session.Id;
            var savedSession = _context.Sessions.Find(sessionId);
            this.HttpContext.Session.Set("persist-session", new byte[] { 1 });
            if (savedSession != null)
            {
                savedSession.UserId = user.Id;
                savedSession.State = SessionState.Valid;
                savedSession.ValidTo = DateTime.Now.AddHours(1);
            }
            else
            {
                _context.Sessions.Add(new Session()
                {
                    State = SessionState.Valid,
                    UserId = user.Id,
                    ValidTo = DateTime.Now.AddHours(1),
                    Id = this.HttpContext.Session.Id
                });
            }
            _context.SaveChanges();
            return Ok(new LoginResult { Name = r.Name, UserId = user.Id });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult<LoginResult> Login(LoginRequest r)
        {
            return ProcessLogin(r);
        }

        [HttpPost("Logout")]
        public ActionResult<LoginResult> Logout()
        {
            var sessionId = this.HttpContext.Session.Id;
            var savedSession = _context.Sessions.Find(sessionId);
            savedSession.State = SessionState.Expired;
            _context.SaveChanges();
            return Ok(new MessageResult { Message = "Logged out successfully" });
        }
    }
}

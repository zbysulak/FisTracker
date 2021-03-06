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
    public class UsersController : BaseController
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
            var newGuid = Guid.NewGuid().ToString();
            _context.Sessions.Add(new Session()
            {
                State = SessionState.Valid,
                UserId = user.Id,
                ValidTo = DateTime.Now.AddHours(1),
                Id = newGuid
            });
            _context.SaveChanges();
            return Ok(new LoginResult { Name = r.Name, UserId = user.Id, Token = newGuid });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult<LoginResult> Login(LoginRequest r)
        {
            return ProcessLogin(r);
        }

        [HttpGet("AuthorizationCheck")]
        public ActionResult<LoginResult> AuthorizationCheck()
        {
            if (Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues tokens))
            {
                var token = tokens[0];
                var user = this._context.Users.Find(this.CurrentUser.UserId);
                return Ok(new LoginResult { Name = user.Name, UserId = user.Id, Token = token });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("Logout")]
        public ActionResult<LoginResult> Logout()
        {
            if (Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues tokens))
            {

                var token = tokens[0];
                var savedSession = _context.Sessions.Find(token);
                savedSession.State = SessionState.Expired;
                _context.SaveChanges();
                return Ok(new MessageResult { Message = "Logged out successfully" });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FisTracker
{
    public class SimpleAuthentication : AuthenticationHandler<SimpleAuthenticationOptions>
    {
        //public IServiceProvider ServiceProvider { get; set; }
        private Data.AppDbContext _context;

        public SimpleAuthentication(IOptionsMonitor<SimpleAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IServiceProvider serviceProvider)
                : base(options, logger, encoder, clock)
        {
            var scope = serviceProvider.CreateScope();
            _context = scope.ServiceProvider.GetService<Data.AppDbContext>();
            //ServiceProvider = serviceProvider;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var s = _context.Sessions.Find(Request.HttpContext.Session.Id);
            if (s == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Session not found"));
            }
            if (s.State != Data.SesstionState.Valid)
            {
                return Task.FromResult(AuthenticateResult.Fail("Session is not valid"));
            }
            if (s.ValidTo < DateTime.Now)
            {
                s.State = Data.SesstionState.Expired;
                _context.SaveChanges();
                return Task.FromResult(AuthenticateResult.Fail("Session is expired"));
            }
            s.ValidTo = s.ValidTo.AddMinutes(10);
            _context.SaveChanges();
            var claims = new[] { new Claim("userId", s.UserId.ToString()) };
            var identity = new ClaimsIdentity(claims, nameof(SimpleAuthentication));

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), this.Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));//Fail("jen to zkousim"));// 

        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            return base.HandleForbiddenAsync(properties);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            return base.HandleChallengeAsync(properties);
        }


    }
    public class SimpleAuthenticationOptions : AuthenticationSchemeOptions
    {
    }
}

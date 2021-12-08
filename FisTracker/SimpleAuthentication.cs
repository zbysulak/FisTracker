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
            if (Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues token))
            {
                if (token.Count == 1)
                {
                    var s = _context.Sessions.Find(token[0]);
                    if (s == null)
                    {
                        return Task.FromResult(AuthenticateResult.Fail("Token not found"));
                    }
                    if (s.State != Data.SessionState.Valid)
                    {
                        return Task.FromResult(AuthenticateResult.Fail("Token is not valid"));
                    }
                    if (s.ValidTo < DateTime.Now)
                    {
                        s.State = Data.SessionState.Expired;
                        _context.SaveChanges();
                        return Task.FromResult(AuthenticateResult.Fail("Tokën is expired"));
                    }
                    s.ValidTo = s.ValidTo.AddMinutes(10);
                    _context.SaveChanges();
                    var claims = new[] { new Claim("userId", s.UserId.ToString()) };
                    var identity = new ClaimsIdentity(claims, nameof(SimpleAuthentication));

                    var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), this.Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else
                {
                    return Task.FromResult(AuthenticateResult.Fail("Got more authorization headers"));
                }
            }
            return Task.FromResult(AuthenticateResult.Fail("Token not found in headers"));
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            throw new UnauthorizedAccessException("Forbidden error");
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            return base.HandleChallengeAsync(properties);
            //throw new UnauthorizedAccessException("Challenge error");
        }


    }
    public class SimpleAuthenticationOptions : AuthenticationSchemeOptions
    {
    }
}

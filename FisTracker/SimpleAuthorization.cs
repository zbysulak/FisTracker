using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FisTracker
{
    public class SimpleAuthorizationRequirement : IAuthorizationRequirement { 
    
    }

    internal class SimpleAuthorization : AuthorizationHandler<SimpleAuthorizationRequirement>
    {
        private Data.AppDbContext _context;

        public SimpleAuthorization() : base() { }/* IServiceProvider sp) : base() { //Data.AppDbContext context) :base(){
            using (var scope = sp.CreateScope()) // this will use `IServiceScopeFactory` internally
            {
                _context = scope.ServiceProvider.GetService<Data.AppDbContext>();
            }
        }*/

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SimpleAuthorizationRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
            //throw new System.Exception("?:(");
            /*var sessionId = context.Resource..HttpContext.Session.Id;
            var ses = await _context.Sessions.FindAsync(sessionId);
            if (ses == null) {
                throw new System.Exception("Session not found");
            }
            if (ses.State != Data.SesstionState.Valid) {
                throw new System.Exception("Session is not valid");
            }
            if (ses.ValidTo < System.DateTime.Now) {
                ses.State = Data.SesstionState.Expired;
                _context.SaveChanges();
                throw new System.Exception("Session is expired");
            }*/
        }
    }
}
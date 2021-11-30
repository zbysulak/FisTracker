using FisTracker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FisTracker.Controllers
{
    [Authorize]
    [PersistSession]
    [ApiController]
    public class BaseController : ControllerBase
    {
        // todo: initialize this somewhere else
        protected UserInfo CurrentUser => new() { UserId = int.Parse(this.User.Claims.First(c => c.Type == "userId").Value) };

        public BaseController() { 
        }
    }
}

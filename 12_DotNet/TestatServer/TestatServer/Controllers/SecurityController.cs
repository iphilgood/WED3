using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestatServer.Services;

namespace TestatServer.Controllers
{
    [Route("auth")]
    [Authorize]
    public class SecurityController : Controller
    {
        private readonly SecurityService _securityService;

        public SecurityController(SecurityService securityService)
        {
            _securityService = securityService;
        }

        /// <summary>
        /// Request a new token for a given username/password pair.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<dynamic> Post([FromBody] AuthRequest req)
        {
            return await _securityService.GetToken(req);
        }
    }
}

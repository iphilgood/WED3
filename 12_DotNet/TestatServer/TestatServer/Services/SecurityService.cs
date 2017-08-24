using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestatServer.Models;

namespace TestatServer.Services
{
    public static class SecurityClaims
    {
        public static string AccountIdClaim = "AccountId";
    }

    public class SecurityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SecurityUtil _securityUtil;


        public SecurityService(UserManager<ApplicationUser> userManager, SecurityUtil securityUtil)
        {
            _userManager = userManager;
            _securityUtil = securityUtil;
        }

        public async Task<TokenInformation> GetToken([FromBody] AuthRequest req)
        {
            var user = await _userManager.FindByNameAsync(req.Username);
            if (await _userManager.CheckPasswordAsync(user, req.Password))
            {
                DateTime? expires = DateTime.UtcNow.AddMinutes(10);
                return new TokenInformation()
                {
                    Authenticated = true,
                    Token = GetToken(user.UserName, expires),
                    TokenExpires = expires
                };
            }
            return new TokenInformation() {Authenticated = false};
        }

        private string GetToken(string userName, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();
            var user = _userManager.Users.Include(x => x.Account).FirstOrDefault(x => x.UserName == userName);
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user.UserName, "TokenAuth"),
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(SecurityClaims.AccountIdClaim, user.Account.AccountNr),
                }
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = _securityUtil.TokenAuthOptions.Issuer,
                Audience = _securityUtil.TokenAuthOptions.Audience,
                SigningCredentials = _securityUtil.TokenAuthOptions.SigningCredentials,
                Subject = identity,
                Expires = expires,
            });
            return handler.WriteToken(securityToken);
        }
    }

    public class TokenInformation
    {
        public bool Authenticated { get; set; }
        public string Token { get; set; }

        public DateTime? TokenExpires { get; set; }
    }


    public class AuthRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

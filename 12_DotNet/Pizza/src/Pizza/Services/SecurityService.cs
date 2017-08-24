using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pizza.Data;
using Pizza.Models;

namespace Pizza.Services
{
    public class SecurityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenAuthOptions _tokenOptions;

        public SecurityService(UserManager<ApplicationUser> userManager, TokenAuthOptions tokenOptions)
        {
            _userManager = userManager;
            _tokenOptions = tokenOptions;
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
                    Token = GetToken(user, expires),
                    TokenExpires = expires
                };
            }
            return new TokenInformation() {Authenticated = false};
        }

        private string GetToken(ApplicationUser user, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();

            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user.UserName, "TokenAuth"),
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                }
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = _tokenOptions.Issuer,
                Audience = _tokenOptions.Audience,
                SigningCredentials = _tokenOptions.SigningCredentials,
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

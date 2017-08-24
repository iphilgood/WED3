using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;

namespace TestatServer.Services
{
    public class SecurityUtil
    {
        private readonly RsaSecurityKey _key;
        const string TokenAudience = "self";
        const string TokenIssuer = "TestatServer";

        public SecurityUtil()
        {
            RSA rsa = RSA.Create();
            rsa.KeySize = 2048;
            RSAParameters rsaKeyInfo = rsa.ExportParameters(true);
            _key = new RsaSecurityKey(rsaKeyInfo);
        }

        public TokenAuthOptions TokenAuthOptions => new TokenAuthOptions
        {
            Audience = TokenAudience,
            Issuer = TokenIssuer,
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256Signature)
        };

        public JwtBearerOptions JwtBearerOptions => new JwtBearerOptions
        {
            AutomaticAuthenticate = true,
            AutomaticChallenge = true,
            TokenValidationParameters = TokenValidationParameters
        };

        public TokenValidationParameters TokenValidationParameters => new TokenValidationParameters
        {
            // The signing key must match!
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _key,

            // Validate the JWT Issuer (iss) claim
            ValidateIssuer = true,
            ValidIssuer = TokenIssuer,

            // Validate the JWT Audience (aud) claim
            ValidateAudience = true,
            ValidAudience = TokenAudience,

            // Validate the token expiry
            ValidateLifetime = true,

            // If you want to allow a certain amount of clock drift, set that here:
            ClockSkew = TimeSpan.Zero
        };
    }

    public class TokenAuthOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}

using ho1a.reclutamiento.models.Seguridad;
using ho1a.reclutamiento.services.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ho1a.reclutamiento.services.Services
{
    public class TokenService : ITokenService
    {
        //private SymmetricSecurityKey key;
        private RsaSecurityKey key;
        private string algoritm;
        private string issuer;
        private string audience;

        //private readonly TokenManagement tokenManagement;
        public TokenService(string issuer, string audience, string secret)
        {
            var parameters = new CspParameters() { KeyContainerName = secret };
            var provider = new RSACryptoServiceProvider(2048, parameters);
            key = new RsaSecurityKey(provider);
            algoritm = SecurityAlgorithms.RsaSha256Signature;
            this.issuer = issuer;
            this.audience = audience;
        }

        public string CreateToken(User user, DateTime expiry)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var identity = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, $"{user.Nombre} {user.Apellidos}" ),
                    new Claim(ClaimTypes.PrimarySid, user.Id.ToString() )
                }, "Custom");

            SecurityToken token = tokenHandler.CreateJwtSecurityToken(new SecurityTokenDescriptor
            {
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(key, algoritm),
                Expires = expiry.ToUniversalTime(),
                Subject = identity
            });

            return tokenHandler.WriteToken(token);
        }

        public TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidAudience = audience,
                ValidIssuer = issuer,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(0)
            };
        }

    }
}

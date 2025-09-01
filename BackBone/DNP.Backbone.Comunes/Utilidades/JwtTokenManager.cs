using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Utilidades
{
    public class JwtTokenManager
    {
        private const string ISSUER = "DNP";
        
        private readonly SymmetricSecurityKey _key;

        public JwtTokenManager(string secret)
        {
            _key = new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }

        public string EscribirToken(List<Claim> claims, DateTime expires)
        {
            var subject = new ClaimsIdentity(claims);
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.HmacSha256Signature);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(issuer: ISSUER, subject: subject, expires: expires, signingCredentials: credentials);

            return handler.WriteToken(token);
        }

        public IEnumerable<Claim> ValidarToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = false,
                IssuerSigningKey = _key,
                IssuerSigningKeyValidator = (x) => {},
                ValidIssuer = ISSUER
            };

            var claimIdentity = handler.ValidateToken(token, parameters, out SecurityToken securityToken);

            return claimIdentity.Claims;
        }
    }
}

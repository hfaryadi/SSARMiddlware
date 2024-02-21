using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace SSARMiddlware
{
    internal static class JwtHelper
    {
        internal static bool IsValidToken(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    return false;
                }
                token = token.ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var jsonClaims = JsonConvert.SerializeObject(jwtSecurityToken.Claims);
                var claims = JsonConvert.DeserializeObject<ICollection<JwtClaim>>(jsonClaims);
                var ClientId = claims.FirstOrDefault(claim => claim.Type == "ClientId");
                var ClientSecret = claims.FirstOrDefault(claim => claim.Type == "ClientSecret");
                if (ClientId != null && ClientSecret != null && ClientHelper.HasClient(ClientId.Value, ClientSecret.Value))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String("11E59E9BEAC04681E0631214A8C0FD6E11E59E9BEAC44681E0631214A8C0FD6E"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("ClientId", "SSAR-Application"),
                new Claim("ClientSecret", "11E59E9BEAC14681E0631214A8C0FD6E-11E59E9BEAC24681E0631214A8C0FD6E-11E59E9BEAC34681E0631214A8C0FD6E")
                }),
                SigningCredentials = credentials,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }
    }

    internal class JwtClaim
    {
        public string Issuer { get; set; }
        public string OriginalIssuer { get; set; }
        public Properties Properties { get; set; }
        public object Subject { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }

    internal class Properties
    {
    }
}

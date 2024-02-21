using System.IdentityModel.Tokens.Jwt;

namespace SSARMiddlware
{
    internal static class JwtHelper
    {
        public static bool IsValidToken(string token)
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
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

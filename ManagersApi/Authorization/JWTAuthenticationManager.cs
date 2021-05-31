using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ManagersApi.Auth
{
    public class JWTAuthenticationManager
    {
        private readonly MongoDB db;

        private readonly string tokenKey;

        public JWTAuthenticationManager(string tokenKey)
        {
            this.tokenKey = tokenKey;
            db = new MongoDB();
        }
        
        public User Authenticate(string username, string password)
        {
            var user = db.GetUser(username, password);
            if (user == null)
                return null;
            
            user.Token = GenerateTokenString(user.Id, user.UserType, DateTime.UtcNow);
            return user;
        }

        string GenerateTokenString(int userId,UserType userType, DateTime expires, Claim[] claims = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Sid, userId.ToString()),
                        new Claim(ClaimTypes.Role, userType.ToString()), 
                    }),
                Expires = expires.AddYears(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
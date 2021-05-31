using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ManagersApi.Authorization
{
    /// <summary>
    /// Class which provide Authorization and authentication 
    /// </summary>
    public class JWTAuthenticationManager
    {
        private readonly MongoDB db;

        private readonly string tokenKey;

        public JWTAuthenticationManager(string tokenKey)
        {
            this.tokenKey = tokenKey;
            db = new MongoDB();
        }
        /// <summary>
        /// Generate token if username and password are correct
        /// </summary>
        /// <param name="username">Username of user</param>
        /// <param name="password">Password of user</param>
        /// <returns>User with token</returns>
        public User Authenticate(string username, string password)
        {
            var user = db.GetUser(username, password);
            if (user == null)
                return null;

            user.Token = GenerateTokenString(user.Id, user.UserType, DateTime.UtcNow);
            return user;
        }
        /// <summary>
        /// Token generator
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="expires"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        private string GenerateTokenString(int userId, UserType userType, DateTime expires, Claim[] claims = null)
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
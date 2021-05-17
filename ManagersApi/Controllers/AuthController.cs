using ManagersApi.Auth;
using ManagersApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly JWTAuthenticationManager jWTAuthenticationManager;

        public AuthController(JWTAuthenticationManager jWTAuthenticationManager)
        {
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            var token = jWTAuthenticationManager.Authenticate(userCred.Username, userCred.Password);
            if (token == null)
                return Unauthorized();
            var db = new DataBase();
            var manager = db.GetManagerById(token.Id);
            token.LastName = manager.LastName;
            token.FirstName = manager.FirstName;
            

            return Ok(token);
        }
    }
}
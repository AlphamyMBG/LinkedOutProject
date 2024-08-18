using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using BackendApp.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BackendApp.auth
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string tokenSecret = "ThisIsNotSafeDoItSaferAtSomeOtherPoint";
        private static readonly TimeSpan tokenLifeSpan = TimeSpan.FromHours(4);

        [HttpPost("token")]
        public IActionResult GenerateToken(TokenGenerationRequest request)
        {   
            var tokenHandler = new JwtSecurityTokenHandler();
            // var key = 
            return this.Ok();
        }
    }
}
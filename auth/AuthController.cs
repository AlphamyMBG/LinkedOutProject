using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackendApp.Model.Requests;
using BackendApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendApp.auth
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController
    (IConfiguration config, IAuthenticationService authenticationService) 
    : ControllerBase
    {
        private readonly IConfiguration configuration = config;
        private readonly IAuthenticationService authenticationService = authenticationService;
        private readonly TimeSpan tokenLifeSpan = TimeSpan.FromHours(4);

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult GenerateToken(TokenGenerationRequest request)
        {   
            var user = this.authenticationService.Authenticate(request);
            if(user != null)
            {
                var token = this.authenticationService.GenerateToken(
                    user,
                    this.configuration["Jwt:Key"] ?? throw new Exception("Token Key has not been set within config."),
                    this.configuration["Jwt:Issuer"] ?? throw new Exception("Token Issuer has not been set within config."),
                    this.configuration["Jwt:Audience"] ?? throw new Exception("Token Audience has not been set within config."),
                    this.tokenLifeSpan
                );
                return Ok(token);
            }
            return NotFound("User not found.");
        }
    }
}
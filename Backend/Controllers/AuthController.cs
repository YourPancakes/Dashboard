using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Backend.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly RsaSecurityKey _signingKey;
        private readonly string _issuer;
        private readonly string _audience;

        public AuthController(RsaSecurityKey signingKey, IConfiguration cfg)
        {
            _signingKey = signingKey;
            _issuer = cfg["Jwt:Issuer"] ?? throw new InvalidOperationException("Missing Jwt:Issuer");
            _audience = cfg["Jwt:Audience"] ?? throw new InvalidOperationException("Missing Jwt:Audience");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            if (req is null)
                return BadRequest(new { error = "Malformed request body." });
            if (req.Email != "admin@mirra.dev" || req.Pwd != "admin123")
                return Unauthorized(new { error = "Invalid credentials." });

            var creds = new SigningCredentials(_signingKey, SecurityAlgorithms.RsaSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, req.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,
                      DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                      ClaimValueTypes.Integer64)
        };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(
                             int.Parse(Request.HttpContext
                                              .RequestServices
                                              .GetRequiredService<IConfiguration>()
                                              ["Jwt:AccessTokenExpireMinutes"]!
                                         )
                           ),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }

}

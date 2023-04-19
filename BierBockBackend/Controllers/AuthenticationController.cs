using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BierBockBackend.Auth;
using DataStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BierBockBackend.Controllers
{

    [ApiController]
    [Route("/security/")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDatabaseContext _databaseContext;

        public AuthenticationController(IConfiguration configuration, AppDatabaseContext databaseContext)
        {
            _configuration = configuration;
            _databaseContext = databaseContext;
        }

        [AllowAnonymous]
        [HttpPost("createToken", Name = "CreateToken")]

        public object CreateToken(JAuthUser user)
        {
            var userMatch = _databaseContext.GetUsers().FirstOrDefault(x => x.UserName == user.UserName);

            if (userMatch != null) return Results.Unauthorized();

            if (!PasswordHashing.VerifyPassword(user.Password, userMatch.PasswordHash, userMatch.PasswordSalt))
                return Results.Unauthorized();

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return Results.Ok(stringToken);


        }
    }
    public record JAuthUser(string UserName, string Password);
}

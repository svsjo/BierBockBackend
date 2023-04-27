using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BierBockBackend.Auth;
using BierBockBackend.Data;
using BierBockBackend.Identity;
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
        private readonly IEmailSender _iEmailSender;

        public AuthenticationController(IConfiguration configuration, AppDatabaseContext databaseContext, IEmailSender iEmailSender)
        {
            _configuration = configuration;
            _databaseContext = databaseContext;
            _iEmailSender = iEmailSender;
        }

        [AllowAnonymous]
        [HttpGet("confirmEmail", Name = "ConfirmEmail")]
        public string ConfirmEmail(string emailToken, string username)
        {
            var user = _databaseContext.GetUsers().FirstOrDefault(x => x.UserName == username);
            if (user == null) return "Invalid Username";

            if (user.EmailConfirmed) return "User is already confirmed";

            if (user.EmailToken != emailToken) return "Invalid Token";

            user.EmailConfirmed = true;

            _databaseContext.Update(user);
            _databaseContext.SaveChanges();
            return "Success";
        }

        [AllowAnonymous]
        [HttpPost("register", Name = "Register")]
        public RequestStatus<object> Register(RegisterUser registerUser)
        {
            if (!registerUser.IsUserNameValid)
                return new RequestStatus<object>()
                    { Status = Status.Error, ErrorCode = ErrorCodes.invalid_username };

            if (_databaseContext.GetUsers().Any(x => x.UserName == registerUser.UserName))
                return new RequestStatus<object>()
                    { Status = Status.Error, ErrorCode = ErrorCodes.username_taken };

            if(_databaseContext.GetUsers().Any(x=>x.Email==registerUser.Email)) return new RequestStatus<object>()
                { Status = Status.Error, ErrorCode = ErrorCodes.mail_taken };

            if (!registerUser.IsVornameValid)
                return new RequestStatus<object>()
                    { Status = Status.Error, ErrorCode = ErrorCodes.invalid_firstname_format };

            if (!registerUser.IsNachnameValid)
                return new RequestStatus<object>()
                    { Status = Status.Error, ErrorCode = ErrorCodes.invalid_surname_format };

            if (!registerUser.IsPasswordValid)
                return new RequestStatus<object>()
                    { Status = Status.Error, ErrorCode = ErrorCodes.invalid_password_format };

            if (!registerUser.IsEmailValid)
                return new RequestStatus<object>()
                    { Status = Status.Error, ErrorCode = ErrorCodes.invalid_email_format };

            if (!registerUser.IsBirthdateValid)
                return new RequestStatus<object>()
                    { Status = Status.Error, ErrorCode = ErrorCodes.invalid_birthdate_format};

            var pwdHash = PasswordHashing.HashPassword(registerUser.Password);
            var user = new User
            {
                Name = registerUser.Nachname,
                VorName = registerUser.Vorname,
                UserName = registerUser.UserName,
                PasswordHash = pwdHash.Hash,
                PasswordSalt = pwdHash.Salt,
                Email = registerUser.Email,
                BirthDate = registerUser.Birthdate,
                EmailToken = GenerateRandomToken(),
                Location = new Coordinate(),
                FavouriteBeer = new Product()
            };
            _databaseContext.AddUser(user);

            _iEmailSender.SendConfirmationMail(user.Email, user.EmailToken, user.UserName);

            return new RequestStatus<object>()
            {
                Status = Status.Successful
            };
        }


        private string GenerateRandomToken()
        {
            var g = Guid.NewGuid();
            var GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString;
        }


        [AllowAnonymous]
        [HttpPost("createToken", Name = "CreateToken")]
        public RequestStatus<object> CreateToken(AuthUser user)
        {
            var userMatch = _databaseContext.GetUsers().FirstOrDefault(x => x.UserName == user.UserName);

            #region Validation

            if (userMatch == null)
                return new RequestStatus<object>()
                {
                    Status = Status.Error,
                    ErrorCode = ErrorCodes.user_not_found
                };

            if (!userMatch.EmailConfirmed)
                return new RequestStatus<object>()
                {
                    Status = Status.Error,
                    ErrorCode = ErrorCodes.mail_not_confirmed
                };

            if (!PasswordHashing.VerifyPassword(user.Password, userMatch.PasswordHash, userMatch.PasswordSalt))
                return new RequestStatus<object>()
                {
                    Status = Status.Error,
                    ErrorCode = ErrorCodes.invalid_password
                };

            #endregion

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
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(IdentityData.AdminUserClaimName, userMatch.IsAdmin.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);

            return new RequestStatus<object>()
            {
                Status = Status.Successful,
                Result = stringToken
            };
        }


        
        public record AuthUser(string UserName, string Password);

        public class RegisterUser
        {
            public RegisterUser(string UserName, string Vorname, string Nachname, string Password, string Email,
                string Birthdate)
            {
                this.UserName = UserName;
                this.Vorname = Vorname;
                this.Nachname = Nachname;
                this.Password = Password;
                this.Email = Email;
                this.Birthdate = Birthdate;
            }

            public string UserName { get; init; }
            public string Vorname { get; init; }
            public string Nachname { get; init; }
            public string Password { get; init; }
            public string Email { get; init; }
            public string Birthdate { get; init; }


            public bool IsUserNameValid => !string.IsNullOrEmpty(UserName) && UserName.Length is <= 15 and >= 4;
            public bool IsVornameValid => !string.IsNullOrEmpty(Vorname) && Vorname.Length is <= 15 and >= 3;
            public bool IsNachnameValid => !string.IsNullOrEmpty(Nachname) && Nachname.Length is <= 15 and >= 3;
            public bool IsPasswordValid => !string.IsNullOrEmpty(Password) && Password.Length is >= 8 and <= 20;

            public bool IsEmailValid =>
                !string.IsNullOrEmpty(Email) && Email.Length is <= 100 and >= 4 && IsValidEmail(Email);

            public bool IsBirthdateValid => !string.IsNullOrEmpty(Birthdate) && Birthdate.Length == 10;


            private static bool IsValidEmail(string email)
            {
                try
                {
                    var adr = new System.Net.Mail.MailAddress(email);
                    return adr.Address == email;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}

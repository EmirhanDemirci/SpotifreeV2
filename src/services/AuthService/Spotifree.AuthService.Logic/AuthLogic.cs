using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Spotifree.AuthService.DataAccess.Data.Services.Interfaces;
using Spotifree.AuthService.Logic.Exceptions;
using Spotifree.AuthService.Logic.Interfaces;
using Spotifree.AuthService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Spotifree.AuthService.Logic
{
    public class AuthLogic : IAuthLogic
    {
        private readonly IAuthService _authService;
        private readonly Jwt _jwtOptions;

        public AuthLogic(IAuthService authService, IOptions<Jwt> jwtOptions)
        {
            _authService = authService;
            _jwtOptions = jwtOptions.Value;
        }
        public CredentialsWithToken Authenticate(string email, string password)
        {
            var credentials = _authService.GetUser(email);

            // return null if user not found
            if (credentials == null)
                throw new UnauthorizedAccessException("Invalid email");

            DeHashPassword(password, credentials.Password);

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, credentials.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //credentials = credentials;
            return new CredentialsWithToken { Credentials = credentials, Token = tokenHandler.WriteToken(token) };
        }

        public void CreateCredentials(Credentials credentials)
        {
            if (credentials == null || credentials.Email == "" || credentials.Password == "")
                //TODO: Custom exception 
                throw new InvalidCredentials("Invalid credentials given");

            credentials.Password = HashPassword(credentials.Password);
            _authService.Create(credentials);
        }
        public string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        public void DeHashPassword(string password, string dbPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(dbPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    throw new UnauthorizedAccessException("Invalid password");
        }
    }
}
using Microsoft.IdentityModel.Tokens;
using NotesOnline.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NotesOnline.Api.Services
{
    /// <summary>
    /// Service to authenticate users
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public AuthService(IConfiguration configuration)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Verifies the password hash
        /// </summary>
        /// <param name="password">The string password</param>
        /// <param name="hash">the byte hash of the password</param>
        /// <param name="salt">the salt used to encrypt the password</param>
        /// <returns></returns>
        public bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hash);

            }
        }

        /// <summary>
        /// Creates a password hash 
        /// </summary>
        /// <param name="password">The string password</param>
        /// <param name="hash">The hashed password</param>
        /// <param name="salt">The salt the password was hashed with</param>
        public void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Creates a JWT token
        /// </summary>
        /// <param name="userReadDto">The user data</param>
        /// <returns></returns>
        public string CreateToken(UserReadDto userReadDto)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]!));
            var signingCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", userReadDto.Id.ToString()));
            claimsForToken.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userReadDto.UserName!));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(20),
                signingCredentials);

            var finishedToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return finishedToken;
        }
    }
}

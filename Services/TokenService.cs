using Dating_App.Entities;
using Dating_App.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dating_App.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        /// <summary>
        /// Creates a JSON Web Token (JWT) for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token is being created. This parameter cannot be null.</param>
        /// <returns>A string representing the generated JWT.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public string CreateToken(AppUser user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            };

            var tokenKey = config["JWT_Secret"] ?? throw new InvalidOperationException("TokenKey is not configured.");
            if(tokenKey.Length < 16)
            {
                throw new InvalidOperationException("TokenKey must be at least 16 characters long for security reasons.");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.DisplayName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}

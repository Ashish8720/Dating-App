using Dating_App.Data;
using Dating_App.DTOs;
using Dating_App.Entities;
using Dating_App.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Dating_App.Services
{
    /// <summary>
    /// Provides functionality for managing user accounts, including user registration and authentication.
    /// </summary>
    /// <remarks>This service is responsible for creating new user accounts and authenticating existing users.
    /// It interacts with the application's database context to persist user data and validate credentials.</remarks>
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AccountService(AppDbContext context , ITokenService tokenService )
        {
            _context = context;
            _tokenService = tokenService;
        }

        // Register method to create a new user
        public async Task<UserDTO> Register(RegisterDTO register)
        {
            try
            {
                using (var hmac = new HMACSHA512())
                {
                    var user = new AppUser
                    {
                        Email = register.Email,
                        DisplayName = register.DisplayName,
                        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                        PasswordSalt = hmac.Key
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return new UserDTO
                    {
                        Id = user.Id,
                        DisplayName = user.DisplayName,
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user)   
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        // Login method to authenticate a user
        public async Task<UserDTO> Login(LoginDTO login)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == login.Email);
                if (user == null) throw new UnauthorizedAccessException("Invalid email or password");
                using (var hmac = new HMACSHA512(user.PasswordSalt))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if (computedHash[i] != user.PasswordHash[i])
                        {
                            throw new UnauthorizedAccessException("Invalid email or password");
                        }
                    }
                }
                return new UserDTO
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    ImageUrl = user.ImageUrl!,
                    Token = _tokenService.CreateToken(user),

                };
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}

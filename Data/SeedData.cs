using Dating_App.DTOs;
using Dating_App.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Dating_App.Data
{
    public class SeedData
    {
        // Seed Users
        public static async Task SeedUsers(AppDbContext context)
        {
            if (await context.Users.AnyAsync()) return;

            // Read user seed data from JSON file
            var memberDetails = await File.ReadAllTextAsync("Data/UserSeedData.json");

            Console.WriteLine(memberDetails);

            // Deserialize JSON data to list of SeedUserDto
            var members = System.Text.Json.JsonSerializer.Deserialize<List<SeedUserDto>>(memberDetails);

            if (members == null)
            {
                Console.WriteLine("No memebers in seed DTO");
                return;
            }

            
            

            foreach (var member in members)
            {
                // Create HMACSHA512 instance for password hashing
                using var hmac = new HMACSHA512();

                var user = new AppUser()
                {
                    Id = member.Id,
                    Email = member.Email,
                    DisplayName = member.DisplayName,

                    ImageUrl = member.ImageUrl,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")),
                    PasswordSalt = hmac.Key,

                    // Navigation property
                    Member = new Member
                    {
                        Id = member.Id,
                        DisplayName = member.DisplayName,
                        Description = member.Description,
                        DateOfBirth = member.DateOfBirth,
                        ImageUrl = member.ImageUrl,
                        Gender = member.Gender,
                        Created = member.Created,
                        LastActive = member.LastActive,
                        City = member.City,
                        Country = member.Country


                    }

                    
                };

                user.Member.Photos.Add(new Photo
                {
                    Url = member.ImageUrl!,
                    MemberId = member.Id,
                });

                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
        }
    }
}

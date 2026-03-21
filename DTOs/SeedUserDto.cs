namespace Dating_App.DTOs
{
    public class SeedUserDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string? ImageUrl { get; set; }

        public required string DisplayName { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public required string Gender { get; set; }

        public required string City { get; set; }

        public required string Country { get; set; }

    }
}

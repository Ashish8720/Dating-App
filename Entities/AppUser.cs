namespace Dating_App.Entities
{
    public class AppUser
    {
        // Using Guid for simplicity; in a real app, consider using a more robust ID generation strategy
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Using 'required' to ensure these properties are set during object initialization
        /// <summary>
        /// Gets or sets the display name of the object.
        /// </summary>
        public required string DisplayName { get; set; }

        // Using 'required' to ensure these properties are set during object initialization
        /// <summary>
        /// Gets or sets the email address associated with the object.
        /// </summary>
        public required string Email { get; set; }
    }
}

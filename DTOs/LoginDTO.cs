namespace Dating_App.DTOs
{
    public class LoginDTO
    {
        // Using 'required' to ensure these properties are set during object initialization
        /// <summary>
        /// Gets or sets the user email address 
        /// </summary>
        public required string Email { get; set; }
        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public required string Password { get; set; }
    }
}

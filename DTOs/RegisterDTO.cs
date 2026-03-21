using System.ComponentModel.DataAnnotations;

namespace Dating_App.DTOs
{
    public class RegisterDTO
    {
        // Using 'required' to ensure these properties are set during object initialization
        /// <summary>
        /// Gets or sets the display name of the object.
        /// </summary>
        [Required]
        public string DisplayName { get; set; } = "";

        // Using 'required' to ensure these properties are set during object initialization
        /// <summary>
        /// Gets or sets the email address associated with the object.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Entered Email is incorrect", ErrorMessageResourceName = "Email")]
        public string Email { get; set; } = "";

        /// <summary>
        /// Gets or sets the hashed representation of the user's password.
        /// </summary>
        [Required]
        [MaxLength(16, ErrorMessage = "Password cannot exceed 16 characters.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = "";

        /// <summary>
        /// Gets or sets the cryptographic salt used for hashing the password.
        /// </summary>
    }
}

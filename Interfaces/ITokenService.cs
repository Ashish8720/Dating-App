using Dating_App.Entities;

namespace Dating_App.Interfaces
{
    // Interface for token generation service
    /// <summary>
    /// Defines a contract for generating authentication tokens for users.
    /// </summary>
    /// <remarks>Implementations of this interface are responsible for creating tokens that can be used  to
    /// authenticate users in the application. The generated token typically contains user-specific  claims and may have
    /// an expiration time.</remarks>
    public interface ITokenService
    {
        // Method to create a token for a given user
        string CreateToken(AppUser user);
    }
}

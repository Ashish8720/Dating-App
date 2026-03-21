namespace Dating_App.Error
{
    /// <summary>
    /// Represents an exception that occurs during API operations, providing details about the error.
    /// </summary>
    /// <remarks>This exception is typically used to encapsulate information about an error response from an
    /// API,  including the HTTP status code, a message describing the error, and optional additional details.</remarks>
    /// <param name="statusCode"></param>
    /// <param name="message"></param>
    /// <param name="details"></param>
    public class ApiException(int statusCode, string message, string? details)
    {
        // The HTTP status code associated with the error.
        public int StatusCode { get; set; } = statusCode;

        // A message describing the error.
        public string Message { get; set; } = message;

        // Optional additional details about the error.
        public string? Details { get; set; } = details;
    }
}

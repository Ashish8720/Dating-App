using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dating_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : BaseController
    {
        /// <summary>
        /// Returns an unauthorized response to indicate that the user is not authenticated.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing an HTTP 401 Unauthorized response.</returns>
        [HttpGet("auth")]
        public IActionResult GetAuth()
        {
            return Unauthorized();
        }

        /// <summary>
        /// Returns a 404 Not Found response.
        /// </summary>
        /// <remarks>This method is typically used to simulate or handle scenarios where a requested
        /// resource  cannot be found. It always returns an HTTP 404 status code.</remarks>
        /// <returns>An <see cref="IActionResult"/> representing the HTTP 404 Not Found response.</returns>
        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }


        /// <summary>
        /// Simulates a server error response for testing purposes.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing an unauthorized response.</returns>
        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            throw new Exception("This is the server error");
        }

        /// <summary>
        /// Returns a 400 Bad Request response.
        /// </summary>
        /// <remarks>This method is typically used to indicate that the request sent by the client is
        /// invalid  or cannot be processed due to client-side errors.</remarks>
        /// <returns>An <see cref="IActionResult"/> representing a 400 Bad Request response.</returns>
        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            return BadRequest();
        }
    }
}

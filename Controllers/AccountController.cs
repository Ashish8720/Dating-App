
using Dating_App.Data;
using Dating_App.DTOs;
using Dating_App.Entities;
using Dating_App.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace Dating_App.Controllers
{
    /// <summary>
    /// Provides endpoints for managing user accounts, including registration and authentication.
    /// </summary>
    /// <remarks>This controller is responsible for handling account-related operations such as user
    /// registration. It interacts with the <see cref="IAccountService"/> to perform the underlying business
    /// logic.</remarks>
    /// <param name="accountService"></param>
    public class AccountController(IAccountService accountService) : BaseController
    {
        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="register">An object containing the user's registration details, such as username, password, and other required
        /// information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ActionResult{T}"/>
        /// of type <see cref="UserDTO"/> representing the registered user details if the operation is successful.</returns>

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
        {
            try
            { 
                return Ok(await accountService.Register(register));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Authenticates a user based on the provided login credentials.
        /// </summary>
        /// <param name="login">An object containing the user's login credentials, such as email and password.</param>
        /// <returns>A <see cref="Task{ActionResult{UserDTO}}"/> representing the asynchronous operation.  If authentication is
        /// successful, returns an <see cref="OkObjectResult"/> containing the authenticated user's details as a <see
        /// cref="UserDTO"/>.  If authentication fails, returns an <see cref="UnauthorizedObjectResult"/> with an error
        /// message.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login)
        {
            try
            {
                var user = await accountService.Login(login);
                if (user == null) return Unauthorized("Invalid email or password");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Additional endpoints for login, logout, and other account-related operations can be added here.
    }
}

using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Models;
using MyBackendNemura.Dtos;
using Microsoft.AspNetCore.Identity;

namespace MyBackendNemura.Controllers.V1.Users;

[ApiController]
[Route("api/v1/users")]
// Defines an API controller in ASP.NET Core. This controller handles HTTP requests directed
// to the route "api/v1/users".
public class UsersPostController : ControllerBase
{
    // Properties to handle the database and other configurations.
    private readonly ApplicationDbContext Context;
    // `Context` is the property that represents access to the database,
    // allowing operations on it.

    private readonly IConfiguration _configuration;
    // `IConfiguration` allows access to configurations such as keys, database connection, or JWT.

    private readonly PasswordHasher<User> _passwordHasher;
    // `PasswordHasher` is used to hash (encrypt) the user's password before storing it.

    // Constructor that initializes the necessary dependencies for the controller.
    public UsersPostController(ApplicationDbContext context, IConfiguration configuration)
    {
        Context = context; // Inject the database context.
        _configuration = configuration; // Inject the configuration.
        _passwordHasher = new PasswordHasher<User>(); // Initialize the password hasher.
    }

    // POST method to register a new user.
    [HttpPost("Register")]
    // The [HttpPost] attribute indicates that this method will respond to HTTP POST requests at the "Register" route.
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        // Check if the model received in the request is valid.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // If not valid, return a 400 Bad Request error.
        }

        // Create a new `User` object from the data received from the DTO (Data Transfer Object).
        var user = new User
        {
            Name = userRegisterDto.Name,
            LastName = userRegisterDto.LastName,
            NickName = userRegisterDto.NickName,
            Email = userRegisterDto.Email,
            Password = userRegisterDto.Password // This password is not yet encrypted.
        };

        // Instantiate the password hasher for encryption.
        var passwordHash = new PasswordHasher<User>();

        // Encrypt the user's password before storing it in the database.
        user.Password = passwordHash.HashPassword(user, userRegisterDto.Password);

        // Add the new user to the database.
        Context.Users.Add(user);
        await Context.SaveChangesAsync(); // Save changes to the database asynchronously.

        return Ok("User has been successfully registered."); // Respond with a success message.
    }
}

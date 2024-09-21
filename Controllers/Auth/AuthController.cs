using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.Dtos.Auth;
using MyBackendNemura.Models;
using MyBackendNemura.DataBase;

namespace MyBackendNemura.Controllers.Auth;

[ApiController]
[Route("api/v1/auths")]
// Define un controlador de API en ASP.NET Core. Este controlador maneja las solicitudes HTTP dirigidas a la ruta "api/v1/auths".
public class AuthController : ControllerBase
{
    // Propiedad para acceder al contexto de la base de datos.
    // `Context` es la propiedad que representa el acceso a la base de datos, permitiendo realizar operaciones sobre ella.
    private readonly ApplicationDbContext Context;

    // Propiedad para acceder a la configuración de la aplicación.
    // `IConfiguration` permite acceder a configuraciones como claves, conexión a la base de datos o JWT.
    private readonly IConfiguration _configuration;

    // Propiedad para manejar el hashing de contraseñas.
    // `PasswordHasher` se utiliza para hashear (encriptar) la contraseña del usuario antes de almacenarla.
    private readonly PasswordHasher<User> _passwordHasher;

    // Constructor del controlador.
    // Inicializa el contexto de la base de datos, la configuración y el passwordHasher.
    public AuthController(ApplicationDbContext context, IConfiguration configuration)
    {
        Context = context; // Se inyecta el contexto de la base de datos.
        _configuration = configuration; // Se inyecta la configuración.
        _passwordHasher = new PasswordHasher<User>(); // Inicializa el hasher de contraseñas.
    }

    // Método POST que permite registrar un nuevo usuario.
    [HttpPost("Register")]
    // El atributo [HttpPost] indica que este método responderá a las solicitudes HTTP POST en la ruta "Register".
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        // Verificar si el modelo recibido en la solicitud es válido.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Si no es válido, devuelve un error 400 (Bad Request).
        }

        // Crear un nuevo objeto `User` a partir de los datos recibidos del DTO (Data Transfer Object).
        var user = new User
        {
            Name = registerDto.Name,
            LastName = registerDto.LastName,
            NickName = registerDto.NickName,
            Email = registerDto.Email,
            Password = registerDto.Password // Esta contraseña aún no está encriptada.
        };

        // Instanciar el hasher de contraseñas para aplicar encriptación.
        var passwordHash = new PasswordHasher<User>();

        // Encriptar la contraseña del usuario antes de almacenarla en la base de datos.
        user.Password = passwordHash.HashPassword(user, registerDto.Password);

        // Añadir el nuevo usuario a la base de datos.
        Context.Users.Add(user);
        await Context.SaveChangesAsync(); // Guardar los cambios en la base de datos de forma asíncrona.

        return Ok("User has been successfully registered."); // Responder con un mensaje de éxito.
    }

    // Método Post que permite loguear a un usuario.
    // Utiliza el atributo [HttpPost("Login")] para definir la ruta del endpoint.
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        // Verifica si el modelo del DTO es válido.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Busca al usuario en la base de datos por su NickName.
        var user = await Context.Users.FirstOrDefaultAsync(item => item.NickName == loginDto.NickName);
        if (user == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        // Verifica si la contraseña proporcionada coincide con la contraseña hasheada en la base de datos.
        var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);
        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Invalid credentials.");
        }

        // Si la autenticación es exitosa, genera un token JWT para el usuario.
        var token = GenerateJwtToken(user);

        // Devuelve una respuesta OK con los datos del usuario y el token JWT.
        return Ok(new
        {
            message = "Success",
            data = new
            {
                id = user.Id,
                name = user.Name,
                lastName = user.LastName,
                nickName = user.NickName,
                email = user.Email,
                token = token
            }
        });
    }

    // Método privado para generar el JWT.
    private string GenerateJwtToken(User user)
    {
        // Crea una clave de seguridad usando la clave secreta de la configuración.
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_KEY"]));

        // Crea las credenciales de firma usando la clave de seguridad y el algoritmo HMAC-SHA256.
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Define los claims que se incluirán en el token JWT.
        var claims = new[]
        {
            new Claim("Id", user.Id.ToString()), // Id del usuario.
            new Claim("Name", user.Name), // Nombre del usuario.
            new Claim("LastName", user.LastName), // Apellido del usuario.
            new Claim("NickName", user.NickName), // Apodo del usuario.
            new Claim("Email", user.Email) // Email del usuario.
        };

        // Crea el token JWT con los parámetros configurados.
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT_ISSUER"], // Emisor del token.
            audience: _configuration["JWT_AUDIENCE"], // Audiencia del token.
            claims: claims, // Claims que se incluirán en el token.
            expires: DateTime.Now.AddMinutes(double.Parse(_configuration["JWT_EXPIREMINUTES"])), // Tiempo de expiración del token.
            signingCredentials: credentials // Credenciales para la firma del token.
        );

        // Devuelve el token JWT como una cadena.
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

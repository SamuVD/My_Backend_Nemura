using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.Dtos;
using MyBackendNemura.Models;
using MyBackendNemura.DataBase;

namespace MyBackendNemura.Controllers.Auth;

[ApiController]
[Route("api/v1/users")]
public class AuthsPostController : ControllerBase
{
    // Propiedad para acceder al contexto de la base de datos.
    private readonly ApplicationDbContext Context;

    // Propiedad para acceder a la configuración de la aplicación.
    private readonly IConfiguration _configuration;

    // Propiedad para manejar el hashing de contraseñas.
    private readonly PasswordHasher<User> _passwordHasher;

    // Constructor del controlador.
    // Inicializa el contexto de la base de datos, la configuración y el passwordHasher.
    public AuthsPostController(ApplicationDbContext context, IConfiguration configuration)
    {
        Context = context;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    // Método para iniciar sesión de un usuario.
    // Utiliza el atributo [HttpPost("Login")] para definir la ruta del endpoint.
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        // Verifica si el modelo del DTO es válido.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Busca al usuario en la base de datos por su NickName.
        var user = await Context.Users.FirstOrDefaultAsync(item => item.NickName == userLoginDto.NickName);
        if (user == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        // Verifica si la contraseña proporcionada coincide con la contraseña hasheada en la base de datos.
        var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.Password, userLoginDto.Password);
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

using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Models;
using MyBackendNemura.Dtos;
using Microsoft.AspNetCore.Identity;

namespace MyBackendNemura.Controllers.V1.Users;

[ApiController]
[Route("api/v1/users")]
// Define un controlador de API en ASP.NET Core. Este controlador maneja las solicitudes HTTP dirigidas
// a la ruta "api/v1/users".

public class UsersPostController : ControllerBase
{
    // Propiedades para manejar la base de datos y otras configuraciones.
    private readonly ApplicationDbContext Context;
    // `Context` es la propiedad que representa el acceso a la base de datos, 
    // permitiendo realizar operaciones sobre ella.

    private readonly IConfiguration _configuration;
    // `IConfiguration` permite acceder a configuraciones como claves, conexión a la base de datos o JWT.

    private readonly PasswordHasher<User> _passwordHasher;
    // `PasswordHasher` se utiliza para hashear (encriptar) la contraseña del usuario antes de almacenarla.

    // Constructor que inicializa las dependencias necesarias para el controlador.
    public UsersPostController(ApplicationDbContext context, IConfiguration configuration)
    {
        Context = context; // Se inyecta el contexto de la base de datos.
        _configuration = configuration; // Se inyecta la configuración.
        _passwordHasher = new PasswordHasher<User>(); // Inicializa el hasher de contraseñas.
    }

    // Método POST que permite registrar un nuevo usuario.
    [HttpPost("Register")]
    // El atributo [HttpPost] indica que este método responderá a las solicitudes HTTP POST en la ruta "Register".
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        // Verificar si el modelo recibido en la solicitud es válido.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Si no es válido, devuelve un error 400 (Bad Request).
        }

        // Crear un nuevo objeto `User` a partir de los datos recibidos del DTO (Data Transfer Object).
        var user = new User
        {
            Name = userRegisterDto.Name,
            LastName = userRegisterDto.LastName,
            NickName = userRegisterDto.NickName,
            Email = userRegisterDto.Email,
            Password = userRegisterDto.Password // Esta contraseña aún no está encriptada.
        };

        // Instanciar el hasher de contraseñas para aplicar encriptación.
        var passwordHash = new PasswordHasher<User>();

        // Encriptar la contraseña del usuario antes de almacenarla en la base de datos.
        user.Password = passwordHash.HashPassword(user, userRegisterDto.Password);

        // Añadir el nuevo usuario a la base de datos.
        Context.Users.Add(user);
        await Context.SaveChangesAsync(); // Guardar los cambios en la base de datos de forma asíncrona.

        return Ok("User has been successfully registered."); // Responder con un mensaje de éxito.
    }
}

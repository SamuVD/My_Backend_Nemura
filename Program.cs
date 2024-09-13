// Importamos los paquetes necesarios de Entity Framework Core, DotNetEnv (para manejar variables de entorno)
// y otros paquetes relacionados con autenticación, JWT, y seguridad.
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using MyBackendNemura.DataBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Creamos el constructor del proyecto, `builder`, que permite configurar servicios y características.
var builder = WebApplication.CreateBuilder(args);

// Cargar las variables de entorno desde un archivo .env.
Env.Load();

// Agregar variables de entorno al sistema de configuración del proyecto.
builder.Configuration.AddEnvironmentVariables();

// Aquí obtenemos las variables de entorno para conectarse a la base de datos.
// Estas variables deberían estar definidas en el entorno o en un archivo .env.
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbDatabaseName = Environment.GetEnvironmentVariable("DB_DATABASE");
var dbUser = Environment.GetEnvironmentVariable("DB_USERNAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

// Construimos la cadena de conexión a MySQL usando las variables de entorno.
// Esta cadena incluye el host, puerto, nombre de la base de datos, usuario y contraseña.
var mySqlConnection = $"server={dbHost};port={dbPort};database={dbDatabaseName};uid={dbUser};password={dbPassword}";

// Registramos el contexto de la base de datos (DbContext) en los servicios del proyecto.
// Aquí se está utilizando MySQL como el motor de base de datos y se define la versión del servidor MySQL.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.Parse("8.0.20-mysql")));

// Obtenemos las variables de entorno necesarias para configurar la autenticación JWT.
// Estas variables deben contener la clave de seguridad, el emisor, la audiencia y el tiempo de expiración del token JWT.
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
var jwtExpireMinutes = Environment.GetEnvironmentVariable("JWT_EXPIREMINUTES");

// Configuramos la autenticación JWT en los servicios del proyecto.
// Se especifica que se va a usar el esquema de autenticación JWT para autenticar y desafiar (challenge) solicitudes.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Configuración específica de JWT, que incluye validaciones como emisor, audiencia, 
// tiempo de vida del token, y la clave de seguridad usada para firmar los tokens.
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;  // No requiere HTTPS para el token (opcional).
    options.SaveToken = true;  // Guarda el token de autenticación.
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,  // Valida que el emisor del token sea correcto.
        ValidateAudience = true,  // Valida que la audiencia del token sea correcta.
        ValidateLifetime = true,  // Valida que el token no esté expirado.
        ValidateIssuerSigningKey = true,  // Valida que la clave usada para firmar el token sea válida.
        ValidIssuer = jwtIssuer,  // Define el emisor válido.
        ValidAudience = jwtAudience,  // Define la audiencia válida.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))  // Define la clave de firma del token.
    };
});

// Añadimos el servicio de autorización, que se va a usar para restringir el acceso a ciertos endpoints.
builder.Services.AddAuthorization();

// Configuramos las políticas de CORS (Cross-Origin Resource Sharing).
// Esto permite que solo ciertos orígenes (dominios) puedan hacer peticiones a nuestra API.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",  // Nombre de la política de CORS.
        builder =>
        {
            // Permite solicitudes de los orígenes http://127.0.0.1:5173 y http://localhost:5173.
            // También permite cualquier tipo de encabezado y método HTTP (GET, POST, etc.).
            // `AllowCredentials` permite que las credenciales (cookies, headers de autenticación) se envíen.
            builder.WithOrigins("http://127.0.0.1:5173", "http://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

// Añadimos soporte para controladores (MVC o API Controllers).
builder.Services.AddControllers();

// Configuramos Swagger, que es una herramienta que genera documentación interactiva para la API.
// OpenAPI es una especificación estándar para la documentación de APIs.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Construimos la aplicación y comenzamos a configurar el pipeline de procesamiento de solicitudes.
var app = builder.Build();

// Aquí se configura Swagger en el pipeline, que permitirá a los usuarios ver la documentación de la API.
app.UseSwagger();
app.UseSwaggerUI();

// Configura CORS para que las políticas definidas antes se apliquen en las solicitudes.
app.UseCors("AllowSpecificOrigin");

// Habilita la autenticación en el pipeline de la aplicación, es decir, cada solicitud pasará por el proceso de autenticación JWT.
app.UseAuthentication(); // Agregar autenticación

// Habilita la autorización, que se usará para asegurar que solo los usuarios con permisos accedan a ciertos recursos.
app.UseAuthorization(); // Agregar autorización

// Mapea los controladores definidos en la API para que respondan a las rutas HTTP.
app.MapControllers();

// Ejecuta la aplicación, iniciando el servidor y comenzando a escuchar solicitudes.
app.Run();

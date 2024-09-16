using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Project;
using MyBackendNemura.Models;

namespace MyBackendNemura.Controllers.V1.Projects;

[Authorize] // Atributo para proteger el Endpoint
[ApiController]
[Route("api/v1/projects")]
public class ProjectsPostController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public ProjectsPostController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método crea un nuevo proyecto en la base de datos.
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProjectPostDto projectPostDto)
    {
        // Verificamos si el modelo recibido es válido. Si no es válido, devolvemos un código 400 Bad Request junto con los detalles de la validación.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Creamos una nueva instancia de 'Project' y asignamos los valores del DTO a las propiedades del modelo.
        var project = new Project
        {
            Name = projectPostDto.Name,   // Nombre del proyecto
            UserId = projectPostDto.UserId // ID del usuario asociado al proyecto
        };

        // Agregamos el nuevo proyecto al contexto de la base de datos.
        Context.Projects.Add(project);

        // Guardamos los cambios en la base de datos de forma asíncrona.
        await Context.SaveChangesAsync();

        // Devolvemos una respuesta con un estado 200 OK indicando que el proyecto ha sido creado exitosamente.
        return Ok("Project has been successfully created.");
    }
}

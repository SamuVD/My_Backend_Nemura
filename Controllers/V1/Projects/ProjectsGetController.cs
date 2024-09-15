// Importamos las librerías necesarias para trabajar con controladores, Entity Framework, la Data Base y los DTOs.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Project;

namespace MyBackendNemura.Controllers.V1.Projects;

// Definimos el controlador para manejar las solicitudes relacionadas con la obtención de proyectos.
[ApiController]
[Route("api/v1/projects")]
public class ProjectsGetController : ControllerBase
{
    // Esta propiedad se utiliza para interactuar con la base de datos.
    private readonly ApplicationDbContext Context;

    // Constructor del controlador, donde inyectamos el contexto de la base de datos.
    // El contexto permite realizar operaciones sobre la base de datos.
    public ProjectsGetController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Método para manejar solicitudes HTTP GET. Este método devuelve todos los proyectos junto con su UserId.
    [HttpGet]
    public async Task<ActionResult<List<ProjectGetDto>>> Get()
    {
        // 1. Consulta a la base de datos para obtener todos los proyectos, seleccionando solo los campos relevantes.
        var projects = await Context.Projects
            .Select(project => new ProjectGetDto
            {
                Id = project.Id,       // ID del proyecto
                Name = project.Name,   // Nombre del proyecto
                UserId = project.UserId // ID del usuario relacionado con el proyecto
            }).ToListAsync();

        // 2. Verificamos si la lista de proyectos está vacía (null). 
        // Si no hay proyectos, devolvemos un código 204 No Content.
        if (projects == null)
        {
            return NoContent(); // No hay proyectos disponibles.
        }

        // 3. Si encontramos proyectos, devolvemos la lista con un código 200 OK.
        return Ok(projects); // Devolvemos los proyectos.
    }
}

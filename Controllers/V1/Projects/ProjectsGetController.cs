using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Project;
using MyBackendNemura.Models;

namespace MyBackendNemura.Controllers.V1.Projects;

[ApiController]
[Route("api/v1/projects")]
public class ProjectsGetController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public ProjectsGetController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método se va a encargar de traer todos los proyectos. Y dentro de cada proyecto va a estar el UserId con el que tiene relación.
    [HttpGet]
    public async Task<ActionResult<List<ProjectGetDto>>> Get()
    {
        // 1. Consulta a la base de datos para obtener todos los proyectos
        var projects = await Context.Projects
            .Select(project => new ProjectGetDto
            {
                Name = project.Name,
                UserId = project.UserId
            }).ToListAsync();

        // 2. Verificamos si no hay proyectos
        if (projects == null || projects.Count == 0)
        {
            return NoContent(); // Devolvemos un código 204 No Content si no hay proyectos
        }

        // 3. Devolvemos la lista de proyectos con un código 200 OK
        return Ok(projects);
    }
}

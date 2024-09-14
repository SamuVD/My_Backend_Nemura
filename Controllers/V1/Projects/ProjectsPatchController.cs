using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Project;

namespace MyBackendNemura.Controllers.V1.Projects;

[ApiController]
[Route("api/v1/projects")]
public class ProjectsUpdateController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public ProjectsUpdateController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método se encargará de actualizar el nombre de un proyecto.
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, ProjectPatchDto projectPatchDto)
    {
        // Buscamos el proyecto por su ID.
        var project = await Context.Projects.FindAsync(id);
        
        // Si el proyecto no existe, devolvemos un mensaje de error.
        if (project == null)
        {
            return NotFound("The project was not found.");
        }
        
        project.Name = projectPatchDto.Name;

        await Context.SaveChangesAsync();
        return Ok("Project updated successfully.");
    }
}

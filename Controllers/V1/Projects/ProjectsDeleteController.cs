using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;

namespace MyBackendNemura.Controllers.V1.Projects;

[ApiController]
[Route("api/v1/projects")]
public class ProjectsDeleteController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public ProjectsDeleteController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método va a ser llamado cuando se quiera borrar un proyecto por el Id.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        // Primero, buscamos el proyecto en la base de datos.
        var projectToRemove = await Context.Projects.FindAsync(id);

        // Si el proyecto no existe, devolvemos un mensaje de error.
        if (projectToRemove == null)
        {
            return NotFound("The project was not found.");
        }

        // Si el proyecto existe, lo eliminamos de la base de datos.
        Context.Projects.Remove(projectToRemove);
        await Context.SaveChangesAsync();

        // Devolvemos un mensaje de confirmación.
        return Ok("The project was deleted.");
    }
}

// Importamos las librerías necesarias para trabajar con Autorizaciones, controladores y el acceso a la base de datos.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;

namespace MyBackendNemura.Controllers.V1.Projects;

// Definimos el controlador para manejar las solicitudes relacionadas con la eliminación de proyectos.
//[Authorize] // Atributo para proteger el Endpoint
[ApiController]
[Route("api/v1/projects")]
public class ProjectsDeleteController : ControllerBase
{
    // Esta propiedad se utiliza para interactuar con la base de datos.
    private readonly ApplicationDbContext Context;

    // Constructor del controlador, donde inyectamos la instancia del contexto de la base de datos.
    // El contexto es necesario para realizar operaciones CRUD sobre la base de datos.
    public ProjectsDeleteController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Método para manejar solicitudes HTTP DELETE. Este método elimina un proyecto específico usando su ID.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        // Buscamos el proyecto en la base de datos por su ID. Si no lo encontramos, 'projectToRemove' será null.
        var projectToRemove = await Context.Projects.FindAsync(id);

        // Si no encontramos el proyecto, devolvemos una respuesta con un estado 404 (Not Found).
        if (projectToRemove == null)
        {
            return NotFound("The project was not found.");
        }

        // Si encontramos el proyecto, lo eliminamos del contexto de la base de datos.
        Context.Projects.Remove(projectToRemove);

        // Guardamos los cambios en la base de datos de forma asíncrona.
        await Context.SaveChangesAsync();

        // Devolvemos una respuesta con un estado 200 (OK) indicando que el proyecto fue eliminado con éxito.
        return Ok("The project was deleted.");
    }
}

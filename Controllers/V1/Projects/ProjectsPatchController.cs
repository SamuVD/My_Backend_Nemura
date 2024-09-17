// Importamos las librerías necesarias para trabajar con Autorizaciones, controladores y el acceso a la base de datos, y los DTOS.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Project;

namespace MyBackendNemura.Controllers.V1.Projects;

// Definimos el controlador para manejar las solicitudes relacionadas con la actualización parcial de proyectos.
[Authorize] // Atributo para proteger el Endpoint
[ApiController]
[Route("api/v1/projects")]
public class ProjectsPatchController : ControllerBase
{
    // Esta propiedad se utiliza para interactuar con la base de datos.
    private readonly ApplicationDbContext Context;

    // Constructor del controlador, donde inyectamos la instancia del contexto de la base de datos.
    // El contexto es necesario para realizar operaciones CRUD sobre la base de datos.
    public ProjectsPatchController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Método para manejar solicitudes HTTP PATCH. Este método actualiza el nombre de un proyecto específico usando su ID.
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch([FromRoute] int id, ProjectPatchDto projectPatchDto)
    {
        // Buscamos el proyecto en la base de datos por su ID. Si no lo encontramos, 'project' será null.
        var project = await Context.Projects.FindAsync(id);

        // Si no encontramos el proyecto, devolvemos una respuesta con un estado 404 (Not Found).
        if (project == null)
        {
            return NotFound("The project was not found.");
        }

        // Actualizamos el nombre del proyecto con el nuevo valor proporcionado en el DTO.
        project.Name = projectPatchDto.Name;

        // Guardamos los cambios en la base de datos de forma asíncrona.
        await Context.SaveChangesAsync();

        // Devolvemos una respuesta con un estado 200 (OK) indicando que el proyecto se actualizó exitosamente.
        return Ok("Project updated successfully.");
    }
}

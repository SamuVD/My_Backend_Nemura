using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Assignment;
using MyBackendNemura.Models;
using MyBackendNemura.Enums;

namespace MyBackendNemura.Controllers.V1.Assignments;

[Authorize] // Atributo para proteger el Endpoint
[ApiController]
[Route("api/v1/assignments")]
public class AssignmentsPostController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public AssignmentsPostController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método se encargará de crear una nueva tarea.
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AssignmentPostDto assignmentPostDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // Convertir el valor de Status a Enum
        // Intenta convertir la cadena recibida en el DTO a un valor del enum AssignmentStatus.
        // Si la conversión falla, devuelve un error 400 con un mensaje de valor de estado inválido.
        // if (!Enum.TryParse(assignmentPostDto.Status, true, out AssignmentStatus status))
        // {
        //     return BadRequest("Invalid status value.");
        // }

        // Convertir el valor de Priority a Enum
        // Intenta convertir la cadena recibida en el DTO a un valor del enum AssignmentPriority.
        // Si la conversión falla, devuelve un error 400 con un mensaje de valor de prioridad inválido.
        // if (!Enum.TryParse(assignmentPostDto.Priority, true, out AssignmentPriority priority))
        // {
        //     return BadRequest("Invalid priority value.");
        // }

        // Busca el proyecto en la base de datos usando el ID recibido en el DTO.
        // Si no se encuentra el proyecto, devuelve un error 404 con un mensaje de proyecto no encontrado.
        var project = await Context.Projects.FindAsync(assignmentPostDto.ProjectId);
        if (project == null)
        {
            return NotFound("Project not found");
        }

        // Crea una nueva instancia de Assignment usando los valores del DTO y los enums convertidos.
        var assignment = new Assignment
        {
            Name = assignmentPostDto.Name,
            Description = assignmentPostDto.Description,
            Status = assignmentPostDto.Status, // Aquí el enum ya está siendo manejado
            Priority = assignmentPostDto.Priority, // Al igual que el de prioridad
            ProjectId = assignmentPostDto.ProjectId,
            Project = project
        };

        // Agrega la nueva tarea al contexto y guarda los cambios en la base de datos.
        Context.Assignments.Add(assignment);
        await Context.SaveChangesAsync();

        // Devuelve un mensaje de éxito con estado 200 OK.
        return Ok("Assignment has been created successfully.");
    }
}

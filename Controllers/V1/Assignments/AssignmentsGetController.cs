// Importamos las librerías necesarias para trabajar con Autorizaciones, controladores, Entity Framework y los DTOs.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Assignment;
using Microsoft.EntityFrameworkCore;

namespace MyBackendNemura.Controllers.V1.Assignments;

// Definimos el controlador para manejar las solicitudes relacionadas con la obtención de tareas.
[Authorize] // Atributo para proteger el Endpoint
[ApiController]
[Route("api/v1/assignments")]
public class AssignmentsGetController : ControllerBase
{
    // Esta propiedad se utiliza para interactuar con la base de datos.
    private readonly ApplicationDbContext Context;

    // Constructor del controlador, donde inyectamos la instancia del contexto de la base de datos.
    // El contexto permite realizar operaciones sobre la base de datos.
    public AssignmentsGetController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Método para manejar solicitudes HTTP GET. Este método devuelve todas las tareas junto con el ID del proyecto al que pertenecen.
    [HttpGet]
    public async Task<ActionResult<List<AssignmentGetDto>>> Get()
    {
        // Consulta para obtener todas las tareas desde la base de datos y proyectarlas en el DTO.
        var assignments = await Context.Assignments.Select(
            assignment => new AssignmentGetDto
            {
                Id = assignment.Id,                   // ID de la tarea
                Name = assignment.Name,               // Nombre de la tarea
                Description = assignment.Description, // Descripción de la tarea
                Status = assignment.Status.ToString(), // Estado de la tarea
                Priority = assignment.Priority.ToString(), // Prioridad de la tarea
                ProjectId = assignment.ProjectId      // ID del proyecto asociado
            }).ToListAsync();

        // Verificamos si la lista de tareas está vacía. 
        // Si no hay tareas, devolvemos un código 204 No Content.
        if (assignments == null)
        {
            return NoContent(); // No hay tareas disponibles.
        }

        // Devolvemos la lista de tareas con un estado 200 OK.
        return Ok(assignments); // Tareas encontradas.
    }

    // Método para manejar solicitudes HTTP GET. Este método devuelve una tarea específica usando su ID.
    [HttpGet("ById/{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        // Buscamos la tarea en la base de datos por su ID. Si no la encontramos, 'assignmentFound' será null.
        var assignmentFound = await Context.Assignments.FindAsync(id);

        // Si no encontramos la tarea, devolvemos una respuesta con un estado 404 (Not Found).
        if (assignmentFound == null)
        {
            return NotFound("Assignment not found.");
        }

        // Consultamos nuevamente para proyectar la tarea encontrada en una forma anónima.
        var assignments = await Context.Assignments.Select(
            assignment => new
            {
                assignment.Id,                   // ID de la tarea
                assignment.Name,                 // Nombre de la tarea
                assignment.Description,          // Descripción de la tarea
                assignment.Status,               // Estado de la tarea
                assignment.Priority,             // Prioridad de la tarea
                assignment.ProjectId             // ID del proyecto asociado
            }
        ).ToListAsync();

        // Devolvemos la tarea encontrada con un estado 200 OK.
        return Ok(assignments); // Tarea encontrada.
    }

    // Método para manejar solicitudes HTTP GET. Este método devuelve todas las tareas asociadas a un proyecto específico usando el ID del proyecto.
    [HttpGet("ByProjectId/{id}")]
    public async Task<IActionResult> GetAssignmentsByProjectId(int id)
    {
        // Consultamos la base de datos para obtener todas las tareas asociadas al proyecto con el ID proporcionado.
        var assignments = await Context.Assignments
                                       .Where(assignment => assignment.ProjectId == id)
                                       .Select(assignment => new
                                       {
                                           assignment.Id,                   // ID de la tarea
                                           assignment.Name,                 // Nombre de la tarea
                                           assignment.Description,          // Descripción de la tarea
                                           assignment.Status,               // Estado de la tarea
                                           assignment.Priority,             // Prioridad de la tarea
                                           assignment.ProjectId             // ID del proyecto asociado
                                       }).ToListAsync();

        // Verificamos si la lista de tareas está vacía. 
        // Si no hay tareas asociadas al proyecto, devolvemos una respuesta con un estado 404 (Not Found).
        if (assignments == null)
        {
            return NotFound("No tasks found for the specified project.");
        }

        // Devolvemos la lista de tareas asociadas al proyecto con un estado 200 OK.
        return Ok(assignments); // Tareas encontradas para el proyecto.
    }
}

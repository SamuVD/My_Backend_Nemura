using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Assignment;
using Microsoft.EntityFrameworkCore;

namespace MyBackendNemura.Controllers.V1.Assignments;

[ApiController]
[Route("api/v1/assignments")]
public class AssignmentsGetController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public AssignmentsGetController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método se encargará de traer todas las tareas, y cada una tendrá el Id del proyecto al que pertenece.
    [HttpGet]
    public async Task<ActionResult<List<AssignmentGetDto>>> Get()
    {
        // Consulta para obtener todas las tareas y proyectarlas en el DTO
        var assignments = await Context.Assignments.Select(
            assignment => new AssignmentGetDto
            {
                Id = assignment.Id,
                Name = assignment.Name,
                Description = assignment.Description,
                Status = assignment.Status.ToString(),
                Priority = assignment.Priority.ToString(),
                ProjectId = assignment.ProjectId
            }).ToListAsync();

        // Verificar si hay tareas
        if (assignments == null)
        {
            return NoContent(); // Devolver 204 No Content si no se encontraron tareas.
        }

        return Ok(assignments); // Devolver la lista de tareas en la respuesta con 200 OK.
    }

    // Este método me traerá a una tarea por el Id.
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var assignmentFound = await Context.Assignments.FindAsync(id);

        if (assignmentFound == null)
        {
            return NotFound("Assignment not found.");
        }

        var assignments = await Context.Assignments.Select(
            assignment => new 
            {
                assignment.Id,
                assignment.Name,
                assignment.Description,
                assignment.Status,
                assignment.Priority,
                assignment.ProjectId
            }
        ).ToListAsync();

        return Ok(assignments);
    }

    // Este método me traerá todas las tareas enlazadadas a un solo proyecto. Por el Id del proyecto.
    [HttpGet("GetByProjectId/{id}")]
    public async Task<IActionResult> GetAssignmentsByProjectId(int id)
    {
        var assignments = await Context.Assignments
                                       .Where(assignment => assignment.ProjectId == id)
                                       .Select(assignment => new
                                       {
                                           assignment.Id,
                                           assignment.Name,
                                           assignment.Description,
                                           assignment.Status,
                                           assignment.Priority,
                                           assignment.ProjectId
                                       }).ToListAsync();

        if (assignments == null)
        {
            return NotFound("No tasks found for the specified project.");
        }

        return Ok(assignments);
    }
}

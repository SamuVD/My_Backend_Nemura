using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Assignment;
using MyBackendNemura.Models;
using Microsoft.EntityFrameworkCore;
using MyBackendNemura.Enums;

namespace MyBackendNemura.Controllers.V1.Assignments;

[ApiController]
[Route("api/v1/assignments")]
public class AssignmentsPutController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public AssignmentsPutController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método se encargará de actualizar varias propiedades de una tarea.
    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute] int id, AssignmentPutDto assignmentPutDto)
    {
        var assignmentFound = await Context.Assignments.FindAsync(id);

        if (assignmentFound == null)
        {
            return NotFound("Assignment not found.");
        }

        // Convertir el valor de Status a Enum
        // Intenta convertir la cadena recibida en el DTO a un valor del enum AssignmentStatus.
        // Si la conversión falla, devuelve un error 400 con un mensaje de valor de estado inválido.
        if (!Enum.TryParse(assignmentPutDto.Status, true, out AssignmentStatus status))
        {
            return BadRequest("Invalid status value.");
        }

        // Convertir el valor de Priority a Enum
        // Intenta convertir la cadena recibida en el DTO a un valor del enum AssignmentPriority.
        // Si la conversión falla, devuelve un error 400 con un mensaje de valor de prioridad inválido.
        if (!Enum.TryParse(assignmentPutDto.Priority, true, out AssignmentPriority priority))
        {
            return BadRequest("Invalid priority value.");
        }

        var assignments = await Context.Assignments.Select(
            assignment => new Assignment
            {
                Name = assignmentPutDto.Name,
                Description = assignmentPutDto.Description,
                Status = status,
                Priority = priority,
            }
        ).ToListAsync();

        await Context.SaveChangesAsync();
        return Ok("Assignment has been updated succesfully.");
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Assignment;
using MyBackendNemura.Enums;

namespace MyBackendNemura.Controllers.V1.Assignments;

[Authorize]
[ApiController]
[Route("api/v1/assignments")]
public class AssignmentsPatchController : ControllerBase
{
    // Esta propiedad se utiliza para interactuar con la base de datos.
    private readonly ApplicationDbContext Context;

    // Constructor del controlador, donde inyectamos la instancia del contexto de la base de datos.
    // El contexto permite realizar operaciones CRUD sobre la base de datos.
    public AssignmentsPatchController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método va a actualizar el enum Status de las assignments.
    [HttpPatch("status/{id}")]
    public async Task<IActionResult> Patch([FromRoute] int id, AssignmentPatchStatusDto assignmentPatchStatusDto)
    {
        var assignmentFound = await Context.Assignments.FindAsync(id);

        if (assignmentFound == null)
        {
            return NotFound("Assignment not found.");
        }

        // Convertir el valor de Status a Enum
        // Intentamos convertir la cadena recibida en el DTO a un valor del enum AssignmentStatus.
        // Si la conversión falla, devolvemos un error con código 400 Bad Request y un mensaje de valor de estado inválido.
        // if (!Enum.TryParse(assignmentPatchStatusDto.Status, true, out AssignmentStatus status))
        // {
        //     return BadRequest("Invalid status value.");
        // }

        assignmentFound.Status = assignmentPatchStatusDto.Status;

        await Context.SaveChangesAsync();
        return Ok("Status has been updated succesfully.");
    }

    // Este método va a actualizar el enum Status de las assignments.
    [HttpPatch("priority/{id}")]
    public async Task<IActionResult> Patch([FromRoute] int id, AssignmentPatchPriorityDto assignmentPatchPriorityDto)
    {
        var assignmentFound = await Context.Assignments.FindAsync(id);

        if (assignmentFound == null)
        {
            return NotFound("Assignment not found.");
        }

        // Convertir el valor de Status a Enum
        // Intentamos convertir la cadena recibida en el DTO a un valor del enum AssignmentStatus.
        // Si la conversión falla, devolvemos un error con código 400 Bad Request y un mensaje de valor de estado inválido.
        // if (!Enum.TryParse(assignmentPatchStatusDto.Status, true, out AssignmentStatus status))
        // {
        //     return BadRequest("Invalid status value.");
        // }

        assignmentFound.Priority = assignmentPatchPriorityDto.Priority;

        await Context.SaveChangesAsync();
        return Ok("Priority has been updated succesfully.");
    }
}

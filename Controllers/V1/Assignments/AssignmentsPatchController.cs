using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Assignment;

namespace MyBackendNemura.Controllers.V1.Assignments;

[Authorize]
[ApiController]
[Route("api/v1/assignments")]
public class AssignmentsPatchController : ControllerBase
{
    // This property is used to interact with the database.
    private readonly ApplicationDbContext Context;

    // Controller constructor where we inject the database context instance.
    // The context allows CRUD operations on the database.
    public AssignmentsPatchController(ApplicationDbContext context)
    {
        Context = context;
    }

    // This method will update the Status enum of assignments.
    [HttpPatch("status/{id}")]
    public async Task<IActionResult> Patch([FromRoute] int id, AssignmentPatchStatusDto assignmentPatchStatusDto)
    {
        var assignmentFound = await Context.Assignments.FindAsync(id);

        if (assignmentFound == null)
        {
            return NotFound("Assignment not found.");
        }

        // Convert the Status value to Enum
        // Attempt to convert the string received in the DTO to an AssignmentStatus enum value.
        // If conversion fails, return a 400 Bad Request error with an invalid status value message.
        // if (!Enum.TryParse(assignmentPatchStatusDto.Status, true, out AssignmentStatus status))
        // {
        //     return BadRequest("Invalid status value.");
        // }

        assignmentFound.Status = assignmentPatchStatusDto.Status;

        await Context.SaveChangesAsync();
        return Ok("Status has been updated successfully.");
    }

    // This method will update the Priority enum of assignments.
    [HttpPatch("priority/{id}")]
    public async Task<IActionResult> Patch([FromRoute] int id, AssignmentPatchPriorityDto assignmentPatchPriorityDto)
    {
        var assignmentFound = await Context.Assignments.FindAsync(id);

        if (assignmentFound == null)
        {
            return NotFound("Assignment not found.");
        }

        // Convert the Priority value to Enum
        // Attempt to convert the string received in the DTO to an AssignmentPriority enum value.
        // If conversion fails, return a 400 Bad Request error with an invalid priority value message.
        // if (!Enum.TryParse(assignmentPatchPriorityDto.Priority, true, out AssignmentPriority priority))
        // {
        //     return BadRequest("Invalid priority value.");
        // }

        assignmentFound.Priority = assignmentPatchPriorityDto.Priority;

        await Context.SaveChangesAsync();
        return Ok("Priority has been updated successfully.");
    }
}

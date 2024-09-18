// Import the necessary libraries for working with Authorization, controllers, database, DTOs, and enums.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Assignment;

namespace MyBackendNemura.Controllers.V1.Assignments;

// Define the controller to handle requests related to task updates.
[Authorize] // Attribute to protect the Endpoint
[ApiController]
[Route("api/v1/assignments")]
public class AssignmentsPutController : ControllerBase
{
    // This property is used to interact with the database.
    private readonly ApplicationDbContext Context;

    // Controller constructor where we inject the database context instance.
    // The context allows CRUD operations on the database.
    public AssignmentsPutController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Method to handle HTTP PUT requests. This method updates various properties of a specific task using its ID.
    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute] int id, AssignmentPutDto assignmentPutDto)
    {
        // Search for the task in the database by its ID. If not found, 'assignmentFound' will be null.
        var assignmentFound = await Context.Assignments.FindAsync(id);

        // If the task is not found, return a 404 (Not Found) response.
        if (assignmentFound == null)
        {
            return NotFound("Assignment not found.");
        }

        // Convert the Status value to Enum
        // Attempt to convert the string received in the DTO to an AssignmentStatus enum value.
        // If conversion fails, return a 400 Bad Request error with an invalid status value message.
        // if (!Enum.TryParse(assignmentPutDto.Status, true, out AssignmentStatus status))
        // {
        //     return BadRequest("Invalid status value.");
        // }

        // Convert the Priority value to Enum
        // Attempt to convert the string received in the DTO to an AssignmentPriority enum value.
        // If conversion fails, return a 400 Bad Request error with an invalid priority value message.
        // if (!Enum.TryParse(assignmentPutDto.Priority, true, out AssignmentPriority priority))
        // {
        //     return BadRequest("Invalid priority value.");
        // }

        // Update the properties of the found task with the values provided in the DTO.
        assignmentFound.Name = assignmentPutDto.Name;
        assignmentFound.Description = assignmentPutDto.Description;
        assignmentFound.Status = assignmentPutDto.Status;       // Assign the converted status value
        assignmentFound.Priority = assignmentPutDto.Priority;   // Assign the converted priority value

        // Save the changes to the database asynchronously.
        await Context.SaveChangesAsync();

        // Return a 200 (OK) response indicating that the task has been successfully updated.
        return Ok("Assignment has been updated successfully.");
    }
}

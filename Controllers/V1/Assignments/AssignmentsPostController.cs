using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Assignment;
using MyBackendNemura.Models;
using MyBackendNemura.Enums;

namespace MyBackendNemura.Controllers.V1.Assignments;

[Authorize] // Attribute to protect the Endpoint
[ApiController]
[Route("api/v1/assignments")]
public class AssignmentsPostController : ControllerBase
{
    // This property is our key to access the database.
    private readonly ApplicationDbContext Context;

    // Builder. This constructor is responsible for connecting to the database with the help of the key.
    public AssignmentsPostController(ApplicationDbContext context)
    {
        Context = context;
    }

    // This method is responsible for creating a new task.
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AssignmentPostDto assignmentPostDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // Convert the Status value to Enum
        // Attempt to convert the string received in the DTO to an AssignmentStatus enum value.
        // If conversion fails, return a 400 Bad Request error with an invalid status value message.
        // if (!Enum.TryParse(assignmentPostDto.Status, true, out AssignmentStatus status))
        // {
        //     return BadRequest("Invalid status value.");
        // }

        // Convert the Priority value to Enum
        // Attempt to convert the string received in the DTO to an AssignmentPriority enum value.
        // If conversion fails, return a 400 Bad Request error with an invalid priority value message.
        // if (!Enum.TryParse(assignmentPostDto.Priority, true, out AssignmentPriority priority))
        // {
        //     return BadRequest("Invalid priority value.");
        // }

        // Search for the project in the database using the ID received in the DTO.
        // If the project is not found, return a 404 error with a project not found message.
        var project = await Context.Projects.FindAsync(assignmentPostDto.ProjectId);
        if (project == null)
        {
            return NotFound("Project not found");
        }

        // Create a new instance of Assignment using the values from the DTO and converted enums.
        var assignment = new Assignment
        {
            Name = assignmentPostDto.Name,
            Description = assignmentPostDto.Description,
            Status = assignmentPostDto.Status, // Here the enum is already being handled
            Priority = assignmentPostDto.Priority, // Likewise for priority
            ProjectId = assignmentPostDto.ProjectId,
            Project = project
        };

        // Add the new task to the context and save the changes to the database.
        Context.Assignments.Add(assignment);
        await Context.SaveChangesAsync();

        // Return a success message with a 200 OK status.
        return Ok("Assignment has been created successfully.");
    }
}

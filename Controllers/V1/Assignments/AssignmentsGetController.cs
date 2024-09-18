// Import the necessary libraries for working with Authorizations, controllers, Entity Framework, and DTOs.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Assignment;
using Microsoft.EntityFrameworkCore;

namespace MyBackendNemura.Controllers.V1.Assignments;

// Define the controller to handle requests related to fetching tasks.
[Authorize] // Attribute to protect the Endpoint
[ApiController]
[Route("api/v1/assignments")]
public class AssignmentsGetController : ControllerBase
{
    // This property is used to interact with the database.
    private readonly ApplicationDbContext Context;

    // Constructor of the controller, where we inject the database context instance.
    // The context allows performing operations on the database.
    public AssignmentsGetController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Method to handle HTTP GET requests. This method returns all tasks along with the ID of the project they belong to.
    [HttpGet]
    public async Task<ActionResult<List<AssignmentGetDto>>> Get()
    {
        // Query to retrieve all tasks from the database and project them into the DTO.
        var assignments = await Context.Assignments.Select(
            assignment => new AssignmentGetDto
            {
                Id = assignment.Id,                   // Task ID
                Name = assignment.Name,               // Task name
                Description = assignment.Description, // Task description
                Status = assignment.Status,           // Task status
                Priority = assignment.Priority,       // Task priority
                ProjectId = assignment.ProjectId      // Associated project ID
            }).ToListAsync();

        // Check if the list of tasks is empty. 
        // If no tasks are found, return a 204 No Content.
        if (assignments == null)
        {
            return NoContent(); // No tasks available.
        }

        // Return the list of tasks with a 200 OK status.
        return Ok(assignments); // Tasks found.
    }

    // Method to handle HTTP GET requests. This method returns a specific task using its ID.
    [HttpGet("ById/{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        // Search for the task in the database by its ID. If not found, 'assignmentFound' will be null.
        var assignmentFound = await Context.Assignments.FindAsync(id);

        // If the task is not found, return a 404 (Not Found) response.
        if (assignmentFound == null)
        {
            return NotFound("Assignment not found.");
        }

        // Query again to project the found task into an anonymous form.
        var assignments = await Context.Assignments.Select(
            assignment => new
            {
                assignment.Id,                   // Task ID
                assignment.Name,                 // Task name
                assignment.Description,          // Task description
                assignment.Status,               // Task status
                assignment.Priority,             // Task priority
                assignment.ProjectId             // Associated project ID
            }
        ).ToListAsync();

        // Return the found task with a 200 OK status.
        return Ok(assignments); // Task found.
    }

    // Method to handle HTTP GET requests. This method returns all tasks associated with a specific project using the project ID.
    [HttpGet("ByProjectId/{id}")]
    public async Task<IActionResult> GetAssignmentsByProjectId([FromRoute] int id)
    {
        // Query the database to retrieve all tasks associated with the project with the given ID.
        var assignments = await Context.Assignments
                                       .Where(assignment => assignment.ProjectId == id)
                                       .Select(assignment => new
                                       {
                                           assignment.Id,                   // Task ID
                                           assignment.Name,                 // Task name
                                           assignment.Description,          // Task description
                                           assignment.Status,               // Task status
                                           assignment.Priority,             // Task priority
                                           assignment.ProjectId             // Associated project ID
                                       }).ToListAsync();

        // Check if the list of tasks is empty. 
        // If no tasks are found for the project, return a 404 (Not Found) response.
        if (assignments == null)
        {
            return NotFound("No tasks found for the specified project.");
        }

        // Return the list of tasks associated with the project with a 200 OK status.
        return Ok(assignments); // Tasks found for the project.
    }
}

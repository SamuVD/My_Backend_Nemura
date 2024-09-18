// Import necessary libraries for working with authorizations, controllers, and database access, and DTOs.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Project;

namespace MyBackendNemura.Controllers.V1.Projects;

// Define the controller to handle requests related to partial project updates.
[Authorize] // Attribute to protect the endpoint
[ApiController]
[Route("api/v1/projects")]
public class ProjectsPatchController : ControllerBase
{
    // This property is used to interact with the database.
    private readonly ApplicationDbContext Context;

    // Constructor of the controller, where we inject the database context instance.
    // The context is necessary to perform CRUD operations on the database.
    public ProjectsPatchController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Method to handle HTTP PATCH requests. This method updates the name of a specific project using its ID.
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch([FromRoute] int id, ProjectPatchDto projectPatchDto)
    {
        // Find the project in the database by its ID. If not found, 'project' will be null.
        var project = await Context.Projects.FindAsync(id);

        // If the project is not found, return a 404 (Not Found) response.
        if (project == null)
        {
            return NotFound("The project was not found.");
        }

        // Update the project name with the new value provided in the DTO.
        project.Name = projectPatchDto.Name;

        // Save changes to the database asynchronously.
        await Context.SaveChangesAsync();

        // Return a 200 (OK) response indicating that the project was updated successfully.
        return Ok("Project updated successfully.");
    }
}

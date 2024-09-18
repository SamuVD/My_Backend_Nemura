// Import the necessary libraries for working with Authorization, controllers, and database access.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;

namespace MyBackendNemura.Controllers.V1.Projects;

// Define the controller to handle requests related to project deletion.
[Authorize] // Attribute to protect the Endpoint
[ApiController]
[Route("api/v1/projects")]
public class ProjectsDeleteController : ControllerBase
{
    // This property is used to interact with the database.
    private readonly ApplicationDbContext Context;

    // Controller constructor where we inject the database context instance.
    // The context is needed to perform CRUD operations on the database.
    public ProjectsDeleteController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Method to handle HTTP DELETE requests. This method deletes a specific project using its ID.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        // Search for the project in the database by its ID. If not found, 'projectToRemove' will be null.
        var projectToRemove = await Context.Projects.FindAsync(id);

        // If the project is not found, return a 404 (Not Found) response.
        if (projectToRemove == null)
        {
            return NotFound("The project was not found.");
        }

        // If the project is found, remove it from the database context.
        Context.Projects.Remove(projectToRemove);

        // Save the changes to the database asynchronously.
        await Context.SaveChangesAsync();

        // Return a 200 (OK) response indicating that the project was successfully deleted.
        return Ok("The project was deleted.");
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Project;
using MyBackendNemura.Models;

namespace MyBackendNemura.Controllers.V1.Projects;

[Authorize] // Attribute to protect the endpoint
[ApiController]
[Route("api/v1/projects")]
public class ProjectsPostController : ControllerBase
{
    // This property is our key to access the database.
    private readonly ApplicationDbContext Context;

    // Builder. This constructor will handle connecting to the database with the help of the key.
    public ProjectsPostController(ApplicationDbContext context)
    {
        Context = context;
    }

    // This method creates a new project in the database.
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProjectPostDto projectPostDto)
    {
        // Check if the received model is valid. If not, return a 400 Bad Request status with validation details.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Create a new instance of 'Project' and assign the DTO values to the model properties.
        var project = new Project
        {
            Name = projectPostDto.Name,   // Project name
            UserId = projectPostDto.UserId // ID of the user associated with the project
        };

        // Add the new project to the database context.
        Context.Projects.Add(project);

        // Save changes to the database asynchronously.
        await Context.SaveChangesAsync();

        // Return a 200 OK response indicating that the project was successfully created.
        return Ok("Project has been successfully created.");
    }
}

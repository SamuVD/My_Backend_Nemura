using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;

namespace MyBackendNemura.Controllers.V1.Assignments;

[Authorize] // Attribute to protect the Endpoint
[ApiController]
[Route("api/[controller]")]
public class AssignmentsDeleteController : ControllerBase
{
    // This property is our key to access the database.
    private readonly ApplicationDbContext Context;

    // Constructor. This constructor is responsible for connecting to the database with the help of the key.
    public AssignmentsDeleteController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Method to handle deleting a task by its Id.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute]int id)
    {
        // Search for the task by its ID.
        var assignment = await Context.Assignments.FindAsync(id);
        
        // If the task does not exist, return an error message.
        if (assignment == null)
        {
            return NotFound("The assignment was not found.");
        }
        // If the task exists, remove it from the database.
        Context.Assignments.Remove(assignment);
        await Context.SaveChangesAsync();
        
        // Return a confirmation message.
        return Ok("The assignment was deleted.");
    }
}

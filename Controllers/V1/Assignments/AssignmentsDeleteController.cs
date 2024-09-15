using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;

namespace MyBackendNemura.Controllers.V1.Assignments;

[ApiController]
[Route("api/[controller]")]
public class AssignmentsDeleteController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public AssignmentsDeleteController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método se encarga de eliminar una tarea por el Id.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute]int id)
    {
        // Buscamos la tarea por su ID.
        var assignment = await Context.Assignments.FindAsync(id);
        
        // Si la tarea no existe, devolvemos un mensaje de error.
        if (assignment == null)
        {
            return NotFound("The assignment was not found.");
        }
        // Si la tarea existe, la eliminamos de la base de datos.
        Context.Assignments.Remove(assignment);
        await Context.SaveChangesAsync();
        
        // Devolvemos un mensaje de confirmación.
        return Ok("The assignment was deleted.");
    }
}

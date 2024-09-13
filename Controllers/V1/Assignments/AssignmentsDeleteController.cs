using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Models;

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

    // Este método se encarga de eliminar una tarea.
    //[HttpDelete]
    //public async Task<IActionResult> Delete(Assignment assignment){}
}

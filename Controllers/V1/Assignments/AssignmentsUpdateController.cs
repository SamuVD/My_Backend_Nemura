using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Models;

namespace MyBackendNemura.Controllers.V1.Assignments;

[ApiController]
[Route("api/[controller]")]
public class AssignmentsUpdateController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public AssignmentsUpdateController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método se encargará de actualizar varias propiedades de una tarea.
    //[HttpPut]
    //public async Task<IActionResult> Put(Assignment assignment){}

    // Este método se encargará de actualizar una sola propiedad de una tarea.
    //[HttpPatch]
    //public async Task<IActionResult> Patch(Assignment assignment){}
}

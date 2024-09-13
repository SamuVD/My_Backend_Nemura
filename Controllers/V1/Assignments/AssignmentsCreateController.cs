using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Models;

namespace MyBackendNemura.Controllers.V1.Assignments;

[ApiController]
[Route("api/[controller]")]
public class AssignmentsCreateController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public AssignmentsCreateController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método se encargará de crear una tarea.
    //[HttpPost]
    //public async Task<IActionResult> Post(Assignment assignment){}
}

using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Models;

namespace MyBackendNemura.Controllers.V1.Assignments;

[ApiController]
[Route("api/v1/assignments")]
public class AssignmentsGetController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public AssignmentsGetController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método se encargará de traer una tarea.
    //[HttpGet]
    //public async Task<IActionResult> Get(Assignment assignment){}
}

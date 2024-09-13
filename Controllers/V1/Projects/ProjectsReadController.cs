using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Models;

namespace MyBackendNemura.Controllers.V1.Projects;

[ApiController]
[Route("api/[controller]")]
public class ProjectsReadController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public ProjectsReadController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método se va a encargar de devolver todos los proyectos.
    //[HttpGet]
    //public async Task<IActionResult> Get(Project project){}
}

using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Models;

namespace MyBackendNemura.Controllers.V1.Projects;

[ApiController]
[Route("api/[controller]")]
public class ProjectsDeleteController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public ProjectsDeleteController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método va a ser llamado cuando se quiera borrar un proyecto.
    //[HttpDelete]
    //public async Task<IActionResult> Delete(Project project){}
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Models;

namespace MyBackendNemura.Controllers.V1.Projects;

[ApiController]
[Route("api/[controller]")]
public class ProjectsCreateController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public ProjectsCreateController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método va a crear un nuevo proyecto en la base de datos.
    //[HttpPost]
    //public async Task<IActionResult> Post([FromBody] Project project){}
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Project project)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Context.Projects.Add(project);
        await Context.SaveChangesAsync();
        return Ok("Project has been successfully created.");
    }
}

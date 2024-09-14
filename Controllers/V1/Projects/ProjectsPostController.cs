using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Project;
using MyBackendNemura.Models;

namespace MyBackendNemura.Controllers.V1.Projects;

[ApiController]
[Route("api/v1/projects")]
public class ProjectsPostController : ControllerBase
{
    // Esta propiedad es nuestra llave para entrar a la base de datos.
    private readonly ApplicationDbContext Context;

    // Builder. Este constructor se va a encargar de hacerme la conexión con la base de datos con ayuda de la llave.
    public ProjectsPostController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Este método va a crear un nuevo proyecto en la base de datos.
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProjectPostDto projectPostDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var project = new Project
        {
            Name = projectPostDto.Name,
            UserId = projectPostDto.UserId
        };

        Context.Projects.Add(project);
        await Context.SaveChangesAsync();
        return Ok("Project has been successfully created.");
    }
}

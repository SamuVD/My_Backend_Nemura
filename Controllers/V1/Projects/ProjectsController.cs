using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;

namespace MyBackendNemura.Controllers.V1.Projects;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    // This property is our key to access the database.
    private readonly ApplicationDbContext Context;

    // Builder. This constructor will handle connecting to the database with the help of the key.
    public ProjectsController(ApplicationDbContext context)
    {
        Context = context;
    }
}

using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;

namespace MyBackendNemura.Controllers.V1.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    // This property is our key to access the database.
    private readonly ApplicationDbContext Context;

    // Builder. This constructor will handle connecting to the database with the help of the key.
    public UsersController(ApplicationDbContext context)
    {
        Context = context;
    }
}

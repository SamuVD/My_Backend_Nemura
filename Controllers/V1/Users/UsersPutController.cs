using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.User;

namespace MyBackendNemura.Controllers.V1.Users;

[Authorize]
[ApiController]
[Route("api/v1/users")]
public class UsersPutController : ControllerBase
{
    // Esta propiedad se utiliza para interactuar con la base de datos.
    private readonly ApplicationDbContext Context;

    // Constructor del controlador, donde inyectamos la instancia del contexto de la base de datos.
    // El contexto es necesario para realizar operaciones CRUD sobre la base de datos.
    public UsersPutController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Método para actualizar la información del usuario.
    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute] int id, UserPutDto userPutDto)
    {
        var userFound = await Context.Users.FindAsync(id);

        if (userFound == null)
        {
            return NotFound("User not found.");
        }

       userFound.Name = userPutDto.Name;
       userFound.LastName = userPutDto.LastName;
       userFound.NickName = userPutDto.NickName;
       userFound.Email = userPutDto.Email;

       await Context.SaveChangesAsync();
       return Ok("Info has been updated succesfully.");
    }
}

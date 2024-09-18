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
    // This property is used to interact with the database.
    private readonly ApplicationDbContext Context;

    // Controller constructor where we inject the database context instance.
    // The context is necessary to perform CRUD operations on the database.
    public UsersPutController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Method to update user information.
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
       return Ok("Info has been updated successfully.");
    }
}


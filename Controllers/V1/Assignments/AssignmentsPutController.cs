// Importamos las librerías necesarias para trabajar con controladores, Data Base, DTOs, y enums.
using Microsoft.AspNetCore.Mvc;
using MyBackendNemura.DataBase;
using MyBackendNemura.Dtos.Assignment;
using MyBackendNemura.Enums;

namespace MyBackendNemura.Controllers.V1.Assignments;

// Definimos el controlador para manejar las solicitudes relacionadas con la actualización de tareas.
[ApiController]
[Route("api/v1/assignments")]
public class AssignmentsPutController : ControllerBase
{
    // Esta propiedad se utiliza para interactuar con la base de datos.
    private readonly ApplicationDbContext Context;

    // Constructor del controlador, donde inyectamos la instancia del contexto de la base de datos.
    // El contexto permite realizar operaciones CRUD sobre la base de datos.
    public AssignmentsPutController(ApplicationDbContext context)
    {
        Context = context;
    }

    // Método para manejar solicitudes HTTP PUT. Este método actualiza varias propiedades de una tarea específica usando su ID.
    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute] int id, AssignmentPutDto assignmentPutDto)
    {
        // Buscamos la tarea en la base de datos por su ID. Si no la encontramos, 'assignmentFound' será null.
        var assignmentFound = await Context.Assignments.FindAsync(id);

        // Si no encontramos la tarea, devolvemos una respuesta con un estado 404 (Not Found).
        if (assignmentFound == null)
        {
            return NotFound("Assignment not found.");
        }

        // Convertir el valor de Status a Enum
        // Intentamos convertir la cadena recibida en el DTO a un valor del enum AssignmentStatus.
        // Si la conversión falla, devolvemos un error con código 400 Bad Request y un mensaje de valor de estado inválido.
        if (!Enum.TryParse(assignmentPutDto.Status, true, out AssignmentStatus status))
        {
            return BadRequest("Invalid status value.");
        }

        // Convertir el valor de Priority a Enum
        // Intentamos convertir la cadena recibida en el DTO a un valor del enum AssignmentPriority.
        // Si la conversión falla, devolvemos un error con código 400 Bad Request y un mensaje de valor de prioridad inválido.
        if (!Enum.TryParse(assignmentPutDto.Priority, true, out AssignmentPriority priority))
        {
            return BadRequest("Invalid priority value.");
        }

        // Actualizamos las propiedades de la tarea encontrada con los valores proporcionados en el DTO.
        assignmentFound.Name = assignmentPutDto.Name;
        assignmentFound.Description = assignmentPutDto.Description;
        assignmentFound.Status = status;       // Asignamos el valor convertido del estado
        assignmentFound.Priority = priority;   // Asignamos el valor convertido de la prioridad

        // Guardamos los cambios en la base de datos de forma asíncrona.
        await Context.SaveChangesAsync();

        // Devolvemos una respuesta con un estado 200 (OK) indicando que la tarea ha sido actualizada exitosamente.
        return Ok("Assignment has been updated successfully.");
    }
}

using Microsoft.EntityFrameworkCore;
using MyBackendNemura.Models;

namespace MyBackendNemura.DataBase;

public class ApplicationDbContext : DbContext
{
    // Propiedades del ApplicationDbContext para hacer referencia a nuestras clases de Models.
    // Estas propiedades permiten que las clases definidas en Models (User, Project, Assignment)
    // se enlacen con las tablas correspondientes en la base de datos.
    public DbSet<User> Users { get; set; }
    // Representa la tabla "Users" en la base de datos y está vinculada a la clase User.

    public DbSet<Project> Projects { get; set; }
    // Representa la tabla "Projects" en la base de datos y está vinculada a la clase Project.

    public DbSet<Assignment> Assignments { get; set; }
    // Representa la tabla "Assignments" en la base de datos y está vinculada a la clase Assignment.

    // Constructor del ApplicationDbContext.
    // Recibe las opciones (DbContextOptions) necesarias para configurar el contexto de la base de datos.
    // Llama al constructor de la clase base DbContext para inicializar el contexto.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}

using Microsoft.EntityFrameworkCore;
using MyBackendNemura.Models;

namespace MyBackendNemura.DataBase;

public class ApplicationDbContext : DbContext
{
    // Properties of ApplicationDbContext to reference our Model classes.
    // These properties allow the classes defined in Models (User, Project, Assignment)
    // to be linked to the corresponding tables in the database.
    public DbSet<User> Users { get; set; }
    // Represents the "Users" table in the database and is linked to the User class.

    public DbSet<Project> Projects { get; set; }
    // Represents the "Projects" table in the database and is linked to the Project class.

    public DbSet<Assignment> Assignments { get; set; }
    // Represents the "Assignments" table in the database and is linked to the Assignment class.

    // Constructor of ApplicationDbContext.
    // Receives the options (DbContextOptions) needed to configure the database context.
    // Calls the base class DbContext constructor to initialize the context.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}

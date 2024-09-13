using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyBackendNemura.Enums;

namespace MyBackendNemura.Models;

// LAS PROPIEDADES DE ESTA CLASE SERÁN VALIDADAS UTILIZANDO DATA ANNOTATIONS.

// Data Annotation para cambiar el nombre de la entidad en la base de datos.
[Table("assignments")]
public class Assignment
{
    // Data Annotation para hacer la referencia de que esta propiedad será la primary key en la entidad assignments de la base de datos.
    [Key]
    // Data Annotation para cambiar el nombre de la columna en la base de datos.
    [Column("id")]
    public int Id { get; set; }

    [Required]
    // Data Annotation para cambiar el nombre de la columna en la base de datos.
    [Column("name")]
    [MaxLength(255, ErrorMessage = "The name can't be longer than {1} characters.")]
    public string Name { get; set; }

    // Data Annotation para cambiar el nombre de la columna en la base de datos.
    [Column("description")]
    [MaxLength(255, ErrorMessage = "The description can't be longer than {1} characters.")]
    public string? Description { get; set; }

    // Data Annotation para cambiar el nombre de la columna en la base de datos.
    [Column("status")]
    [Required]
    public AssignmentStatus Status { get; set; }

    // Data Annotation para cambiar el nombre de la columna en la base de datos.
    [Column("priority")]
    [Required]
    public AssignmentPriority Priority { get; set; }

    // Data Annotation para cambiar el nombre de la columna en la base de datos.
    [Column("project_id")]
    public int ProjectId { get; set; }

    // Enlaces foraneos
    [ForeignKey("ProjectId")]
    public Project Project { get; set; }
}

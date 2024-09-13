using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendNemura.Models;

// LAS PROPIEDADES DE ESTA CLASE SERÁN VALIDADAS UTILIZANDO DATA ANNOTATIONS.

// Data Annotation para cambiar el nombre de la entidad en la base de datos.
[Table("projects")]
public class Project
{
    // Data Annotation para hacer la referencia de que esta propiedad será la primary key en la entidad assignments de la base de datos.
    [Key]
    // Data Annotation para cambiar el nombre de la columna en la base de datos.
    [Column("id")]
    public int Id { get; set; }

    // Data Annotation para cambiar el nombre de la columna en la base de datos.
    [Column("name")]
    // Data Annotation para indicar que esta propiedad debe ser NOT NULL en la base de datos.
    [Required]
    // Data Annotation para indicar que esta propiedad debe tener una longitud máxima de 255 caracteres.
    [MaxLength(255, ErrorMessage = "The project name can't be longer than {1} characters.")]
    // Data Annotation para indicar que esta propiedad debe tener una longitud mínima de 255 caracteres.
    [MinLength(4, ErrorMessage = "The project name can't be shorter than {1} characters.")]
    public string Name { get; set; }

    // Data Annotation para cambiar el nombre de la columna en la base de datos.
    [Column("user_id")]
    public int UserId { get; set; }

    // Data Annotation para hacer referencia a una foreignkey. Va a estar relacionado con la propiedad de UserId.
    [ForeignKey("UserId")]
    // Enlaces foraneos
    public User User { get; set; }
}

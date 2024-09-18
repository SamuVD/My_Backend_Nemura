using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendNemura.Models;

// THE PROPERTIES OF THIS CLASS WILL BE VALIDATED USING DATA ANNOTATIONS.

// Data annotation to change the name of the entity in the database.
[Table("projects")]
public class Project
{
    // Data annotation to reference that this property will be the primary key in the database's assignments entity.
    [Key]
    // Data annotation to change the column name in the database.
    [Column("id")]
    public int Id { get; set; }

    // Data annotation to change the column name in the database.
    [Column("name")]
    // Data annotation to indicate that this property should be NOT NULL in the database.
    [Required]
    // Data annotation to indicate that this property should have a maximum length of 255 characters.
    [MaxLength(255, ErrorMessage = "The project name can't be longer than {1} characters.")]
    // Data annotation to indicate that this property should have a minimum length of 4 characters.
    [MinLength(4, ErrorMessage = "The project name can't be shorter than {1} characters.")]
    public string Name { get; set; }

    // Data annotation to change the column name in the database.
    [Column("user_id")]
    public int UserId { get; set; }

    // Data annotation to reference a foreign key. It will be related to the UserId property.
    [ForeignKey("UserId")]
    // Foreign key links
    public User User { get; set; }
}

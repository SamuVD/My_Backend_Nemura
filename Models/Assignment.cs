using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyBackendNemura.Enums;

namespace MyBackendNemura.Models;

// THE PROPERTIES OF THIS CLASS WILL BE VALIDATED USING DATA ANNOTATIONS.

// Data annotation to change the name of the entity in the database.
[Table("assignments")]
public class Assignment
{
    // Data annotation to reference that this property will be the primary key in the database's assignments entity.
    [Key]
    // Data annotation to change the column name in the database.
    [Column("id")]
    public int Id { get; set; }

    [Required]
    // Data annotation to change the column name in the database.
    [Column("name")]
    [MaxLength(255, ErrorMessage = "The name can't be longer than {1} characters.")]
    public string Name { get; set; }

    // Data annotation to change the column name in the database.
    [Column("description")]
    [MaxLength(255, ErrorMessage = "The description can't be longer than {1} characters.")]
    public string? Description { get; set; }

    // Data annotation to change the column name in the database.
    [Column("status")]
    [Required]
    public AssignmentStatus Status { get; set; }

    // Data annotation to change the column name in the database.
    [Column("priority")]
    [Required]
    public AssignmentPriority Priority { get; set; }

    // Data annotation to change the column name in the database.
    [Column("project_id")]
    public int ProjectId { get; set; }

    // Foreign key links
    [ForeignKey("ProjectId")]
    public Project Project { get; set; }
}


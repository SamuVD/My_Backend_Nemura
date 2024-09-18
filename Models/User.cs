using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendNemura.Models;

// THE PROPERTIES OF THIS CLASS WILL BE VALIDATED USING DATA ANNOTATIONS.

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    [MaxLength(255, ErrorMessage = "The name can't be longer than {1} characters.")]
    public string Name { get; set; }

    [Required]
    [Column("last_name")]
    [MaxLength(255, ErrorMessage = "The last name can't be longer than {1} characters.")]
    [MinLength(2, ErrorMessage = "The last name can't be longer than {1} character.")]
    public string LastName { get; set; }

    [Required]
    [Column("nick_name")]
    [MaxLength(255, ErrorMessage = "The nickname can't be longer than {1} characters.")]
    public string NickName { get; set; }

    [Required]
    [Column("email")]
    [MaxLength(255, ErrorMessage = "The email can't be longer than {1} characters.")]
    // This Data Annotation validates that the user provides the correct format of an email address.
    [EmailAddress(ErrorMessage = "You must write a correct email format.")]
    // This Data Annotation shows a message about the property's field.
    [Display(Name = "example@gmail.com")]
    public string Email { get; set; }

    [Required]
    [Column("password")]
    // This Data Annotation indicates that the entered characters will be hidden (e.g., with asterisks or dots).
    [DataType(DataType.Password)]
    // This Data Annotation validates that the password has at least 8 characters and contains at least one number, one uppercase letter, and one lowercase letter.
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number.")]
    [Display(Name = "Must be at least 8 characters, one uppercase letter, one lowercase, and one number.")]
    public string Password { get; set; }
}

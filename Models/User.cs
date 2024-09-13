using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendNemura.Models;

// LAS PROPIEDADES DE ESTA CLASE SERÁN VALIDADAS UTILIZANDO DATA ANNOTATIONS.

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
    [MinLength(2, ErrorMessage = "The last name can't be longer than {1} caracter.")]
    public string LastName { get; set; }

    [Required]
    [Column("nick_name")]
    [MaxLength(255, ErrorMessage = "The nick name can't be longer than {1} characters.")]
    public string NickName { get; set; }

    [Required]
    [Column("email")]
    [MaxLength(255, ErrorMessage = "The email can't be longer than {1} characters.")]
    // Esta Data Annotation valida que el usuario ponga el formato correcto de una dirección de correo electrónico.
    [EmailAddress(ErrorMessage = "You must write a correct email format.")]
    // Esta Data Annotation muestra un mensaje sobre el campo de la propiedad.
    [Display(Name = "example@gmail.com")]
    public string Email { get; set; }

    [Required]
    [Column("password")]
    // Esta Data Annotation indica que los caracteres ingresados aparecerán ocultos (por ejemplo, con asteriscos o puntos).
    [DataType(DataType.Password)]
    // Esta Data Annotation valida que la contraseña tenga al menos 8 caracteres y contenga al menos un número, una letra mayúscula, una letra minúscula.
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number.")]
    [Display(Name = "Must be at least 8 characters, one uppercase letter and one lowercase, and one number.")]
    public string Password { get; set; }
}
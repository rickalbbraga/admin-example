using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace API.DTOs.Requests;

[ExcludeFromCodeCoverage]
public class UserRequest
{
    [Microsoft.Build.Framework.Required]
    public string? Name { get; set; }
    
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Microsoft.Build.Framework.Required]
    public DateTime BirthDate { get; set; }

    [Microsoft.Build.Framework.Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required]
    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
}
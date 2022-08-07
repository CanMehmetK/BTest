using System.ComponentModel.DataAnnotations;

namespace BTest.Infrastructure.Identity.DTO;

public class ForgotPasswordRequest
{
  [Required]
  [EmailAddress]
  public string Email { get; set; }
}

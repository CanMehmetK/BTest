using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTest.Mvc.Models
{
  public class Product : BaseEntity<Guid>
  {

    [Required(ErrorMessage = "Please Enter a Valid Name")]
    public string Name { get; set; } = default!;
    public string? Slug { get; set; }
    [Required, MinLength(5, ErrorMessage = "Minimun Length is 5")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Price required")]
    [Column(TypeName = "decimal(8,2)")]
    public decimal Price { get; set; }
    public string Image { get; set; } = "noimage.png";

    [Required(ErrorMessage = "You must choose a category")]
    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }

    public Stock? Stock { get; set; }
    [NotMapped]
    [FileExtension]
    public IFormFile ImageUpload { get; set; }

  }


  public class FileExtensionAttribute : ValidationAttribute
  {
    string[] extensions = { "jpg", "png" };

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value is IFormFile file)
      {
        var extension = Path.GetExtension(file.FileName);
        if (!extensions.Any(ex => extension.EndsWith(ex)))
          return new ValidationResult("Allowed Exxtensions : 'jpg', 'png' .");
      }

      return ValidationResult.Success;

    }
  }
}

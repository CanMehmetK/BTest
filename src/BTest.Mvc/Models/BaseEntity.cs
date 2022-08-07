using System.ComponentModel.DataAnnotations;

namespace BTest.Mvc.Models
{
  public abstract class BaseEntity<TId>
  {
    [Key]
    public TId Id { get; set; }
  }
}

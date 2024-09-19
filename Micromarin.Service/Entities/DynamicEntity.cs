using System.ComponentModel.DataAnnotations;

namespace Micromarin.Services.Entities;

public class DynamicEntity {
    [Key]
    public int Id { get; set; }
    [MaxLength(100, ErrorMessage = "The maximum length for the name is 100 characters.")]
    public required string Type { get; set; }
    [MaxLength(1000, ErrorMessage = "The maximum length for the name is 1000 characters.")]
    public required string Data { get; set; } = "{}";
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

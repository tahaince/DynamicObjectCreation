using System.ComponentModel.DataAnnotations;

namespace Micromarin.Services.Entities;

public class DynamicEntity {
    [Key]
    public int Id { get; set; }
    public required string Type { get; set; }
    public required string Data { get; set; } = "{}";
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Micromarin.Services.Dtos;

public class DynamicUpdateDto {
    [Required]
    public int Id { get; set; }
    [Required]
    public string? Type { get; set; }
    [Required]
    [StringLength(1000)]
    public string? Data { get; set; }
}

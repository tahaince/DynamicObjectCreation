using System.ComponentModel.DataAnnotations;

namespace Micromarin.Services.Dtos;

public class DynamicCommand {
    [Required]
    public required string Type { get; set; }

    [Required]
    [StringLength(1000)]
    public required string Data { get; set; }
}

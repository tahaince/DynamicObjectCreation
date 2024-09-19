namespace Micromarin.Services.Dtos;

public class DynamicQuery {
    public required string Type { get; set; }
    public required string Data { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

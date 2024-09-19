namespace Micromarin.Services.Dtos;

public class DynamicUpsert {
    public int Id { get; set; }
    public required string Type { get; set; }
    public required string Data { get; set; }
}

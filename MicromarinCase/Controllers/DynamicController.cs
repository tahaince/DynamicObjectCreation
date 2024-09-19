using Micromarin.Services.Dtos;
using Micromarin.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace MicromarinCase.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DynamicController(IDynamicService dynamicService) : ControllerBase {
    private readonly IDynamicService _dynamicService = dynamicService;

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var result = await _dynamicService.GetAllDynamicObjectAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) {
        var result = await _dynamicService.GetDynamicQueryByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DynamicCommand dynamicCommand) {
        await _dynamicService.CreateDynamicObjectAsync(dynamicCommand);
        return Created();
    }

    [HttpPatch]
    public async Task<IActionResult> Update(DynamicUpdateDto dynamicUpdateDto) {
        var result = await _dynamicService.UpdateDynamicObjectAsync(dynamicUpdateDto);
        return Ok(result);
    }

    [HttpDelete("{id: int}")]
    public async Task<IActionResult> Delete(int id) {
        await _dynamicService.DeleteDynamicObjectAsync(id);
        return NoContent();
    }
}

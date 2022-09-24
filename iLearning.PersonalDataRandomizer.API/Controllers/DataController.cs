using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace iLearning.PersonalDataRandomizer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataController : ControllerBase
{
    private readonly IDataService _dataService;

    public DataController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpPost]
    public async Task<IActionResult> GeneratePersonalData(RandomOptions options)
    {
        return Ok(await _dataService.GeneratePersonalData(options));
    }
}

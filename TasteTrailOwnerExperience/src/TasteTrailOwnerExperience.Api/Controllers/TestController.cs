using Microsoft.AspNetCore.Mvc;

namespace TasteTrailOwnerExperience.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult GetNumber() {
        var postgresConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
        return Ok(postgresConnectionString);
    }
}

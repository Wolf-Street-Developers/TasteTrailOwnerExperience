using Microsoft.AspNetCore.Mvc;

namespace TasteTrailOwnerExperience.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult GetNumber() {
        return Ok(2);
    }
}

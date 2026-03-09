using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "Catalog Service is running", timestamp = DateTime.UtcNow });
    }
}


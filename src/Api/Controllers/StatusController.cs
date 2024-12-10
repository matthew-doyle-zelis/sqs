using Microsoft.AspNetCore.Mvc;
using Api.Infrastructure.Data;
namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;
    private readonly ActivityContext _context;
    public StatusController(ILogger<StatusController> logger, ActivityContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetStatus(string trackingId)
    {
        var status = await _context.QueueStatuses.FindAsync(trackingId);
        return Ok(status);
    }
}

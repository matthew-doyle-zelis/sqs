using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.Infrastructure.Data;
using Api.Infrastructure.Queue;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ActivityController : ControllerBase
{
    private readonly ILogger<ActivityController> _logger;
    private readonly ActivityContext _context;
    private readonly IQueueService _queueService;

    public ActivityController(ILogger<ActivityController> logger, ActivityContext context, IQueueService queueService)
    {
        _logger = logger;
        _context = context;
        _queueService = queueService;
    }

    [HttpGet]
    public async Task<IActionResult> GetActivities(string id)
    {
        try
        {
            var activity = await _context.Activities.FindAsync(id);
            return Ok(activity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting activities");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]  
    public async Task<IActionResult> CreateActivity(Activity activity)
    {
        await _queueService.EnqueueActivityAsync(activity);

        return Ok(new { message = "Activity created" });
    }
}
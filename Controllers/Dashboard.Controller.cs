
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/dashboard")]
public class Dashboard : ControllerBase
{
    private readonly IDashboardService dashboardService;
    public Dashboard(IDashboardService dashboardService)
    {
        this.dashboardService = dashboardService;
    }

    [HttpGet("fetch-announcements")]
    public IActionResult fetchAnnouncements()
    {
        try
        {
            List<DashboardEntity> res = dashboardService.FetchAnnouncements();
            return Ok(res);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "An unexpected error occured.");
        }
    }
    [HttpPost("post-announcement")]
    public IActionResult postAnnouncement(DashboardDto dashboardDto)
    {
        try
        {
          DashboardResponse res = dashboardService.PostAnnouncement(dashboardDto);
          if(res.Success == true)
          {
            return Ok(res);
          }
          return BadRequest(res);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "An unexpected error occured.");
        }
    }
}

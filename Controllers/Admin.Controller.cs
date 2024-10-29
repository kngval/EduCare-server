
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize(Policy = "admin")]
[ApiController]
[Route("api/admin")]
public class AdminController:ControllerBase {
  
  [HttpGet("test")]
  public IActionResult AdminEndpoint(){
    return Ok("Hi User");
  }
}

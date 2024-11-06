
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize(Policy = "admin")]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{

    private readonly IAdminService adminService;

    public AdminController(IAdminService adminService)
    {
        this.adminService = adminService;
    }
    [HttpGet("test")]
    public IActionResult AdminEndpoint()
    {
        return Ok("Hi User");
    }
    
    //Room Creation Endpoint
    [HttpPost("create-room")]
    public IActionResult CreateRoom(RoomDto roomDto){
      try{
        var res = adminService.CreateRoom(roomDto);
        if(res.Success == false){
          return BadRequest(res);
        }
        return Ok(res);
      } catch(Exception ex){
        Console.WriteLine(ex);
        return StatusCode(500,"An error occured while creating the room");
      }
    }

    [HttpGet("fetch-codes")]
    public IActionResult FetchCodes(){
      try{
        var res = adminService.GetAllCodes(); 
        return Ok(res);
      } catch(Exception ex){
        Console.WriteLine(ex);
        return StatusCode(500, "An error occurred while generating the code.");
      }
    }
      
    //Generate code
    [HttpPost("create-code")]
    public IActionResult CreateCode()
    {
        try
        {
            var res = adminService.GenerateCode(10);
            if (res.Success == false)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "An error occurred while generating the code.");
        }

    }

}


using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/room")]
public class RoomController :ControllerBase {

  private readonly RoomService roomService;

  public RoomController(RoomService roomService){
    this.roomService = roomService;
  }

  [HttpGet("fetchrooms")]
  public IActionResult<List<RoomEntity>> FetchRooms(){
    try{
      var userId = GetUserId();
      if(userId == null){
        return BadRequest("User id is null while fetching rooms");
      }
      var res = roomService.FetchRooms();

    } catch(Exception ex){
      Console.Write(ex.Message);
      return StatusCode(500,"Error occured while fetching rooms");
    }
  }

    private int? GetUserId()
    {
        var userIdClaimValue = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaimValue == null)
        {
            return null;
           
        }
        return int.Parse(userIdClaimValue);
    }
}

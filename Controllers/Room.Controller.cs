
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/room")]
public class RoomController : ControllerBase
{

    private readonly IRoomService roomService;

    public RoomController(IRoomService roomService)
    {
        this.roomService = roomService;
    }
    [HttpGet("fetch-rooms/admin")]
    public IActionResult AdminFetchRooms([FromQuery] string role)
    {
        if(role != "admin"){
          return Unauthorized("Admin Only");
        }
        try
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return BadRequest("User id is null while fetching rooms");
            }
            var res = roomService.AdminFetchRooms();

            return Ok(res);

        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return StatusCode(500, "Error occured while fetching rooms");
        }
    }

    [HttpGet("fetch-rooms/{id}")]
    public IActionResult FetchRoomDetails([FromRoute] int id){
      var res = roomService.FetchRoomDetails(id);
      return Ok(res);
    }

    [HttpPost("create-room")]
    public IActionResult CreateRoom([FromBody] RoomDto roomDto)
    {
        try
        {
            var res = roomService.CreateRoom(roomDto);
            if (res.Success == false)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "An error occured while creating the room");
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

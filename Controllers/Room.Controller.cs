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


    [HttpGet("fetch-rooms")]
    public IActionResult FetchRooms([FromQuery] string role)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized("User Id not found");
        }
        if (role == "admin")
        {
            var res = roomService.AdminFetchRooms();
            return Ok(res);
        }
        else if (role == "teacher")
        {
          var res = roomService.TeacherFetchRooms(userId.Value);
          return Ok(res);
        }
        else
        {
            var res = roomService.FetchRooms(userId.Value);
            return Ok(res);
        }
    }

    [HttpGet("fetch-rooms/{id}")]
    public IActionResult FetchRoomDetails([FromRoute] int id)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return BadRequest("User Id is not present");
        }
        var res = roomService.FetchRoomDetails(id, userId.Value);
        if (res == null)
        {
            return BadRequest("Inaccessible !");
        }
        return Ok(res);
    }

    [HttpPost("join-room")]
    public IActionResult JoinRoom([FromBody] string roomCode)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized("User Id not found");
        }

        var res = roomService.JoinRoom(userId.Value, roomCode);


        if (res.Success == false)
        {
            return BadRequest(res);
        }

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
        var userIdClaimValue = User
            .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        if (userIdClaimValue == null)
        {
            return null;
        }
        return int.Parse(userIdClaimValue);
    }
}

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

    [HttpGet("fetch-rooms/{roomId}")]
    public IActionResult FetchRoomDetails([FromRoute] int roomId)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return BadRequest("User Id is not present");
        }
        var res = roomService.FetchRoomDetails(roomId, userId.Value);
        if (res == null)
        {
            return BadRequest("Inaccessible !");
        }
        return Ok(res);
    }

    [HttpGet("fetch-students/{roomId}")]
    public IActionResult FetchRoomsStudents([FromRoute] int roomId)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized("User Id is not present");
        }

        var res = roomService.FetchRoomsStudent(roomId, userId.Value);
        if (res == null || !res.Any())
        {
            return Ok(new List<RoomToStudentEntity>()); // Return an empty list
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
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized("User Id not found");
        }
        try
        {
            var res = roomService.CreateRoom(roomDto, userId.Value);
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

    [Authorize(Policy = "adminOrTeacher")]
    [HttpDelete("remove-student")]
    public IActionResult RemoveStudent([FromQuery] int studentId, [FromQuery] int roomId)
    {
        try
        {
          var res = roomService.RemoveStudent(studentId,roomId);
          if(res.Success == true)
          {
            return Ok(res);
          }
          return BadRequest(res);
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

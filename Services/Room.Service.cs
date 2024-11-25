using sms_server.Entities;

public class RoomService : IRoomService
{
    private readonly SMSDbContext context;

    public RoomService(SMSDbContext context)
    {
        this.context = context;
    }

    //Fetch Rooms 
    public List<RoomEntity> FetchRooms()
    {
      var room = context.Rooms.ToList();
      return room;
    }

    //Room Details 
    public RoomEntity FetchRoomDetails(int id){
     throw new NotImplementedException(); 
    }
    //Create Room 
    public CreateRoomResponse CreateRoom(RoomDto roomDto)
    {
        if (string.IsNullOrEmpty(roomDto.RoomName) || string.IsNullOrWhiteSpace(roomDto.RoomName))
        {
            return new CreateRoomResponse()
            {
                Success = false,
                Message = "Room Name is required"
            };
        }
        if (string.IsNullOrWhiteSpace(roomDto.teacherEmail) || string.IsNullOrEmpty(roomDto.teacherEmail))
        {
            return new CreateRoomResponse()
            {
                Success = false,
                Message = "Teacher email is required"
            };
        }
        var user = context.Users.Find(roomDto.teacherEmail);
        if (user == null)
        {
            return new CreateRoomResponse()
            {
                Success = false,
                Message = "User not found"
            };
        }

        if (user.Role != "teacher")
        {
            return new CreateRoomResponse()
            {
                Success = false,
                Message = "User is not a valid teacher"
            };
        }

        var room = context.Rooms.Add(new RoomEntity()
        {
            SubjectName = roomDto.RoomName,
            TeacherId = user.Id,
            RoomCode = Helpers.GenerateRandomCode(10) 
        });
        context.SaveChangesAsync();
        return new CreateRoomResponse()
        {
            Success = true,
            Message = "Room created successfully !"
        };
    }

    public CreateRoomResponse DeleteRoom(int id)
    {
      return new CreateRoomResponse(){
        Success = false,
        Message = "Not implemented"
      };
    }

}


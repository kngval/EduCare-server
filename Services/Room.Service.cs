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
    public RoomEntity? FetchRoomDetails(int id){
      var res = context.Rooms.Find(id);
      if(res == null){
        return null;
      }
      return res;
    }
    //Create Room 
    public CreateRoomResponse CreateRoom(string roomName)
    {
        if (string.IsNullOrEmpty(roomName) || string.IsNullOrWhiteSpace(roomName))
        {
            return new CreateRoomResponse()
            {
                Success = false,
                Message = "Room Name is required"
            };
        }
        var user = context.Users.Find(roomName);
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
            SubjectName = roomName,
            TeacherId = null, 
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


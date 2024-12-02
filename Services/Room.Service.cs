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
    public CreateRoomResponse CreateRoom(RoomDto roomDto)
    {
        if (string.IsNullOrEmpty(roomDto.roomName) || string.IsNullOrWhiteSpace(roomDto.roomName))
        {
            return new CreateRoomResponse()
            {
                Success = false,
                Message = "Room Name is required"
            };
        }

        var room = context.Rooms.Add(new RoomEntity()
        {
            SubjectName = roomDto.roomName,
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


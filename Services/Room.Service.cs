using sms_server.Entities;

public class RoomService : IRoomService
{
    private readonly SMSDbContext context;

    public RoomService(SMSDbContext context)
    {
        this.context = context;
    }

    public List<RoomEntity> AdminFetchRooms()
    {
        var room = context.Rooms.ToList();
        room.Sort((x, y) => y.Id.CompareTo(x.Id));
        return room;
    }

    public RoomEntity? FetchRoomDetails(int id)
    {
        // join table Rooms & UserInfo => userJoin;
        var res =
            from room in context.Rooms
            join user in context.UserInfo on room.TeacherId equals user.UserId
            into userJoin
            from user in userJoin.DefaultIfEmpty()
            where room.Id == id
            select new RoomEntity
            {
                Id = room.Id,
                SubjectName = room.SubjectName,
                RoomCode = room.RoomCode,
                TeacherId = room.TeacherId,
                TeacherName = user != null ? user.FirstName + " " + user.LastName : null,
            };

        if (res == null)
        {
            return null;
        }
        return res.FirstOrDefault();
    }

    //Create Room
    public CreateRoomResponse CreateRoom(RoomDto roomDto)
    {
        if (string.IsNullOrEmpty(roomDto.roomName) || string.IsNullOrWhiteSpace(roomDto.roomName))
        {
            return new CreateRoomResponse() { Success = false, Message = "Room Name is required" };
        }

        var room = context.Rooms.Add(
            new RoomEntity()
            {
                SubjectName = roomDto.roomName,
                RoomCode = Helpers.GenerateRandomCode(10),
            }
        );
        context.SaveChangesAsync();
        return new CreateRoomResponse() { Success = true, Message = "Room created successfully !" };
    }

    public CreateRoomResponse DeleteRoom(int id)
    {
        return new CreateRoomResponse() { Success = false, Message = "Not implemented" };
    }
}

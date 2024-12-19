using Microsoft.EntityFrameworkCore;
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

    public List<RoomToStudentEntity> FetchRooms(int userId)
    {
        var userRooms = context.RoomsToStudent.Where(r => r.StudentId == userId).Include(r => r.Room).ToList();
        return userRooms;
    }

    public CreateRoomResponse JoinRoom(int userId, string roomCode)
    {
        var userInfoExists = context.UserInfo.FirstOrDefault(u => u.UserId == userId);
        if(userInfoExists == null) {
          return new CreateRoomResponse {
            Success = false,
            Message = "Finish setting up your profile in 'Account' Tab"
          };
        }
        if (string.IsNullOrEmpty(roomCode) || string.IsNullOrWhiteSpace(roomCode))
        {
            return new CreateRoomResponse
            {
                Success = false,
                Message = "Room code is required"
            };
        }

        var room = context.Rooms.FirstOrDefault(r => r.RoomCode == roomCode);

        if (room == null)
        {

            return new CreateRoomResponse
            {
                Success = false,
                Message = "Room does not exist"
            };
        }

        RoomToStudentEntity rts = new RoomToStudentEntity()
        {
            StudentId = userId,
            RoomId = room.Id,
            Room = room
        };

        context.RoomsToStudent.Add(rts);
        context.SaveChangesAsync();

        return new CreateRoomResponse
        {
            Success = true,
            Message = "Successfully joined the room"
        };

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


    public CreateRoomResponse JoinRoom(string roomCode, int studentId)
    {
        if (string.IsNullOrEmpty(roomCode) || string.IsNullOrWhiteSpace(roomCode))
        {
            return new CreateRoomResponse
            {
                Success = false,
                Message = "Room Code is required"

            };
        }

        RoomEntity? room = context.Rooms.FirstOrDefault(r => r.RoomCode == roomCode);

        if (room == null)
        {
            return new CreateRoomResponse
            {
                Success = false,
                Message = "Room does not exist"
            };
        }

        //check if the student already joined this room, if true then they shouldn't be allowed to join it, if false then they can
        var alreadyJoinedRoom = context.RoomsToStudent.FirstOrDefault(r => r.StudentId == studentId);

        if (alreadyJoinedRoom != null)
        {
            return new CreateRoomResponse
            {
                Success = false,
                Message = "Already joined this room"
            };
        }

        //create the entity if they haven't joined this room
        RoomToStudentEntity roomToStudentLink = new RoomToStudentEntity()
        {
            StudentId = studentId,
            RoomId = room.Id,
            Grade = null
        };

        context.RoomsToStudent.Add(roomToStudentLink);
        context.SaveChangesAsync();

        return new CreateRoomResponse
        {
            Success = true,
            Message = "Successfully joined the room"
        };
    }


    public CreateRoomResponse DeleteRoom(int id)
    {
        return new CreateRoomResponse() { Success = false, Message = "Not implemented" };
    }

}

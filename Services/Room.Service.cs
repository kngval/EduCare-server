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

    public List<RoomEntity> TeacherFetchRooms(int userId)
    {
        var user = context.Users.Find(userId);
        var userRooms = context.Rooms.Where(r => r.TeacherId == userId).ToList();
        userRooms.Sort((x, y) => y.Id.CompareTo(x.Id));
        return userRooms;
    }

    public List<RoomToStudentEntity> FetchRooms(int userId)
    {
        var user = context.Users.Find(userId);
        var userRooms = context.RoomsToStudent.Where(r => r.StudentId == userId).Include(r => r.Room).ToList();
        return userRooms;
    }

    public CreateRoomResponse JoinRoom(int userId, string roomCode)
    {
        var userInfoExists = context.UserInfo.FirstOrDefault(u => u.UserId == userId);
        if (userInfoExists == null)
        {
            return new CreateRoomResponse
            {
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





        //check if the user already joined this room
        var roomJoin = context.RoomsToStudent.Where(s => s.StudentId == userId).FirstOrDefault(r => r.RoomId == room.Id);


        if (roomJoin != null)
        {
            return new CreateRoomResponse
            {
                Success = false,
                Message = "Already joined this room"
            };
        }
        if (userInfoExists.Role == "teacher")
        {
            if (room.TeacherId != null)
            {
                return new CreateRoomResponse
                {
                    Success = false,
                    Message = "A teacher already occupied this room"
                };
            }

            //add teacher in the room
            room.TeacherId = userInfoExists.UserId;
            room.TeacherName = userInfoExists.FirstName + " " + userInfoExists.LastName;
            context.SaveChanges();
            return new CreateRoomResponse
            {
                Success = true,
                Message = "Successfully joined the room"
            };

        }
        var user = context.Users.Find(userId);
        if(user == null) {
          return new CreateRoomResponse  {
            Success = false,
            Message = "User does not exist"
          };
        }
        RoomToStudentEntity rts = new RoomToStudentEntity()
        {
            StudentId = userId,
            Email = user.Email,
            RoomId = room.Id,
            Room = room,
            Role = userInfoExists.Role,
            UserInfoId = userInfoExists.UserId
        };


        context.RoomsToStudent.Add(rts);
        context.SaveChangesAsync();

        return new CreateRoomResponse
        {
            Success = true,
            Message = "Successfully joined the room"
        };

    }

    public RoomEntity? FetchRoomDetails(int roomId, int userId)
    {
        var dbUser = context.Users.Find(userId);
        if (dbUser == null)
        {
            return null;
        }
        if (dbUser.Role == "student")
        {
            var RTSExist = context.RoomsToStudent.Where(r => r.StudentId == dbUser.Id).FirstOrDefault(rm => rm.RoomId == roomId);
            if (RTSExist == null)
            {
                return null;
            }
        }

        // join table Rooms & UserInfo => userJoin;
        var res =
            from room in context.Rooms
            join user in context.UserInfo on room.TeacherId equals user.UserId
            into userJoin
            from user in userJoin.DefaultIfEmpty()
            where room.Id == roomId
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

    public List<RoomToStudentEntity>? FetchRoomsStudent(int roomId, int userId)
    {

        var dbUser = context.Users.Find(userId);
        if (dbUser == null)
        {
            return null;
        }
        if (dbUser.Role == "student")
        {
            var RTSExist = context.RoomsToStudent.Where(r => r.StudentId == dbUser.Id).FirstOrDefault(rm => rm.RoomId == roomId);
            if (RTSExist == null)
            {
                return null;
            }

            var students = context.RoomsToStudent.Where(r => r.RoomId == roomId).Include(u => u.UserInfo).ToList();
            return students;
        }

        return context.RoomsToStudent.Where(r => r.RoomId == roomId).Include(u => u.UserInfo).ToList();
    }

    //REMOVE STUDENT 
    public CreateRoomResponse RemoveStudent(int studentId, int roomId)
    {
      var student = context.RoomsToStudent.Where(r => r.StudentId == studentId).FirstOrDefault(rs => rs.RoomId == roomId); 

      if(student == null)
      {
        return new CreateRoomResponse {
          Success = false,
          Message = "Could not be found"
        };
      }

      context.Remove(student);
      context.SaveChanges();

      return new CreateRoomResponse{
        Success = true,
        Message = "User removed successfully"
      };
    }

    //Create Room
    public CreateRoomResponse CreateRoom(RoomDto roomDto, int userId)
    {
        if (string.IsNullOrEmpty(roomDto.roomName) || string.IsNullOrWhiteSpace(roomDto.roomName))
        {
            return new CreateRoomResponse() { Success = false, Message = "Room Name is required" };
        }

        var user = context.Users.Find(userId);

        if (user == null)
        {
            return new CreateRoomResponse
            {
                Success = false,
                Message = "User not found"
            };
        }

        if (user.Role != "admin")
        {
            return new CreateRoomResponse
            {
                Success = false,
                Message = "User is not an admin"
            };
        }

        var userInfo = context.UserInfo.FirstOrDefault(u => u.UserId == userId);
        if (userInfo == null)
        {
            return new CreateRoomResponse
            {
                Success = false,
                Message = "Finish setting up your profile in 'Account' Tab"
            };
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

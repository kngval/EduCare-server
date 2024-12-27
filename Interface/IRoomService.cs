
public interface IRoomService {
  List<RoomEntity> AdminFetchRooms(); 
  List<RoomEntity> TeacherFetchRooms(int userId);
  List<RoomToStudentEntity> FetchRooms(int userId);
  RoomEntity? FetchRoomDetails(int roomId,int userId);
  List<RoomToStudentEntity>? FetchRoomsStudent(int roomId,int userId);
  CreateRoomResponse CreateRoom(RoomDto roomDto,int userId); 
  CreateRoomResponse JoinRoom(int studentId,string roomCode);
  CreateRoomResponse DeleteRoom(int id);
}


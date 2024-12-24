
public interface IRoomService {
  List<RoomEntity> AdminFetchRooms(); 
  List<RoomEntity> TeacherFetchRooms(int userId);
  List<RoomToStudentEntity> FetchRooms(int userId);
  RoomEntity? FetchRoomDetails(int roomId,int userId);
  RoomToStudentEntity? FetchRoomsStudent(int roomId);
  CreateRoomResponse CreateRoom(RoomDto roomDto); 
  CreateRoomResponse JoinRoom(int studentId,string roomCode);
  CreateRoomResponse DeleteRoom(int id);
}


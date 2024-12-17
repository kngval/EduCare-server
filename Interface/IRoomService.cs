
public interface IRoomService {
  List<RoomEntity> AdminFetchRooms(); 
  List<RoomToStudentEntity> FetchRooms(int userId);
  RoomEntity? FetchRoomDetails(int id);
  CreateRoomResponse CreateRoom(RoomDto roomDto); 
  CreateRoomResponse JoinRoom(int studentId,string roomCode);
  CreateRoomResponse DeleteRoom(int id);
}


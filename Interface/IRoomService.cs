
public interface IRoomService {
  List<RoomEntity> AdminFetchRooms(); 
  List<RoomToStudentEntity> FetchRooms(int userId);
  RoomEntity? FetchRoomDetails(int roomId,int userId);
  CreateRoomResponse CreateRoom(RoomDto roomDto); 
  CreateRoomResponse JoinRoom(int studentId,string roomCode);
  CreateRoomResponse DeleteRoom(int id);
}


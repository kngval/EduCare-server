
public interface IRoomService {
  List<RoomEntity> FetchRooms(); 
  RoomEntity? FetchRoomDetails(int id);
  CreateRoomResponse CreateRoom(string roomName); 
  CreateRoomResponse DeleteRoom(int id);
}


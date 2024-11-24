
public interface IRoomService {
  List<RoomEntity> FetchRooms(); 
  CreateRoomResponse CreateRoom(RoomDto roomDto); 
  CreateRoomResponse DeleteRoom(int id);
}



public interface IRoomService {
  List<RoomEntity> FetchRooms(); 
  RoomEntity FetchRoomDetails(int id);
  CreateRoomResponse CreateRoom(RoomDto roomDto); 
  CreateRoomResponse DeleteRoom(int id);
}


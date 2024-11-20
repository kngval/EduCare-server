
public interface IRoomService {
  List<RoomEntity> FetchRooms(); 
  CreateRoomResponse CreateRoom(RoomDto roomDto); 
}

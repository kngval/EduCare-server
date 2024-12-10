
public interface IRoomService {
  List<RoomEntity> AdminFetchRooms(); 
  RoomEntity? FetchRoomDetails(int id);
  CreateRoomResponse CreateRoom(RoomDto roomDto); 
  CreateRoomResponse DeleteRoom(int id);
}


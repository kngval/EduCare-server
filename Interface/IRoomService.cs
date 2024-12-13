
public interface IRoomService {
  List<RoomEntity> AdminFetchRooms(); 
  List<RoomEntity> FetchRooms(int userId);
  RoomEntity? FetchRoomDetails(int id);
  CreateRoomResponse CreateRoom(RoomDto roomDto); 
  CreateRoomResponse DeleteRoom(int id);
}


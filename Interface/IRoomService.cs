
public interface IRoomService {
  List<RoomEntity> AdminFetchRooms(); 
  List<RooomEntity> FetchRooms(int userId)
  RoomEntity? FetchRoomDetails(int id);
  CreateRoomResponse CreateRoom(RoomDto roomDto); 
  CreateRoomResponse DeleteRoom(int id);
}


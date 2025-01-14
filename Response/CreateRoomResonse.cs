public class CreateRoomResponse {
  public bool Success {get;set;}
  public required string Message {get;set;}
  public RoomEntity? Room {get;set;}  
}

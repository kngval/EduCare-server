
public class RoomEntity {
  public int Id {get;set;}
  public required string  Name {get;set;}
  

  //Foreign Keys
  public int TeacherId {get;set;}
  public int StudentId {get;set;}
}

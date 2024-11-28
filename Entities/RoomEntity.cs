
public class RoomEntity {
  public int Id {get;set;}
  public required string RoomCode {get;set;}
  public required string  SubjectName {get;set;}
  public int? TeacherId {get;set;} //Foreign Key pointing to UserInfo that have a teacher role 
}

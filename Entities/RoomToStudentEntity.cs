
public class RoomToStudentEntity {
  public int Id {get;set;}
  //Foreign Keys
  public int StudentId {get;set;} //FK pointing to UserInfo that have a student role
  public int RoomCode {get;set;} // FK pointing to RoomEntity
  public int Grade {get;set;}
}

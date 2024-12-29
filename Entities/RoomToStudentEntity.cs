
public class RoomToStudentEntity {
  public int Id {get;set;}
  //Foreign Keys
  public int StudentId {get;set;} //FK pointing to UserInfo that have a student role
  public string? Email {get;set;}
  public int RoomId {get;set;} // FK pointing to RoomEntity
  public int UserInfoId {get;set;} //FK pointing to UserInfo
  public int? Grade {get;set;}
  public string? Role {get;set;}
  public RoomEntity? Room{get;set;}
  public UserInfoEntity? UserInfo {get;set;}
}

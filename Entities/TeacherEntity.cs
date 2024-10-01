namespace sms_server.Entities;
public class TeacherEntity {
  public int Id {get;set;}
  public required string Name {get;set;}
  public int UserId {get;set;} //foreign key
}

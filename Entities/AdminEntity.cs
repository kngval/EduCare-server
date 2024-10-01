
namespace sms_server.Entities;

public class AdminEntity {
  public int Id {get;set;}
  public required string Name {get;set;}
  public int UserId {get;set;} //foreign key
}

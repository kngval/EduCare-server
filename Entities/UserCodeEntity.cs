
namespace sms_server.Entities;

public class UserCodeEntity {
  public int Id {get;set;}
  public required string Code {get;set;}
  public int? UserId {get;set;}
  public bool Available {get;set;}
}

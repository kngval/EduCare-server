
namespace sms_server.Entities;
public class UserEntity {
  public int Id {get;set;} //primary key
  public required string Email {get;set;}
  public required string Password {get;set;}
  public required string Role {get;set;}
}

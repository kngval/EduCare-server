
namespace sms_server.Entities;

public class AuthResponse {
  public bool Success {get;set;}
  public required string Message{get;set;}
  public UserEntity? User{get;set;}

}

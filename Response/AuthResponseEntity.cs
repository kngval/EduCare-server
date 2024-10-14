using sms_server.Entities;
namespace sms_server.Response;

public class AuthResponse {
  public bool Success {get;set;}
  public required string Message{get;set;}
  public UserEntity? User{get;set;}
  public string? Field {get;set;}
  public string? Token {get;set;}
}

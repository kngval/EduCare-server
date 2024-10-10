
namespace sms_server.Entities;

public class AdminEntity {
  public int Id {get;set;}
  public required string Email {get;set;}
  public required string Password {get;set;}
  public required string Code {get;set;}

  public string? Name {get;set;}
}

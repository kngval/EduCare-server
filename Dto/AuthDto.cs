
namespace sms_server.Dtos;
public record SignUpDto(string email, string password,string role);

public record LoginDto(string email, string password);

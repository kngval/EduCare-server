

//Interface for AuthService
using sms_server.Dtos;
using sms_server.Entities;

public interface IAuthInterface {
  Task<AuthResponse> SignUp(SignUpDto authDto); 
  Task<string> Login(LoginDto loginDto);
}



//Interface for AuthService
using sms_server.Dtos;
using sms_server.Entities;

public interface IAuthInterface {
  Task<UserEntity?> SignUp(AuthDto authDto); 
}

using sms_server.Dtos;
using sms_server.Entities;

namespace educare_server.Mapping;

public static class AuthMapping {

  
  public static UserEntity toAuthEntity(this SignUpDto userDto)
  {
    return new UserEntity()
    {
      Email = userDto.email,
      Password = userDto.password,
      Role = userDto.role
    };
  }

}

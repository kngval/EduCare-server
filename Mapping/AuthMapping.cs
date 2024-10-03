

using sms_server.Dtos;
using sms_server.Entities;

namespace sms_server.Mapping;

public static class AuthMapping {

  
  public static UserEntity toAuthEntity(this AuthDto userDto)
  {
    return new UserEntity()
    {
      Email = userDto.email,
      Password = userDto.password,
      Role = userDto.role
    };
  }

}

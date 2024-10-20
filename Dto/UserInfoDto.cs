

namespace sms_server.Dtos;

public record UserInfoDto(string FirstName,string MiddleName,string LastName,string Role,DateOnly Birthdate,int userId);

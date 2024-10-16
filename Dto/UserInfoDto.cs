

namespace sms_server.Dtos;

public record UserInfoDto(string FirstName,string MiddleName,string LastName,DateOnly Birthdate,int userId);



namespace sms_server.Dtos;

public record UserInfoDto(string FirstName, string MiddleName, string LastName, string Role, DateOnly Birthdate, string Country, string State, string City, string PostalCode, int userId);

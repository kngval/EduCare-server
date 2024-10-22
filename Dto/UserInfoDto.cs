

namespace sms_server.Dtos;

public record UserInfoDto(string FirstName, string LastName, string Role, DateOnly Birthdate, string Country, string State, string City, string PostalCode,string Gender,string? LRN);

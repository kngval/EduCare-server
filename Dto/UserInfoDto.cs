

namespace sms_server.Dtos;

public record UserInfoDto(string FirstName="" , string LastName="", string Role="",string Phone="", DateOnly? Birthdate = null, string Country="", string State="", string City="", string PostalCode="",string Gender="",string LRN="");

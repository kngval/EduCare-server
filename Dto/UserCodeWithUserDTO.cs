using sms_server.Entities;

public class UserCodeWithUserDTO
{
    public required UserCodeEntity UserCode { get; set; }
    public string? UserEmail { get; set; }
}


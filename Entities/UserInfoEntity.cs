

public class UserInfoEntity {
  public int Id {get;set;}
  public string FirstName {get;set;} = string.Empty;
  public string LastName {get;set;} = string.Empty; 
  public required string Role {get;set;} 
  public string? Phone {get;set;}
  public DateOnly Birthdate {get;set;}
  public string Country {get;set;} = string.Empty;
  public string State {get;set;} = string.Empty;
  public string City {get;set;} = string.Empty;
  public string PostalCode {get;set;} = string.Empty;
  public int UserId {get;set;} //foreign key for UserEntity.Id

}

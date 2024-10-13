

public class UserInfoEntity {
  public int Id {get;set;}
  public string FirstName {get;set;} = string.Empty;
  public string? Middlename {get;set;} 
  public string LastName {get;set;} = string.Empty; 
  public DateOnly Birthdate {get;set;}

  public int UserId {get;set;} //foreign key for UserEntity.Id

}

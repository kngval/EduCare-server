


public class UserInfoService:IUserInfoInterface {
  private readonly SMSDbContext context; 
  
  //UserInfo Constructor
  public UserInfoService(SMSDbContext context){
    this.context = context; 
  }
}




using sms_server.Dtos;

public class UserInfoService:IUserInfoInterface {
  private readonly SMSDbContext context; 
   
  //UserInfo Constructor
  public UserInfoService(SMSDbContext context){
    this.context = context; 

  }

    public Task<UserInfoResponse> CreateUserInfo(UserInfoDto userInfo)
    {
        throw new NotImplementedException();
    }

    public Task<UserInfoResponse> GetUserInfo(int id)
    {
        throw new NotImplementedException();
    }
}

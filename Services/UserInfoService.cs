


using sms_server.Dtos;

public class UserInfoService : IUserInfoInterface
{
    private readonly SMSDbContext context;

    //UserInfo Constructor
    public UserInfoService(SMSDbContext context)
    {
        this.context = context;

    }
    public UserInfoEntity? FetchUser(int id)
    {
        return context.UserInfo.FirstOrDefault(u => u.UserId == id);
    }

    public async Task<UserInfoResponse> CreateUserInfo(UserInfoDto userInfo)
    {
        if(string.IsNullOrEmpty(userInfo.FirstName))
        {
          return new UserInfoResponse()
          {
            Success = false,
            Message = "First Name is required.",
            Field = "firstName"
          };
        }

        if(string.IsNullOrEmpty(userInfo.LastName))
        {
          return new UserInfoResponse()
          {
            Success = false,
            Message = "Last Name is required.",
            Field = "firstName"
          };
        }
        if(string.IsNullOrEmpty(userInfo.FirstName))
        {
          return new UserInfoResponse()
          {
            Success = false,
            Message = "First Name is required.",
            Field = "firstName"
          };
        }
       var user = FetchUser(userInfo.userId);

        if (user == null)
        {
            var newUserInfo = await context.UserInfo.AddAsync(new UserInfoEntity()
            {
                FirstName = userInfo.FirstName,
                MiddleName = userInfo.MiddleName,
                LastName = userInfo.LastName,
                Birthdate = userInfo.Birthdate,
                UserId = userInfo.userId
            });

            await context.SaveChangesAsync();
            return new UserInfoResponse()
            {
                Success = true,
                Message = "User Information added successfully !",
            };
        }
        else
        {
            user.FirstName = userInfo.FirstName;
            user.MiddleName = userInfo.MiddleName;
            user.LastName = userInfo.LastName;
            user.Birthdate = userInfo.Birthdate;
            user.UserId = userInfo.userId;
            await context.SaveChangesAsync();

            return new UserInfoResponse()
            {
                Success = true,
                Message = "User Information updated successfully !"
            };
        }
    }

}

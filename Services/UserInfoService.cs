


using sms_server.Dtos;

public class UserInfoService : IUserInfoInterface
{
    private readonly SMSDbContext context;

    //UserInfo Constructor
    public UserInfoService(SMSDbContext context)
    {
        this.context = context;

    }
    public UserInfoEntity? GetUserInfo(int id)
    {
        return context.UserInfo.FirstOrDefault(u => u.UserId == id);
    }

    public async Task<UserInfoResponse> CreateUserInfo(UserInfoDto userInfo)
    {

        var user = GetUserInfo(userInfo.userId);

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

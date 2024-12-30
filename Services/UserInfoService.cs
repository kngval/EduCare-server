using System.Security.Claims;
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

    public async Task<UserInfoResponse> CreateUserInfo(UserInfoDto userInfo, int userId)
    {

        var user = FetchUser(userId);

        if (user == null)
        {
            if(userInfo.age == 0){
                return new UserInfoResponse()
                {
                    Success = false,
                    Message = "Age is required",
                    Field = "age"
                };

            }
            if (string.IsNullOrEmpty(userInfo.FirstName))
            {
                return new UserInfoResponse()
                {
                    Success = false,
                    Message = "First Name is required.",
                    Field = "personal"
                };
            }

            if (string.IsNullOrEmpty(userInfo.LastName))
            {
                return new UserInfoResponse()
                {
                    Success = false,
                    Message = "Last Name is required.",
                    Field = "personal"
                };
            }
            if (string.IsNullOrEmpty(userInfo.Role))
            {
                return new UserInfoResponse()
                {
                    Success = false,
                    Message = "Role is required.",
                };
            }

            if (string.IsNullOrEmpty(userInfo.Country))
            {
                return new UserInfoResponse()
                {
                    Success = false,
                    Message = "Country is required.",
                    Field = "address"
                };
            }
            if (string.IsNullOrEmpty(userInfo.PostalCode))
            {
                return new UserInfoResponse()
                {
                    Success = false,
                    Message = "Postal Code is required.",
                    Field = "address"
                };
            }
            if (string.IsNullOrEmpty(userInfo.City))
            {
                return new UserInfoResponse()
                {
                    Success = false,
                    Message = "City is required.",
                    Field = "address"
                };
            }
            if (string.IsNullOrEmpty(userInfo.State))
            {
                return new UserInfoResponse()
                {
                    Success = false,
                    Message = "State is required.",
                    Field = "address"
                };
            }




            var newUserInfo = await context.UserInfo.AddAsync(new UserInfoEntity()
            {
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Role = userInfo.Role,
                Gender = userInfo.Gender,
                Phone = userInfo.Phone,
                LRN = userInfo.LRN,
                Country = userInfo.Country,
                State = userInfo.State,
                City = userInfo.City,
                PostalCode = userInfo.PostalCode,
                Birthdate = userInfo.Birthdate,
                UserId = userId
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
            if (!string.IsNullOrEmpty(userInfo.FirstName))
            {
                user.FirstName = userInfo.FirstName;
            }


            if (!string.IsNullOrEmpty(userInfo.LastName))
            {
                user.LastName = userInfo.LastName;
            }
            if (!string.IsNullOrEmpty(userInfo.Role))
            {
                user.Role = userInfo.Role;
            }
            if (!string.IsNullOrEmpty(userInfo.Gender))
            {
                user.Gender = userInfo.Gender;
            }
            if (!string.IsNullOrEmpty(userInfo.Phone))
            {
                user.Phone = userInfo.Phone;
            }
            if (!string.IsNullOrEmpty(userInfo.LRN))
            {
                user.LRN = userInfo.LRN;
            }
            if (!string.IsNullOrEmpty(userInfo.Country))
            {
                user.Country = userInfo.Country;
            }
            if (!string.IsNullOrEmpty(userInfo.State))
            {
                user.State = userInfo.State;
            }
            if (!string.IsNullOrEmpty(userInfo.City))
            {
                user.City = userInfo.City;
            }
            if (!string.IsNullOrEmpty(userInfo.PostalCode))
            {
                user.PostalCode = userInfo.PostalCode;
            }
            await context.SaveChangesAsync();

            return new UserInfoResponse()
            {
                Success = true,
                Message = "User Information updated successfully !"
            };
        }
    }

}

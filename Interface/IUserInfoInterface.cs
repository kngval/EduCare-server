
using sms_server.Dtos;
public interface IUserInfoInterface {
  Task<UserInfoResponse> GetUserInfo(int id);
  Task<UserInfoResponse> CreateUserInfo(UserInfoDto userInfo);
}

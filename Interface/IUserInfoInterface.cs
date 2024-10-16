
using sms_server.Dtos;
public interface IUserInfoInterface {
  UserInfoEntity? GetUserInfo(int id);
  Task<UserInfoResponse> CreateUserInfo(UserInfoDto userInfo);
}

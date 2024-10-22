
using sms_server.Dtos;
public interface IUserInfoInterface {
  UserInfoEntity? FetchUser(int id);
  Task<UserInfoResponse> CreateUserInfo(UserInfoDto userInfo,int userId);
}
